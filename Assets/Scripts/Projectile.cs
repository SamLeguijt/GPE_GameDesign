using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed = 0f;

    private bool isEnabled = false;

    private Vector2 screenBounds;

    public void Instantiate(PlayerWeapon owner)
    {
        screenBounds = owner.ScreenBounds;

        this.speed = owner.ProjectileSpeed;
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
        Destroy(gameObject);
    }
}
