using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealthController playerHealth;
    [SerializeField] private Image healthPanelUI = null;
    [SerializeField] private Transform leftPanelSide = null;
    [SerializeField] private Image heartSprite = null;

    private List<Image> heartImages = new List<Image>();

    private Color disabledHeartColor = Color.black;

    private int lostLives = 0;
    private void OnEnable()
    {
        playerHealth.PlayerLoseLifeEvent += OnPlayerLoseLife;
    }

    private void OnDisable()
    {
        playerHealth.PlayerLoseLifeEvent -= OnPlayerLoseLife;

    }

    private void Start()
    {
        float space = 32f;

        for (int i = 0; i < playerHealth.MaxLives; i++)
        {
            Vector2 position = new Vector2(leftPanelSide.transform.position.x + (space * i), healthPanelUI.transform.position.y);
            Image heart = Instantiate(heartSprite, position, Quaternion.identity, healthPanelUI.transform);


            heartImages.Add(heart);
        }

        for (int i = 0; i < heartImages.Count; i++)
        {
            heartImages[i].gameObject.SetActive(true);
        }

        heartImages.Reverse();
    }

    private void OnPlayerLoseLife()
    {
        if (lostLives >= heartImages.Count)
            return;

        heartImages[lostLives].color = disabledHeartColor;
        lostLives++;
    }
}
