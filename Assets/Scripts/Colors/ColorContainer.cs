using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ColorEnum
{
    Green, Blue, Red, Purple, Orange
}

public struct ColorData
{
    public Color Color;
    public ColorEnum ColorType;
}

[CreateAssetMenu(fileName = "New ColorContainer", menuName = "ScriptableObjects/New Color Container")]
public class ColorContainer : ScriptableObject
{
    public ColorData Green { get; private set; }
    public ColorData Blue { get; private set; }
    public ColorData Red { get; private set; }
    public ColorData Purple { get; private set; }
    public ColorData Orange { get; private set; }

    [SerializeField] private Color green;
    [SerializeField] private Color blue;
    [SerializeField] private Color red;
    [SerializeField] private Color purple;
    [SerializeField] private Color orange;


    private void OnValidate()
    {
        Green = new ColorData{ Color = green, ColorType = ColorEnum.Green };
        Blue = new ColorData{ Color = blue, ColorType = ColorEnum.Blue };
        Red = new ColorData{ Color = red, ColorType = ColorEnum.Red };
        Purple = new ColorData{ Color = purple, ColorType = ColorEnum.Purple };
        Orange = new ColorData{ Color= orange, ColorType = ColorEnum.Orange };
    }

    public List<ColorData> GetAllColorData()
    {
        List<ColorData> colorDatas = new List<ColorData>
        {
            Green,
            Blue, 
            Red, 
            Purple,
            Orange,
        };

        return colorDatas;
    }

    public List<Color> GetAllColors()
    {
        List<Color> colors = new List<Color>
        {
            green,
            blue,
            red,
            purple,
            orange
        };

        return colors;
    }
}
