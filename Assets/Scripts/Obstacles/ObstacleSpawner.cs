using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ObstacleSpawner : MonoBehaviour
{
    public delegate void ObjectSpawnerHandler();
    public event ObjectSpawnerHandler OnObjectBudgetEmpty;

    public const int GREEN_INDEX = 0;
    public const int BLUE_INDEX = 1;
    public const int RED_INDEX = 2;
    public const int PURPLE_INDEX = 3;
    public const int ORANGE_INDEX = 4;

    [Header("References")]
    [SerializeField] private GameObject obstaclePrefab = null;

    [field: Header("Spawn Settings")]
    [field: SerializeField] public float SpawnDelay { get; private set; } = 1.5f;
    [field: SerializeField] public float ObjectsPerSpawnTick { get; private set; } = 3f;

    [field: Header("Object budgets")]
    [field: SerializeField] public int GreenBudget { get; private set; } = 1;
    [field: SerializeField] public int BlueBudget { get; private set; } = 0;
    [field: SerializeField] public int RedBudget { get; private set; } = 0;
    [field: SerializeField] public int PurpleBudget { get; private set; } = 0;
    [field: SerializeField] public int OrangeBudget { get; private set; } = 0;

    private Vector2[] spawnPositons;
    private List<ColorData> colors = new List<ColorData>();
    private Coroutine spawningRoutine = null;

    private List<int> obstaclesBudgetTable = new List<int>();
    private Queue<int> coloredObstaclesQueue = new Queue<int>();

    private void Start()
    {
        colors = GameManager.Instance.ColorContainer.GetAllColorData();

        ResetObstacleBudget(false);
        SetupSpawnPositions();
        StartSpawningObstacles();
    }

    public void SetSpawnSettings(float spawnDelay, float objectsPerSpawn)
    {
        this.SpawnDelay = spawnDelay;
        this.ObjectsPerSpawnTick = objectsPerSpawn;
    }

    public void SetObjectBudgets(int green, int blue, int red, int purple, int orange)
    {
        GreenBudget = green;
        BlueBudget = blue;
        RedBudget = red;
        PurpleBudget = purple;
        OrangeBudget = orange;
    }


    public void ResetObstacleBudget(bool shuffleList = true)
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

        if (shuffleList)
            obstaclesBudgetTable = GetShuffledList(obstaclesBudgetTable);

        for (int i = 0; i < obstaclesBudgetTable.Count; i++)
        {
            coloredObstaclesQueue.Enqueue(obstaclesBudgetTable[i]);
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
        WaitForSeconds delay = new WaitForSeconds(SpawnDelay);

        float elapsedTime = 0f;
        float maxDuration = 100000f;

        while (elapsedTime < maxDuration && GameManager.Instance.IsGameActive)
        {
            yield return delay;

            for (int i = 0; i < (int)ObjectsPerSpawnTick; i++)
            {
                Vector2 spawnPos = GetRandomSpawnPosition();

                Obstacle obstacle = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity).GetComponent<Obstacle>();

                if (coloredObstaclesQueue.TryDequeue(out int colorIndex))
                {
                    obstacle.Initialize(colors[colorIndex]);
                }
                else
                {
                    OnObjectBudgetEmpty?.Invoke();
                    Destroy(obstacle.gameObject);
                }
            }

            elapsedTime += Time.deltaTime;
        }
    }

    private List<int> GetShuffledList(List<int> toShuffle)
    {
        List<int> shuffledList = new List<int>(toShuffle);

        for (int i = 0; i < shuffledList.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffledList.Count);

            int temp = shuffledList[i];
            shuffledList[i] = shuffledList[randomIndex];
            shuffledList[randomIndex] = temp;
        }

        return shuffledList;
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
