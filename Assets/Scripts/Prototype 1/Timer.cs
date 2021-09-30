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
    public Text targetsHitT;
    public Text accuracyT;
    public float acc;

    public GameObject gameOverPanel;
    public Text gameoverScore;
    public Text highscoreT;
    private void Start()
    {
        accuracyT.text = "0";
        targetsHitT.text = "0";
    }

    void Update()
    {
        if (StartGame.instance.startPanel.activeInHierarchy == false)
        timeLeft -= Time.deltaTime;

        timerT.text = Mathf.Round(timeLeft).ToString();
        if (timeLeft < 0)
        {
            if(ScoreTracker.instance.score > ScoreTracker.instance.highscore)
            {
                ScoreTracker.instance.highscore = ScoreTracker.instance.score;
            }

            highscoreT.text = ScoreTracker.instance.highscore.ToString();
            targetsHitT.text = ScoreTracker.instance.targetsHit.ToString();
            acc = (ScoreTracker.instance.targetsHit / ScoreTracker.instance.totalClicks) * 100;
            accuracyT.text = acc.ToString("F2") + "%";
            gameOverPanel.SetActive(true);
            gameoverScore.text = ScoreTracker.instance.score.ToString();
        }
    }

    public void RestartGame()
    {
        timeLeft = 10;
        gameOverPanel.SetActive(false);
        ScoreTracker.instance.score = 0;
        ScoreTracker.instance.missedClicks = 0;
        ScoreTracker.instance.targetsHit = 0;
        ScoreTracker.instance.totalClicks = 0;
        ScoreTracker.instance.UpdateScore(0);
        acc = 0;

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
