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
    }

    public void RotateStatue2()
    {
        if (puzzleCompleted)
        {
            return;
        }
    }

    public void RotateStatue3()
    {
        if (puzzleCompleted)
        {
            return;
        }
    }
}
