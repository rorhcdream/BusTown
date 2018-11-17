﻿using UnityEngine;
using System.Collections;

public class DrivewayNode : MonoBehaviour, IPosition
{
    public DrivewayNodeType DrivewayNodeType
    {
        get;
        private set;
    }
    public Grid Grid
    {
        get;
        private set;
    }
    public Vector2 Position { get; private set; }

    //Type이 TrafficLight일때만 사용한다.
    public TrafficLight TrafficLight { get; private set; }

    public DrivewayNode(DrivewayNodeType drivewayNodeType, Grid grid, Vector2 position)
    {
        DrivewayNodeType = DrivewayNodeType;
        Grid = grid;
        Position = position;
    }

    public void ChangeDrivewayNodeType(DrivewayNodeType drivewayNodeType)
    {
        this.DrivewayNodeType = drivewayNodeType;
    }

}
