using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealthController healthController;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;

    [Header("Player sprites")]
    [SerializeField] private Sprite baseSprite = null;
    [SerializeField] private Sprite slightDamagedSprite = null;
    [SerializeField] private Sprite heavyDamagedSprite=  null;

    private List<Sprite> playerSprites = new List<Sprite>(); 
    private int playerSpriteIndex = 0;

    private void Awake()
    {
        playerSprites.Add(baseSprite);    
        playerSprites.Add(slightDamagedSprite);    
        playerSprites.Add(heavyDamagedSprite);   

        playerSpriteIndex = 0;

        if (playerSpriteIndex < playerSprites.Count)
            playerSpriteRenderer.sprite = playerSprites[playerSpriteIndex];
    }

    private void OnEnable()
    {
        healthController.PlayerLoseLifeEvent += OnPlayerLoseLifeEvent;
    }

    private void OnDisable()
    {
        healthController.PlayerLoseLifeEvent -= OnPlayerLoseLifeEvent;
    }

    private void OnPlayerLoseLifeEvent()
    {
        playerSpriteIndex++;

        if (playerSpriteIndex < playerSprites.Count)
            playerSpriteRenderer.sprite = playerSprites[playerSpriteIndex];
    }
}
