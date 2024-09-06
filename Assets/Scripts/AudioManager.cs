using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Referencess")]
    [SerializeField] private AudioSource audioSource;

    [Header("Music tracks")]
    [SerializeField] private AudioClip gameRunningMusic;
    [SerializeField] private AudioClip gameOverMusic;

    [Header("SFX")]
    [SerializeField] private AudioClip shootClip; 


    private void Awake()
    {
        if (Instance != null)
            Instance = null;

        Instance = this;
    }

   

    public void PlayAudioClip(AudioClip audio, bool loop)
    {
        audioSource.loop = loop;
        audioSource.clip = audio;
        audioSource.Play();
    }

    public void PlayGameOverClip()
    {
        PlayAudioClip(gameOverMusic, false);
    }

    public void PlayGameMusic()
    {
        PlayAudioClip(gameRunningMusic, true);
    }
}
