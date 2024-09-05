using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private const string MAIN_SCENE = "Main";
    private const string GAME_OVER_SCENE = "GameOver";

    public delegate void GameSpeedHandler(float newSpeed);
    public event GameSpeedHandler GameSpeedChanged;

    [field: Header("References")]
    [field: SerializeField] public PlayerHealthController PlayerHealth { get; private set; }
    [field: SerializeField] public LaneManager LaneManager { get; private set; }


    [field: Header("Settings")]
    [field: SerializeField] public int PlayerLayerIndex { get; private set; } = 7;
    [field: SerializeField] public float LoadGameOverSceneDelay { get; private set; } = 3f;
    [field: SerializeField] public float DecreasedSimulationSpeed { get; private set; } = .75f;
    [field: SerializeField] public float DecreasedSpeedDuration { get; private set; } = 2.5f;
    [SerializeField] public float SimulationSpeed { get; private set; } = 1f;

    private Coroutine decreasedSpeedRoutine = null;

    private void OnEnable()
    {
        if (PlayerHealth != null)
        {
            PlayerHealth.PlayerLoseLifeEvent += OnPlayerLoseLifeEvent;
            PlayerHealth.PlayerDeathEvent += OnPlayerDeathEvent;
        }
    }

    private void OnDestroy()
    {
        if (PlayerHealth != null)
        {
            PlayerHealth.PlayerLoseLifeEvent -= OnPlayerLoseLifeEvent;
            PlayerHealth.PlayerDeathEvent -= OnPlayerDeathEvent;
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnPlayerDeathEvent()
    {
        Invoke("LoadGameOverScene", LoadGameOverSceneDelay);
    }

    private void OnPlayerLoseLifeEvent()
    {
        if (decreasedSpeedRoutine == null)
            StartCoroutine(DecreaseGameSpeedCoroutine());
    }

    private IEnumerator DecreaseGameSpeedCoroutine()
    {
        float normalSpeed = 1;

        SimulationSpeed = DecreasedSimulationSpeed;
        GameSpeedChanged?.Invoke(SimulationSpeed);

        yield return new WaitForSeconds(DecreasedSpeedDuration);

        SimulationSpeed = normalSpeed;
        GameSpeedChanged?.Invoke(SimulationSpeed);

        decreasedSpeedRoutine = null;
    }

    #region SceneManagement
    private void LoadGameOverScene()
    {
        SceneManager.LoadScene(GAME_OVER_SCENE);
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(MAIN_SCENE);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion
}
