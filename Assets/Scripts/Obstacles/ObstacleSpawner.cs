using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public const int GREEN_INDEX = 0;
    public const int BLUE_INDEX = 1;
    public const int RED_INDEX = 2;
    public const int PURPLE_INDEX = 3;
    public const int ORANGE_INDEX = 4;

    [Header("References")]
    [SerializeField] private GameObject obstaclePrefab = null;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnDelay = 1.5f;
    [SerializeField] private int objectsPerSpawnTick = 3;

    [field: Header("Object budgets")]
    [field: SerializeField] public int GreenBudget {get; private set;} = 1;
    [field: SerializeField] public int BlueBudget { get; private set; } = 0;
    [field: SerializeField] public int RedBudget { get; private set; } = 0;
    [field: SerializeField] public int PurpleBudget { get; private set; } = 0;
    [field: SerializeField] public int OrangeBudget { get; private set; } = 0;

    private Vector2[] spawnPositons;
    private List<ColorData> colors = new List<ColorData>();
    private Coroutine spawningRoutine = null;

    private List<int> obstaclesBudgetTable = new List<int>();

    private void Start()
    {
        colors = GameManager.Instance.ColorContainer.GetAllColorData();

        ResetObstacleBudget();
        SetupSpawnPositions();
        StartSpawningObstacles();
    }

    public void SetSpawnSettings(float spawnDelay, int objectsPerSpawn)
    {
        this.spawnDelay = spawnDelay; 
        this.objectsPerSpawnTick = objectsPerSpawn;
    }

    public void SetObjectBudgets(int green, int blue, int red, int purple, int orange)
    {
        GreenBudget = green;
        BlueBudget = blue; 
        RedBudget = red;
        PurpleBudget = purple;
        OrangeBudget = orange;
    }

    private void ResetObstacleBudget()
    {
        obstaclesBudgetTable.Clear();

        if (GreenBudget > 0)
        {
            for (int i = 0; i < GreenBudget; i++)
                obstaclesBudgetTable.Add(GREEN_INDEX);
        }

        if (BlueBudget > 0)
        {
            for (int i = 0; i < BlueBudget; i++)
                obstaclesBudgetTable.Add(BLUE_INDEX);
        }

        if (RedBudget > 0)
        {
            for (int i = 0; i < RedBudget; i++)
                obstaclesBudgetTable.Add(RED_INDEX);
        }


        if (PurpleBudget > 0)
        {
            for (int i = 0; i < PurpleBudget; i++)
                obstaclesBudgetTable.Add(PURPLE_INDEX);
        }

        if (OrangeBudget > 0)
        {
            for (int i = 0; i < OrangeBudget; i++)
                obstaclesBudgetTable.Add(ORANGE_INDEX);
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.GameStartedEvent += StartSpawningObstacles;
        GameManager.Instance.GameEndedEvent += StopSpawningObstacles;
    }

    private void OnDisable()
    {
        GameManager.Instance.GameStartedEvent -= StartSpawningObstacles;
        GameManager.Instance.GameEndedEvent -= StopSpawningObstacles;
    }

    private void SetupSpawnPositions()
    {
        spawnPositons = new Vector2[3];

        spawnPositons[0] = GameManager.Instance.LaneManager.LeftLane.Position;
        spawnPositons[1] = GameManager.Instance.LaneManager.MiddleLane.Position;
        spawnPositons[2] = GameManager.Instance.LaneManager.RightLane.Position;
    }

    private void StartSpawningObstacles()
    {
        if (spawningRoutine != null)
            StopCoroutine(spawningRoutine);

        spawningRoutine = StartCoroutine(SpawnObjectsCoroutine());
    }

    private void StopSpawningObstacles()
    {
        if (spawningRoutine != null)
        {
            StopCoroutine(spawningRoutine);
            spawningRoutine = null;
        }
    }

    private IEnumerator SpawnObjectsCoroutine()
    {
        WaitForSeconds delay = new WaitForSeconds(spawnDelay);

        float elapsedTime = 0f;
        float maxDuration = 10f;

        while (elapsedTime < maxDuration && GameManager.Instance.IsGameActive)
        {
            yield return delay;

            for (int i = 0; i < objectsPerSpawnTick; i++)
            {
                Vector2 spawnPos = GetRandomSpawnPosition();

                Obstacle obstacle = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity).GetComponent<Obstacle>();

                int randomColorIndex = GetRandomInt(0, obstaclesBudgetTable.Count);
                int colorIndex = obstaclesBudgetTable[randomColorIndex];
                obstacle.Initialize(colors[colorIndex]);
            }

            elapsedTime += Time.deltaTime;
        }
    }

    private Vector2 GetRandomSpawnPosition()
    {
        int randomIndex = Random.Range(0, spawnPositons.Length);

        return spawnPositons[randomIndex];
    }

    private float GetRandomFloat(float min, float max)
    {
        float actualMax = max + 1;

        return Random.Range(min, actualMax);
    }

    private int GetRandomInt(int min, int max)
    {
        return Random.Range(min, max);
    }
}
