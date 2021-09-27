using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    #region Singleton
    public static Timer instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Timer found");
            return;
        }
        instance = this;
    }
    #endregion

    public float timeLeft;
    public Text timerT;

    public GameObject gameOverPanel;
    public Text gameoverScore;
    public Text highscoreT;

    void Update()
    {
        timeLeft -= Time.deltaTime;
        timerT.text = Mathf.Round(timeLeft).ToString();
        if (timeLeft < 0)
        {
            if(ScoreTracker.instance.score > ScoreTracker.instance.highscore)
            {
                ScoreTracker.instance.highscore = ScoreTracker.instance.score;
                highscoreT.text = ScoreTracker.instance.highscore.ToString();
            }
            gameOverPanel.SetActive(true);
            gameoverScore.text = ScoreTracker.instance.score.ToString();
        }
    }

    public void RestartGame()
    {
        timeLeft = 10;
        gameOverPanel.SetActive(false);
        ScoreTracker.instance.score = 0;
        ScoreTracker.instance.UpdateScore(0);

        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        for (int i = 0; i < targets.Length; i++)
        {
            Destroy(targets[i]);
        }

        for (int i = 0; i < 3; i++)
        {
            TargetSpawner.instance.MakeNewTarget();
        }
    }
}
