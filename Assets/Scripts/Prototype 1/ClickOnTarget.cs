using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnTarget : MonoBehaviour
{
    public int value;
    public int audioToPlay;

    private void Start()
    {
    }

    private void OnMouseDown()
    {
        if (Timer.instance.timeLeft > 0)
        {
            ScoreTracker.instance.targetsHit++;
            SoundManager.instance.PlayAudio(audioToPlay);        
            ScoreTracker.instance.UpdateScore(value);               
            TargetSpawner.instance.MakeNewTarget();
            Destroy(this.transform.parent.gameObject);
        }
    }    
}
