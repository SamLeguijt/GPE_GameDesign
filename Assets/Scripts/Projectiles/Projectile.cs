using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool HasCollided {  get; private set; }

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer = null;

    private float speed = 0f;
    private bool isEnabled = false;
    private Vector2 screenBounds;

    public void Instantiate(PlayerWeapon owner)
    {
        screenBounds = GameManager.Instance.GetScreenBounds();

        this.speed = owner.ProjectileSpeed;
        spriteRenderer.color = owner.CurrentColor.Color;

        isEnabled = true;
    }

    private void Update()
    {
        if (!isEnabled)
            return;

        transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);


        if (transform.position.y > screenBounds.y)
        {
            Destroy(gameObject);
        }
    }

    public void OnObstacleCollision(Obstacle obstacle)
    {
        HasCollided = true;
        Destroy(gameObject);
    }
}
