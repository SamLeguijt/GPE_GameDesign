using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public float ProjectileSpeed => projectileSpeed;
    public Vector2 ScreenBounds { get; private set; }

    [Header("References")]
    [SerializeField] private Transform firePoint = null;
    [SerializeField] private Projectile projectilePRefab = null;
    [SerializeField] private PlayerInput inputController = null;

    [Header("Settings")]
    [SerializeField] private float projectileSpeed = 1f;

    private Camera cam = null;
    private void OnEnable()
    {
        inputController.ShootInput += OnShootInputReceivedEvent;
    }

    private void OnDisable()
    {
        inputController.ShootInput -= OnShootInputReceivedEvent;
    }

    private void Start()
    {
        cam = Camera.main;
        ScreenBounds = GetScreenBounds();
    }

    private void OnShootInputReceivedEvent()
    {
        Projectile bullet = Instantiate(projectilePRefab, firePoint.position, Quaternion.identity);
        bullet.Instantiate(this);
    }

    private Vector2 GetScreenBounds()
    {
        Vector2 screenBottomLeft = cam.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 screenTopRight = cam.ViewportToWorldPoint(new Vector2(1, 1));

        return new Vector2(screenTopRight.x, screenTopRight.y);
    }
}
