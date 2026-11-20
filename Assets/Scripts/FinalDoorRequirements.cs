using UnityEngine;

public class FinalDoorRequirements : MonoBehaviour
{
    [SerializeField] private SimondiceBrain simon;
    [SerializeField] private ManagerCriptex wordPuzzle;
    [SerializeField] private PedestalController objectPuzzle;
    [SerializeField] private DoorController finalDoor;

    public bool AllSolved => simon != null && simon.IsSolved &&
                             wordPuzzle != null && wordPuzzle.IsSolved &&
                             objectPuzzle != null && objectPuzzle.IsSolved;

    private void Update()
    {
        // Recheck even if the object was placed before the other puzzles were solved.
        if (AllSolved && finalDoor != null && !finalDoor.IsOpen)
        {
            finalDoor.OpenDoor();
        }
    }
}
