using UnityEngine;
using System.Collections;

public class SidewalkNode : MonoBehaviour, IPosition
{
    public SidewalkNodeType SidewalkNodeType
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

    public SidewalkNode(SidewalkNodeType sidewalkNodeType, Grid grid, Vector2 position)
    {
        SidewalkNodeType = sidewalkNodeType;
        Grid = grid;
        Position = position;
    }

    public void ChangeSidewalkNodeType(SidewalkNodeType sidewalkNodeType)
    {
        this.SidewalkNodeType = sidewalkNodeType;
    }

}
