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

    private bool canBeDestroyed = false;
    private bool isActive = false;
    private float fallSpeed = 1f;
    private int projectileLayer;
    private Vector2 screenBounds;

    private SpriteRenderer spriteRenderer;
    private float spriteSizeHalfedY;
    private void Start()
    {
        obstacleCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteSizeHalfedY = spriteRenderer.sprite.bounds.size.y /2;

        obstacleCollider.isTrigger = true;

        screenBounds = GameManager.Instance.GetScreenBounds();
        projectileLayer = GameManager.Instance.ProjectileLayerIndex;

        isActive = true;
    }

    public void Initialize(float fallSpeed)
    {
        this.fallSpeed = fallSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canBeDestroyed)
            return;

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
        if (!isActive)
            return;

        if (!canBeDestroyed)
        {
            if (transform.position.y < (screenBounds.y - spriteSizeHalfedY))
            {
                canBeDestroyed = true;
            }
        }

        transform.position = new Vector2(transform.position.x, transform.position.y - fallSpeed * Time.deltaTime);



        if (transform.position.y < (-screenBounds.y - spriteSizeHalfedY))
        {
            ObstacleEscapedEvent?.Invoke(this);
            isActive = false;
            Destroy(gameObject);
        }
    }
}
