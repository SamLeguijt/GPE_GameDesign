using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public delegate void PlayerHealthHandler();

    public event PlayerHealthHandler PlayerDeathEvent;
    public event PlayerHealthHandler PlayerLoseLifeEvent;

    public bool IsAlive => currentLives > 0;

    [Header("Settings")]
    [SerializeField] private int maxLives = 3;
    [SerializeField] private int currentLives = 0;
    [SerializeField] private float onDeathDestroyDelay = 3f;

    [Header("Debug tools")]
    [SerializeField] private bool takeDamage = false;

    private void Start()
    {
        currentLives = maxLives;
    }

    private void Update()
    {
        // DEBUG.
        if (takeDamage)
        {
            LoseLives(1);
            takeDamage = false;
        }
    }

    public void LoseLives(int amount)
    {
        currentLives -= amount;

        PlayerLoseLifeEvent?.Invoke();

        if (currentLives <= 0)
            KillPlayer();
    }

    private void KillPlayer()
    {
        PlayerDeathEvent?.Invoke();
        Destroy(gameObject, onDeathDestroyDelay);
    }
}
