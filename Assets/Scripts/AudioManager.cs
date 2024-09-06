using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Referencess")]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    [Header("Music tracks")]
    [SerializeField] private AudioClip gameRunningMusic;
    [SerializeField] private AudioClip gameOverMusic;

    [Header("SFX")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip scoreIncreaseClip;
    [SerializeField] private AudioClip playerMoveClip;
    [SerializeField] private AudioClip switchColorClip;
    [SerializeField] private AudioClip gameOverBeep;
    [SerializeField] private AudioClip objectEscapedClip;

    [Header("Settings")]
    [SerializeField] private float randomPitchRangeMin;
    [SerializeField] private float randomPitchRangeMax;


    private void Awake()
    {
        if (Instance != null)
            Instance = null;

        Instance = this;
    }



    public void PlayMusic(AudioClip audio, bool loop)
    {
        if (!GameManager.Instance.EnableMusic)
            return;

        musicAudioSource.loop = loop;
        musicAudioSource.clip = audio;
        musicAudioSource.Play();
    }

    public void PlaySFX(AudioClip clip, bool loop, float pitch)
    {
        if (!GameManager.Instance.EnableSFX)
            return;

        sfxAudioSource.clip = clip;
        sfxAudioSource.pitch = pitch;
        sfxAudioSource.loop = loop;
        sfxAudioSource.Play();
    }

    public void PlayGameOverClip()
    {
        PlayMusic(gameOverMusic, false);
        StartCoroutine(GameOverBeepRoutine());
    }


    public void PlayGameMusic()
    {
        PlayMusic(gameRunningMusic, true);
    }

    public void PlayScoreIncreaseSFX()
    {
        float pitch = 1 + Random.Range(randomPitchRangeMin, randomPitchRangeMax);

        PlaySFX(scoreIncreaseClip, false, pitch);
    }

    public void PlayMovementSFX()
    {
        float pitch = 1 + Random.Range(randomPitchRangeMin, randomPitchRangeMax);

        PlaySFX(playerMoveClip, false, 1);
    }

    public void PlayColorSwitchSFX()
    {
        float pitch = 1 + Random.Range(randomPitchRangeMin, randomPitchRangeMax);

        PlaySFX(switchColorClip, false, pitch);
    }

    public void PlayShootSFX()
    {
        float pitch = 1 + Random.Range(-0.5f, 0.5f);

        PlaySFX(shootClip, false, pitch);
    }

    private void PlayGameOverSFX()
    {
        PlaySFX(gameOverBeep, false, 1);
    }

    public void PlayObjectEscapedSFX()
    {
        float pitch = 1 + Random.Range(randomPitchRangeMin, randomPitchRangeMax);

        PlaySFX(objectEscapedClip, false, pitch);
    }

    private IEnumerator GameOverBeepRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(2);

        for (int i = 0; i < 100; i++)
        {
            yield return delay;
            PlayGameOverSFX();
        }
    }
}
