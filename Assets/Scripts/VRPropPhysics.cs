using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Rigidbody), typeof(XRGrabInteractable))]
public class VRPropPhysics : MonoBehaviour
{
    private Rigidbody body;
    private XRGrabInteractable grab;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        grab = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable() => grab.selectExited.AddListener(Release);
    private void OnDisable() => grab.selectExited.RemoveListener(Release);

    private void Release(SelectExitEventArgs args)
    {
        // Preserve the authored placement until the first pickup, then allow normal physics.
        if (args.isCanceled) return;
        body.isKinematic = false;
        body.useGravity = true;
        body.WakeUp();
    }
}
