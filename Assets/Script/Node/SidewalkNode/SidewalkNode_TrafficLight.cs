using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SidewalkNode_TrafficLight : SidewalkNode
{
    private TrafficLightTimer trafficLightTimer;
    public bool isBlueLight
    { get { return trafficLightTimer.SidewalkOn; }}
    public event Action OnBlueLightSet;
    
    public SidewalkNode_TrafficLight(Grid grid, Vector2 position, TrafficLightTimer trafficLightTimer) : base(grid, position)
    {
        this.trafficLightTimer = trafficLightTimer;
        SetTrafficLightTimerEvent();
    }
    public SidewalkNode_TrafficLight(Grid grid, Vector2 position) : base(grid, position)
    {
        this.trafficLightTimer = gameObject.AddComponent<TrafficLightTimer>();
        SetTrafficLightTimerEvent();
    }

    public override SidewalkNodeType GetNodeType()
    {
        return SidewalkNodeType.TrafficLight;
    }
    
    //set OnBlueLightSet() event to be evoked
    private void SetTrafficLightTimerEvent()
    {
        if(trafficLightTimer != null)
        {
            trafficLightTimer.OnLightChanged += 
                (bool b) => { if (b && OnBlueLightSet != null) OnBlueLightSet(); };
        }
    }
}
