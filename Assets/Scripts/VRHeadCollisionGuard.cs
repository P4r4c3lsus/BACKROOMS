using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[DefaultExecutionOrder(10000)]
[RequireComponent(typeof(XROrigin))]
public class VRHeadCollisionGuard : MonoBehaviour
{
    [SerializeField] private float headRadius = 0.12f;
    [SerializeField] private float wallSkin = 0.015f;
    [SerializeField] private LayerMask obstacleLayers = ~0;

    private readonly RaycastHit[] hits = new RaycastHit[128];
    private readonly Collider[] overlaps = new Collider[128];
    private XROrigin origin;
    private CharacterController body;
    private SphereCollider probe;
    private Vector3 safeHeadPosition;
    private bool initialized;

    private void Awake()
    {
        origin = GetComponent<XROrigin>();
        body = GetComponent<CharacterController>();
        var probeObject = new GameObject("Head Collision Query") { hideFlags = HideFlags.HideAndDontSave };
        probe = probeObject.AddComponent<SphereCollider>();
        probe.radius = headRadius;
        // Used only by ComputePenetration; it never participates in scene physics.
        probe.enabled = false;
    }

    private void OnEnable()
    {
        initialized = false;
        Application.onBeforeRender += BeforeRender;
    }

    private void OnDisable() => Application.onBeforeRender -= BeforeRender;

    private void OnDestroy()
    {
        if (probe != null) Destroy(probe.gameObject);
    }

    private void LateUpdate() => ConstrainHead();

    [BeforeRenderOrder(100)]
    private void BeforeRender() => ConstrainHead();

    private bool IsObstacle(Collider collider)
    {
        if (collider == null || collider == probe || collider.isTrigger ||
            collider.transform.IsChildOf(transform) ||
            collider.gameObject.scene != gameObject.scene) return false;
        // Hands, held clues and puzzle controls must not push the whole player around.
        if (collider.GetComponentInParent<XRBaseInteractable>() != null) return false;
        return collider.attachedRigidbody == null || collider.attachedRigidbody.isKinematic;
    }

    private Vector3 Slide(Vector3 from, Vector3 requested)
    {
        Vector3 position = from;
        Vector3 remaining = requested - from;
        for (int pass = 0; pass < 4 && remaining.sqrMagnitude > 0.0000001f; pass++)
        {
            float distance = remaining.magnitude;
            Vector3 direction = remaining / distance;
            int count = Physics.SphereCastNonAlloc(position, headRadius, direction, hits,
                distance + wallSkin, obstacleLayers, QueryTriggerInteraction.Ignore);
            float nearest = float.PositiveInfinity;
            Vector3 normal = Vector3.zero;
            for (int i = 0; i < count; i++)
            {
                if (!IsObstacle(hits[i].collider) || hits[i].distance >= nearest) continue;
                nearest = hits[i].distance;
                normal = hits[i].normal;
            }
            if (float.IsPositiveInfinity(nearest)) return position + remaining;
            float travel = Mathf.Clamp(nearest - wallSkin, 0f, distance);
            position += direction * travel;
            remaining = Vector3.ProjectOnPlane(remaining - direction * travel, normal);
        }
        return position;
    }

    private Vector3 ResolveOverlap(Vector3 position)
    {
        for (int pass = 0; pass < 4; pass++)
        {
            bool moved = false;
            int count = Physics.OverlapSphereNonAlloc(position, headRadius, overlaps,
                obstacleLayers, QueryTriggerInteraction.Ignore);
            for (int i = 0; i < count; i++)
            {
                Collider other = overlaps[i];
                if (!IsObstacle(other)) continue;
                if (Physics.ComputePenetration(probe, position, Quaternion.identity,
                    other, other.transform.position, other.transform.rotation,
                    out Vector3 direction, out float distance))
                {
                    position += direction * (distance + wallSkin);
                    moved = true;
                }
            }
            if (!moved) break;
        }
        return position;
    }

    private void ConstrainHead()
    {
        if (origin == null || origin.Camera == null || probe == null) return;
        Transform head = origin.Camera.transform;
        Vector3 requested = head.position;
        if (!initialized)
        {
            // Seed from the body position, inside the room, rather than accepting an
            // initial tracked offset on the other side of a wall.
            safeHeadPosition = ResolveOverlap(new Vector3(transform.position.x, requested.y, transform.position.z));
            initialized = true;
        }

        Vector3 allowed = ResolveOverlap(Slide(safeHeadPosition, requested));
        Vector3 correction = allowed - requested;
        if (correction.sqrMagnitude > 0.0000001f)
        {
            // Offset the rig, never the tracked camera: headset rotation stays intact.
            transform.position += correction;
        }
        safeHeadPosition = head.position;

        if (body != null && body.enabled)
        {
            Vector3 localHead = transform.InverseTransformPoint(head.position);
            Vector3 center = body.center;
            center.x = localHead.x;
            center.z = localHead.z;
            if ((center - body.center).sqrMagnitude > 0.0000001f) body.center = center;
        }
    }
}
