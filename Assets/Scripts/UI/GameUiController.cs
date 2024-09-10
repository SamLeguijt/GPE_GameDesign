using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

public class GameUiController : MonoBehaviour
{
    [Header("Component references")]
    [SerializeField] private PlayerHealthController playerHealth = null;

    [Header("UI References")]
    [SerializeField] private GameObject backgroundOverlay = null;
    [SerializeField] private GameObject screenFlashUI = null;
    [SerializeField, Space] private GameObject gameOverUI = null;
    [SerializeField] private GameObject replayButton = null;
    [SerializeField, Space] private GameObject startGameUI = null;
    [SerializeField] private GameObject startButton = null;
    [SerializeField] private GameObject controlsUI = null;
    [SerializeField] private GameObject instructionsUI = null;
    [SerializeField, Space] private GameObject playerScoreUI = null;
    [SerializeField] private GameObject playerHealthUI = null;
    [SerializeField] private GameObject playerColorUI = null;
    private Image screenFlashImg = null;

    

    private Coroutine screenFlashRoutine = null;
    private void Awake()
    {
        backgroundOverlay.SetActive(true);

        gameOverUI.SetActive(false);

        startGameUI.SetActive(true);
        controlsUI.SetActive(true);
        instructionsUI.SetActive(true);
        playerScoreUI.SetActive(true);
        playerHealthUI.SetActive(true);
        playerColorUI.SetActive(true);

    }


    

    private void Start()
    {
        GameManager.Instance.GameStartedEvent += OnGameStartEvent;
        GameManager.Instance.GameEndedEvent += OnGameEndEvent;

        screenFlashImg = screenFlashUI.GetComponent<Image>();



        EventSystem.current.SetSelectedGameObject(startButton);
    }


    private void Update()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject);

    }
    private void OnEnable()
    {
        playerHealth.PlayerLoseLifeEvent += OnPlayerLoseLifeEvent;
    }

    private void OnDisable()
    {
        playerHealth.PlayerLoseLifeEvent -= OnPlayerLoseLifeEvent;

        GameManager.Instance.GameStartedEvent += OnGameStartEvent;
        GameManager.Instance.GameEndedEvent += OnGameEndEvent;
    }

    private void OnGameStartEvent()
    {
        backgroundOverlay.SetActive(false);

        startGameUI.SetActive(false);
        controlsUI.SetActive(false);
        instructionsUI.SetActive(false);
    }

    private void OnGameEndEvent()
    {
        backgroundOverlay.SetActive(true);

        EventSystem.current.SetSelectedGameObject(replayButton);


        playerColorUI.transform.SetParent(backgroundOverlay.transform);
        gameOverUI.SetActive(true);
    }

    private void OnPlayerLoseLifeEvent()
    {
        PerformScreenFlash();
    }

    private void PerformScreenFlash()
    {
        if (screenFlashRoutine != null)
            StopCoroutine(screenFlashRoutine);

        screenFlashRoutine = StartCoroutine(ScreenFlashCoroutine());
    }

    private IEnumerator ScreenFlashCoroutine()
    {
        if (screenFlashImg == null)
            yield break;

        float elapsed = 0f;
        float inDuration = GameManager.Instance.ScreenFlashFadeInDuration;
        float outDuration = GameManager.Instance.DecreasedSpeedDuration - GameManager.Instance.ScreenFlashFadeInDuration;
        float intensity = GameManager.Instance.ScreenFlashAlphaIntensity / 255;

        while (elapsed < inDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Lerp(0, intensity, elapsed / inDuration);
            screenFlashImg.color = new Color(screenFlashImg.color.r, screenFlashImg.color.g, screenFlashImg.color.b, t);

            if (Mathf.Abs(screenFlashImg.color.a - intensity) <= 0.01f)
                break;

            yield return null;
        }

        elapsed = 0f;

        while (elapsed < outDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Lerp(intensity, 0, elapsed / outDuration);
            screenFlashImg.color = new Color(screenFlashImg.color.r, screenFlashImg.color.g, screenFlashImg.color.b, t);

            if (screenFlashImg.color.a <= 0)
                break;

            yield return null;

        }
    }
}
