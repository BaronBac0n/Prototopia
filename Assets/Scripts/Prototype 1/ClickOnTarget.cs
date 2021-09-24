using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnTarget : MonoBehaviour
{
    public int value;

    private void OnMouseDown()
    {
        if (Timer.instance.timeLeft > 0)
        {
            ScoreTracker.instance.UpdateScore(value);

            Destroy(this.transform.parent.gameObject);


            TargetSpawner.instance.MakeNewTarget();
        }
    }    
}
