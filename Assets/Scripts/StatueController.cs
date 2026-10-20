using UnityEngine;

public class StatueController : MonoBehaviour
{
    [Header("Statues")]
    [SerializeField] private Transform statue1;
    [SerializeField] private Transform statue2;
    [SerializeField] private Transform statue3;

    [Header("Correct Positions")]
    [SerializeField] private int correctPosition1 = 1;
    [SerializeField] private int correctPosition2 = 2;
    [SerializeField] private int correctPosition3 = 3;

    [Header("Next clue")]
    [SerializeField] private GameObject clueObject;

    private int currentPosition1 = 0;
    private int currentPosition2 = 0;
    private int currentPosition3 = 0;

    private bool puzzleCompleted = false;

    private void Start()
    {
        if (clueObject != null)
        {
            clueObject.SetActive(false);
        }
    }

    public void RotateStatue1()
    {
        if (puzzleCompleted)
        {
            return;
        }

        statue1.Rotate(0f, 90f, 0f);
        currentPosition1++;

        if (currentPosition1 > 3)
        {
            currentPosition1 = 0;
        }

        CheckPuzzle();
    }

    public void RotateStatue2()
    {
        if (puzzleCompleted)
        {
            return;
        }

        statue2.Rotate(0f, 90f, 0f);
        currentPosition2++;

        if (currentPosition2 > 3)
        {
            currentPosition2 = 0;
        }

        CheckPuzzle();
    }

    public void RotateStatue3()
    {
        if (puzzleCompleted)
        {
            return;
        }

        statue3.Rotate(0f, 90f, 0f);
        currentPosition3++;

        if (currentPosition3 > 3)
        {
            currentPosition3 = 0;
        }

        CheckPuzzle();
    }

    private void CheckPuzzle()
    {
        if (currentPosition1 == correctPosition1 && currentPosition2 == correctPosition2 && currentPosition3 == correctPosition3)
        {
            CompletePuzzle();
        }
    }

    private void CompletePuzzle()
    {
        puzzleCompleted = true;

        if (clueObject != null)
        {
            clueObject.SetActive(true);
        }
    }
}

