using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public float CurrentScore { get; private set; }

    public float TotalScore { get; private set; }

    private void Start()
    {
        CurrentScore = 0;
    }

    private void OnEnable()
    {
        Obstacle.ObstacleProjectileCollisionEvent += OnObstacleProjectileCollisionEvent;
    }

    private void OnDisable()
    {
        Obstacle.ObstacleProjectileCollisionEvent -= OnObstacleProjectileCollisionEvent;
    }

    private void OnObstacleProjectileCollisionEvent(Obstacle obstacle, Projectile projectile)
    {
        CurrentScore += 100f;

        Debug.Log(CurrentScore);
    }
}
