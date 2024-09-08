using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public float ProjectileSpeed => projectileSpeed;

    public ColorData CurrentColor {  get; private set; }
    public bool AllowedToShoot { get; private set; } = false;

    [Header("References")]
    [SerializeField] private Transform firePoint = null;
    [SerializeField] private Projectile projectilePRefab = null;
    [SerializeField] private PlayerInput inputController = null;
    [SerializeField] private PlayerColorController playerColor = null;

    [Header("Settings")]
    [SerializeField] private float projectileSpeed = 1f;

    private void Start()
    {
        GameManager.Instance.GameStartedEvent += OnGameStartEvent;
        GameManager.Instance.GameEndedEvent += OnGameEndEvent;
    }

    private void OnEnable()
    {
        inputController.ShootInput += OnShootInputReceivedEvent;
        playerColor.OnColorChanged += OnColorChangedEvent;
    }

    private void OnDisable()
    {
        inputController.ShootInput -= OnShootInputReceivedEvent;
        GameManager.Instance.GameStartedEvent -= OnGameStartEvent;
        GameManager.Instance.GameEndedEvent -= OnGameEndEvent;
    }

    private void OnShootInputReceivedEvent()
    {
        ShootBullet();
    }

    private void ShootBullet()
    {
        if (!AllowedToShoot)
            return;

        AudioManager.Instance.PlayShootSFX();
        Projectile bullet = Instantiate(projectilePRefab, firePoint.position, Quaternion.identity);
        bullet.Instantiate(this);
    }

    private void OnColorChangedEvent(ColorData colorData)
    {
        CurrentColor = colorData;
    }

    private void OnGameStartEvent()
    {
        AllowedToShoot = true;
    }

    private void OnGameEndEvent()
    {
        AllowedToShoot = false;
    }
}
