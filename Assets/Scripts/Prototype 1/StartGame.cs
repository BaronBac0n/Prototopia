using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    #region Singleton
    public static StartGame instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of StartGame found");
            return;
        }
        instance = this;
    }
    #endregion

    public GameObject startPanel;

    void Start()
    {
        startPanel.SetActive(true);
    }
    
    void Update()
    {
        
    }

    public void BeginGame()
    {
        startPanel.SetActive(false);
        for (int i = 0; i < TargetSpawner.instance.maxTargets; i++)
        {
            TargetSpawner.instance.MakeNewTarget();
        }
    }
}
