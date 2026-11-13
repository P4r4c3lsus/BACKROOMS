using UnityEngine;
using Unity.XR.CoreUtils;

[RequireComponent(typeof(BoxCollider))]
public class DemoGoalZone : MonoBehaviour
{
    [SerializeField] private DoorController finalDoor;
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private Transform playerHead;

    private BoxCollider zone;

    private void Awake()
    {
        zone = GetComponent<BoxCollider>();
        zone.isTrigger = true;
        if (gameTimer == null) gameTimer = FindFirstObjectByType<GameTimer>();
        if (playerHead == null)
        {
            XROrigin origin = FindFirstObjectByType<XROrigin>();
            if (origin != null && origin.Camera != null) playerHead = origin.Camera.transform;
        }
    }

    private void LateUpdate()
    {
        // Check the tracked head, so a hand or a thrown prop cannot finish the demo.
        if (gameTimer == null || !gameTimer.IsRunning || finalDoor == null ||
            !finalDoor.IsOpen || playerHead == null) return;

        Vector3 point = transform.InverseTransformPoint(playerHead.position) - zone.center;
        Vector3 halfSize = zone.size * 0.5f;
        if (Mathf.Abs(point.x) <= halfSize.x && Mathf.Abs(point.y) <= halfSize.y &&
            Mathf.Abs(point.z) <= halfSize.z)
        {
            gameTimer.CompleteDemo();
        }
    }
}
