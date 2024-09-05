using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject obstaclePrefab = null;

    [Header("Settings")]
    [SerializeField] private float spawnDelay = 1.5f;
    [SerializeField] private int objectsPerSpawnTick = 3;
    [SerializeField] private float minObstacleSpeed = 1f;
    [SerializeField] private float maxObstacleSpeed = 5f;

    private Vector2[] spawnPositons;
    private Coroutine spawningRoutine = null;

    private void Start()
    {
        SetupSpawnPositions();
        StartSpawningObstacles();
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

                Obstacle obstacle = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity).GetComponent<Obstacle>() ;
                obstacle.Initialize(GetRandomFloat(minObstacleSpeed, maxObstacleSpeed));
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
}
