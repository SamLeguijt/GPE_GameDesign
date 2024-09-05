using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LaneEnum
{
    Left,
    Middle,
    Right
}

public struct Lane
{
    public Vector2 Position { get; set; }
    public LaneEnum LaneType { get; set; }
}

public class LaneManager : MonoBehaviour
{
    public Lane LeftLane => leftLane;
    public Lane MiddleLane => middleLane;
    public Lane RightLane => rightLane;

    [Header("Lane settings")]
    [Tooltip("X coordinate of Vector is used for the players' X pos for that line, the Y component is used as the players' Z pos.")]
    [SerializeField] private Vector2[] lanePositions = new Vector2[3];

    private Lane leftLane;
    private Lane middleLane;
    private Lane rightLane;

    private void Awake()
    {
        SetupLanes();
    }

    private void SetupLanes()
    {
        leftLane.Position = lanePositions[0];
        leftLane.LaneType = LaneEnum.Left;

        middleLane.Position = lanePositions[1];
        middleLane.LaneType = LaneEnum.Middle;

        rightLane.Position = lanePositions[2];
        rightLane.LaneType = LaneEnum.Right;
    }
}
