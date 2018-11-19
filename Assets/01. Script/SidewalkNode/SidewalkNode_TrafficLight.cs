using UnityEngine;
using System.Collections;

public class SidewalkNode_TrafficLight : SidewalkNode
{
    public SidewalkNode_TrafficLight(Grid grid, Vector2 position) : base(grid, position)
    {}

    public TrafficLightTimer TrafficLight { get; private set; }

    public override SidewalkNodeType GetNodeType()
    {
        return SidewalkNodeType.TrafficLight;
    }
}
