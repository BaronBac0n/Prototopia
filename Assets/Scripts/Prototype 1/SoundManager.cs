using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singleton
    public static SoundManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of SoundManager found");
            return;
        }
        instance = this;
    }
    #endregion

    public AudioSource audioSource;
    public AudioClip[] audioClips;
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    public void PlayAudio(int numberToPlay)
    {
        audioSource.clip = audioClips[numberToPlay];
        audioSource.Play();
    }
}
