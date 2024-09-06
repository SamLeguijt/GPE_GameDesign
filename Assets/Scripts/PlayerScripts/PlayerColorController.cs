using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorController : MonoBehaviour
{
    public delegate void PlayerColorHandler(ColorData colorData);
    public event PlayerColorHandler OnColorChanged;

    public ColorData CurrentColor { get; private set; }
    public int CurrentColorIndex { get; private set; }

    [Header("References")]
    [SerializeField] private SpriteRenderer PlayerSprite = null;
    [SerializeField] private PlayerInput inputController = null;

    private List<ColorData> allColors = new List<ColorData>();

    private void Start()
    {
        allColors = GameManager.Instance.ColorContainer.GetAllColorData();
        CurrentColorIndex = 0;

        SwitchActiveColor(CurrentColorIndex);
    }

    private void OnEnable()
    {
        inputController.SwitchColorLeftInput += OnColorSwitchLeftInputReceivedEvent;
        inputController.SwitchColorRightInput += OnColorSwitchRightInputReceivedEvent;
    }

    private void OnDisable()
    {
        inputController.SwitchColorLeftInput -= OnColorSwitchLeftInputReceivedEvent;
        inputController.SwitchColorRightInput -= OnColorSwitchRightInputReceivedEvent;
    }

    private void OnColorSwitchLeftInputReceivedEvent()
    {
        SwitchActiveColor(-1);
    }

    private void OnColorSwitchRightInputReceivedEvent()
    {
        SwitchActiveColor(1);
    }

    private void SwitchActiveColor(int direction)
    {
        if (!GameManager.Instance.IsGameActive)
            return;

        direction = Mathf.Clamp(direction, -1, 1);

        int newColorIndex = CurrentColorIndex + direction;

        // Left
        if (direction < 0)
        {
            if (newColorIndex < 0)
            {
                newColorIndex = allColors.Count - 1;
            }
        }
        // Right
        else if (direction > 0)
        {
            if (newColorIndex >= allColors.Count)
            {
                newColorIndex = 0;
            }
        }


        CurrentColor = allColors[newColorIndex];
        PlayerSprite.color = CurrentColor.Color;
        CurrentColorIndex = newColorIndex;

        OnColorChanged?.Invoke(CurrentColor);
    }
}
