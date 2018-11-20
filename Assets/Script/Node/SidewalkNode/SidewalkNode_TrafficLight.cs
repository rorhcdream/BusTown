using UnityEngine;
using System.Collections;

public class SidewalkNode_TrafficLight : SidewalkNode
{
    private TrafficLightTimer trafficLightTimer;

    public SidewalkNode_TrafficLight(Grid grid, Vector2 position, TrafficLightTimer trafficLightTimer) : base(grid, position)
    {
        this.trafficLightTimer = trafficLightTimer;
    }

    public override SidewalkNodeType GetNodeType()
    {
        return SidewalkNodeType.TrafficLight;
    }

    
}
