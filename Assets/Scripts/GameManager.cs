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
    [field: SerializeField] public Camera MainCamera { get; private set; }
    [field: SerializeField] public GameObject GameOverUI { get; private set; }
    [field: SerializeField] public ColorContainer ColorContainer { get; private set; }  


    [field: Header("Settings")]
    [field: SerializeField] public int ProjectileLayerIndex { get; private set; } = 6;
    [field: SerializeField] public float LoadGameOverSceneDelay { get; private set; } = 3f;
    [field: SerializeField] public float DecreasedSimulationSpeed { get; private set; } = .75f;
    [field: SerializeField] public float DecreasedSpeedDuration { get; private set; } = 2.5f;
    [SerializeField] public float SimulationSpeed { get; private set; } = 1f;

    public bool IsGameActive { get; private set; } = true; 

    private Coroutine decreasedSpeedRoutine = null;

    private void OnEnable()
    {
        if (PlayerHealth != null)
        {
            PlayerHealth.PlayerLoseLifeEvent += OnPlayerLoseLifeEvent;
            PlayerHealth.PlayerGameOverEvent += OnPlayerDeathEvent;
        }
    }

    private void OnDestroy()
    {
        if (PlayerHealth != null)
        {
            PlayerHealth.PlayerLoseLifeEvent -= OnPlayerLoseLifeEvent;
            PlayerHealth.PlayerGameOverEvent -= OnPlayerDeathEvent;
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        if (MainCamera == null)
            MainCamera = Camera.main;

        GameOverUI.SetActive(false);
    }
    private void OnPlayerDeathEvent()
    {
        //Invoke("LoadGameOverScene", LoadGameOverSceneDelay);

        GameOverUI.SetActive(true);
        SimulationSpeed = 0f;
        IsGameActive = false;
    }

    private void OnPlayerLoseLifeEvent()
    {
        //if (decreasedSpeedRoutine == null)
        //StartCoroutine(DecreaseGameSpeedCoroutine());
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

    public Vector2 GetScreenBounds()
    {
        Vector2 screenBottomLeft = MainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 screenTopRight = MainCamera.ViewportToWorldPoint(new Vector2(1, 1));

        

        return new Vector2(screenTopRight.x, screenTopRight.y);
    }
    #region SceneManagement
    private void LoadGameOverScene()
    {
        //SceneManager.LoadScene(GAME_OVER_SCENE);
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
