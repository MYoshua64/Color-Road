using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private void Awake()
    {
        instance = this;
    }

    public int currentScore;
    public int multiplier = 1;

    public System.Action<int> OnScoreUpdated;

    public void Score()
    {
        currentScore += multiplier;
        OnScoreUpdated?.Invoke(currentScore);
    }
}
