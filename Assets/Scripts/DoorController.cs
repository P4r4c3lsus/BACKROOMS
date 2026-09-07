using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door")]
    [SerializeField] private Transform doorPivot;

    [Header("Opening")]
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openDuration = 1f;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpen;

    private void Start()
    {
        if (doorPivot == null)
        {
            return;
        }

        closedRotation = doorPivot.localRotation;

        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);

        isOpen = false;
    }

    public void OpenDoor()
    {
        if (isOpen)
        {
            return;
        }

        if (doorPivot == null)
        {
            return;
        }

        isOpen = true;

        StartCoroutine(OpenDoorRoutine());
    }

    private IEnumerator OpenDoorRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < openDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = elapsedTime / openDuration;
            progress = Mathf.Clamp01(progress);

            doorPivot.localRotation = Quaternion.Slerp(closedRotation, openRotation, progress);

            yield return null;
        }

        doorPivot.localRotation = openRotation;
    }
}