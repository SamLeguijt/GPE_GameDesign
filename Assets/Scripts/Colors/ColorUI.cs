using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerColorController playerColor;
    [SerializeField] private Image leftPanel;
    [SerializeField] private Image middlePanel;
    [SerializeField] private Image rightPanel;

    [Header("Settings")]
    [SerializeField, Range(0, 1)] private float sidePanelsAlpha = 125f;

    private List<ColorData> colors = new List<ColorData>();

    // Start is called before the first frame update
    void Start()
    {
        colors = GameManager.Instance.ColorContainer.GetAllColorData();
    }

    private void OnEnable()
    {
        playerColor.OnColorChanged += OnPlayerColorSwitchEvent;
    }

    private void OnDisable()
    {
        playerColor.OnColorChanged -= OnPlayerColorSwitchEvent;
    }

    private void OnPlayerColorSwitchEvent(ColorData colorData)
    {
        Color newPlayerColor = playerColor.CurrentColor.Color;

        Color middlePanelColor = newPlayerColor;
        Color leftPanelColor = playerColor.GetPreviousItem(playerColor.AllColors, playerColor.CurrentColorIndex).Color;
        Color rightPanelColor = playerColor.GetNextItem(playerColor.AllColors, playerColor.CurrentColorIndex).Color;
        
        middlePanel.color = middlePanelColor;
        leftPanel.color = new Color(leftPanelColor.r, leftPanelColor.g, leftPanelColor.b, sidePanelsAlpha);
        rightPanel.color = new Color(rightPanelColor.r, rightPanelColor.g, rightPanelColor.b, sidePanelsAlpha);
    }
}

