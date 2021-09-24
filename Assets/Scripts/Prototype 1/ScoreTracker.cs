using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{
    #region Singleton
    public static ScoreTracker instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of ScoreTracker found");
            return;
        }
        instance = this;
    }
    #endregion

    public int score = 0;
    public int highscore = 0;
    public Text scoreT;

    private void Start()
    {
        scoreT.text = score.ToString();
    }

    public void UpdateScore(int points)
    {
        score += points;
        scoreT.text = score.ToString();
    }
}
