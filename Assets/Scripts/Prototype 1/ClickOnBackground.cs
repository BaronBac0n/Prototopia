using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnBackground : MonoBehaviour
{
    public void OnMouseDown()
    {
        if (StartGame.instance.startPanel.activeInHierarchy == false)
        {
            if (Timer.instance.timeLeft > 0)
            {
                ScoreTracker.instance.totalClicks++;
                //coreTracker.instance.missedClicks++;
                SoundManager.instance.PlayAudio(0);
            }
        }
    }
}
