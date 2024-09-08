using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUiController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject backgroundOverlay = null;
    [SerializeField, Space] private GameObject gameOverUI = null;
    [SerializeField, Space] private GameObject startGameUI = null;
    [SerializeField] private GameObject controlsUI = null;
    [SerializeField] private GameObject instructionsUI = null;
    [SerializeField, Space] private GameObject playerScoreUI = null;
    [SerializeField] private GameObject playerHealthUI = null;
    [SerializeField] private GameObject playerColorUI = null;


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
    }

    private void OnDisable()
    {
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

        playerColorUI.transform.parent = backgroundOverlay.transform;
        gameOverUI.SetActive(true);
    }
}
