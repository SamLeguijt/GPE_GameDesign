using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ColorEnum
{
    Green, Blue, Red, Purple, Orange
}

[System.Serializable]
public struct ColorData
{
    public Color Color;
    public ColorEnum ColorType;
    public float MinSpeed;
    public float MaxSpeed;
    public float Score;
}

[CreateAssetMenu(fileName = "New ColorContainer", menuName = "ScriptableObjects/New Color Container")]
public class ColorContainer : ScriptableObject
{
    [field: SerializeField] public ColorData Green { get; private set; }
    [field: SerializeField] public ColorData Blue { get; private set; }
    [field: SerializeField] public ColorData Red { get; private set; }
    [field: SerializeField] public ColorData Purple { get; private set; }
    [field: SerializeField] public ColorData Orange { get; private set; }

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
}
