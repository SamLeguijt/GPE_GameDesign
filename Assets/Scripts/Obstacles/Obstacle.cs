using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public delegate void ObstcaleCollisionHandler(Obstacle obstacle, Projectile projectile);
    public delegate void ObstacleEscapedHandler(Obstacle obstacle);

    public static event ObstcaleCollisionHandler ObstacleProjectileCollisionEvent;
    public static event ObstacleEscapedHandler ObstacleEscapedEvent;

    private Collider2D obstacleCollider = null;

    [field: Header("Settings")]
    [field: SerializeField] public Color ObstacleColor { get; private set; }

    private int projectileLayer;
    private Vector2 screenBounds;

    private void Start()
    {
        obstacleCollider = GetComponent<Collider2D>();
        obstacleCollider.isTrigger = true;

        screenBounds = GameManager.Instance.GetScreenBounds();
        projectileLayer = GameManager.Instance.ProjectileLayerIndex;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == projectileLayer)
        {
            if (collision.TryGetComponent(out Projectile projectile))
            {
                ObstacleProjectileCollisionEvent?.Invoke(this, projectile);
                projectile.OnObstacleCollision(this);
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        if (transform.position.y < -screenBounds.y)
        {
            ObstacleEscapedEvent?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
