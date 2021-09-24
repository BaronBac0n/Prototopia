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

    void Start()
    {
        
    }
    
    void Update()
    {
        timeLeft -= Time.deltaTime;
        timerT.text = Mathf.Round(timeLeft).ToString();
        if (timeLeft < 0)
        {
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
    }
}
