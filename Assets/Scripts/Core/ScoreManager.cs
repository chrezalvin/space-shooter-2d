using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static Action<int> OnScoreChanged;
    public int initialScore = 0;
    private int m_currentScore;

    // Start is called before the first frame update
    void Start()
    {
        m_currentScore = initialScore;

        Debug.Log("ScoreManager initialized with score: " + m_currentScore);
    }

    public int AddScore(Enemy enemy)
    {
        if (enemy == null)
        {
            Debug.LogWarning("Attempted to add score for a null enemy.");
            return m_currentScore;            
        }

        int score = Mathf.FloorToInt(enemy.size * enemy.speed);
        m_currentScore += score == 0 ? 1 : score;

        OnScoreChanged?.Invoke(m_currentScore);

        Debug.Log("Score added: " + score + ", current score: " + m_currentScore);

        return m_currentScore;
    }

    public int GetCurrentScore()
    {
        return m_currentScore;
    }
}