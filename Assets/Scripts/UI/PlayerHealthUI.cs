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

    private void Start()
    {
        float space = 32f;

        for (int i = 0; i < playerHealth.MaxLives; i++)
        {
            Vector2 position = new Vector2(leftPanelSide.transform.position.x + (space * i), healthPanelUI.transform.position.y);
            Image heart = Instantiate(heartSprite, position, Quaternion.identity, healthPanelUI.transform);


            heartImages.Add(heart);
        }

        for (int i = 0; i< heartImages.Count; i++)
        {
            heartImages[i].gameObject.SetActive(true);
        }
    }
}
