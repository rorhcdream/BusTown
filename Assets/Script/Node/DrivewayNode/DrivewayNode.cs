using UnityEngine;
using System.Collections;

public class DrivewayNode : Node
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

    //Type이 TrafficLight일때만 사용한다.
    public TrafficLightTimer TrafficLight { get; private set; }

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


    public virtual bool IsPassable()
    {
        return true;
    }
}
