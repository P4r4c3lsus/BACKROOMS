using UnityEngine;

public class PedestalController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject correctObject;
    [SerializeField] private DoorController doorController;

    public bool IsSolved { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != correctObject &&
            (other.attachedRigidbody == null || other.attachedRigidbody.gameObject != correctObject))
        {
            return;
        }

        IsSolved = true;

        if (doorController != null)
        {
            doorController.OpenDoor();
        }
    }
}
