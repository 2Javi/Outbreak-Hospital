using UnityEngine;

public class BreakerPuzzle : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private Breaker[] breakers;
    private bool puzzleSolved = false;

    private readonly bool[] correctStates = { true, false, false, false, true, true, false, true };

    void Update()
    {
        if (!puzzleSolved && IsPuzzleSolved())
    {
        puzzleSolved = true;
        Debug.Log("Puzzle Solved!");
    }
    }

    private bool IsPuzzleSolved()
    {
        foreach (Breaker breaker in breakers)
        {
            int index = (int)breaker.breakerID;

            if (index < 0 || index >= correctStates.Length)
                continue;

            if (breaker.isUp != correctStates[index])
                return false;
        }

        return true;
    }
}