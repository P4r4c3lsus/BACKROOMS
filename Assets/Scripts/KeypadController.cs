using TMPro;
using UnityEngine;

public class KeypadController : MonoBehaviour
{
    [Header("Code")]
    [SerializeField] private string correctCode = "1034";
    [SerializeField] private int maxDigits = 4;

    [Header("References")]
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private DoorController doorController;

    private string currentCode;
    private bool isUnlocked;

    private void Start()
    {
        currentCode = "";
        isUnlocked = false;

        UpdateDisplay();
    }

    public void PressDigit(int digit)
    {
        if (isUnlocked)
        {
            return;
        }

        if (digit < 0 || digit > 9)
        {
            return;
        }

        if (currentCode.Length >= maxDigits)
        {
            return;
        }

        currentCode += digit.ToString();

        UpdateDisplay();
    }

    public void ClearCode()
    {
        if (isUnlocked)
        {
            return;
        }

        ResetCode();
    }

    public void SubmitCode()
    {
        if (isUnlocked)
        {
            return;
        }

        if (currentCode == correctCode)
        {
            UnlockDoor();
        }
        else
        {
            ResetCode();
        }
    }

    private void UnlockDoor()
    {
        isUnlocked = true;

        if (displayText != null)
        {
            displayText.text = "OPEN";
        }

        if (doorController != null)
        {
            doorController.OpenDoor();
        }
    }

    private void ResetCode()
    {
        currentCode = "";

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (displayText == null)
        {
            return;
        }

        displayText.text = currentCode;
    }
}