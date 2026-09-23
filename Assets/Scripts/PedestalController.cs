using UnityEngine;

public class PedestalController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject correctObject;
    [SerializeField] private DoorController doorController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != correctObject)
        {
            return;
        }

        if (doorController != null)
        {
            doorController.OpenDoor();
        }
    }
}
