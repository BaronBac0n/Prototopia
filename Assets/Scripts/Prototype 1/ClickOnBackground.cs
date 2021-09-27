using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnBackground : MonoBehaviour
{
    public void OnMouseDown()
    {
        if (Timer.instance.timeLeft > 0)
        {
            ScoreTracker.instance.totalClicks++;
            ScoreTracker.instance.missedClicks++;
            SoundManager.instance.PlayAudio(0);
        }
    }
}
