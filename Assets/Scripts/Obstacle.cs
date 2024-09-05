using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Collider2D obstacleCollider = null;

    [field: Header("Settings")]
    [field: SerializeField] public Color ObstacleColor { get; private set; }

    private int projectileLayer;
    private void Start()
    {
        obstacleCollider = GetComponent<Collider2D>();
        obstacleCollider.isTrigger = true;

        projectileLayer = GameManager.Instance.ProjectileLayerIndex;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == projectileLayer)
        {
            Projectile projectile = collision.gameObject.GetComponent<Projectile>();
            projectile.OnObstacleCollision(this);
            Destroy(gameObject);
        }
    }
}
