using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public float ProjectileSpeed => projectileSpeed;

    public ColorData CurrentColor {  get; private set; }

    [Header("References")]
    [SerializeField] private Transform firePoint = null;
    [SerializeField] private Projectile projectilePRefab = null;
    [SerializeField] private PlayerInput inputController = null;
    [SerializeField] private PlayerColorController playerColor = null;

    [Header("Settings")]
    [SerializeField] private float projectileSpeed = 1f;

    private Camera cam = null;
    private void OnEnable()
    {
        inputController.ShootInput += OnShootInputReceivedEvent;
        playerColor.OnColorChanged += OnColorChangedEvent;;
    }

    private void OnDisable()
    {
        inputController.ShootInput -= OnShootInputReceivedEvent;
    }

    private void OnShootInputReceivedEvent()
    {
        Projectile bullet = Instantiate(projectilePRefab, firePoint.position, Quaternion.identity);
        bullet.Instantiate(this);
    }

    private void OnColorChangedEvent(ColorData colorData)
    {
        CurrentColor = colorData;
    }
}
