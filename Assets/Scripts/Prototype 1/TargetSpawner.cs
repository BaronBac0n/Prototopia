using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    #region Singleton
    public static TargetSpawner instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of TargetSpawner found");
            return;
        }
        instance = this;
    }
    #endregion

    public int maxTargets;

    public GameObject targetPrefab;

    void Start()
    {
        
    }

    public void MakeNewTarget()
    {
        float spawnY = Random.Range
              (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
        float spawnX = Random.Range
            (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

        Vector2 spawnPosition = new Vector2(spawnX, spawnY);

        //stop overlap of targets
        if (Physics2D.OverlapCircle(spawnPosition, 1))
        {
            MakeNewTarget();
        }
        else
        {
            Instantiate(targetPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
