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
    public readonly int id;     //짝이 맞는 신호등끼리는 같은 id를 가짐.
    
    public SidewalkNode_TrafficLight(Grid grid, Vector2 position, TrafficLightTimer trafficLightTimer, int id) : base(grid, position)
    {
        this.id = id;
        this.trafficLightTimer = trafficLightTimer;
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
