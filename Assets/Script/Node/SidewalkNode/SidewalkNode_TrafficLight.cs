using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SidewalkNode_TrafficLight : SidewalkNode
{
    //신호등 신호 변경 주기 관련
    private TrafficLightTimer trafficLightTimer;
    public bool isBlueLight
    { get { return trafficLightTimer.SidewalkOn; }}

    //시민 유입, 배출
    private static readonly float CITIZEN_OUTFLOW_DELTATIME = 0.1f;     //파란불이 되었을 때 시민이 대기열에서 출발하는 시간간격
    private static readonly Vector3 CITIZEN_QUEUE_DELTAPOS = new Vector3(0.5f, 0f, 0f);    //시민이 대기열에 서있는 위치간격
    private Queue<Action> OnBlueLightSet;   //파란불이 되었을 때 각 시민들이 행할 동작
    private int waitingCitizenCount = 0;

    //신호등의 id
    public readonly int id;     //짝이 맞는 신호등끼리는 같은 id를 가짐.

    
    
    public SidewalkNode_TrafficLight(Grid grid, Vector2 position, TrafficLightTimer trafficLightTimer, int id) : base(grid, position)
    {
        this.id = id;
        this.trafficLightTimer = trafficLightTimer;
        SetTrafficLightTimerEvent();

        citizenOutflowQueue = new Queue<Transform>();
    }

    //citizens call this method when they enter this node.
    public void EnterTrafficLight(Transform cTrans, Action onBlueLightSet)
    {
        cTrans.position = gameObject.transform.position + waitingCitizenCount * CITIZEN_QUEUE_DELTAPOS;
        OnBlueLightSet.Enqueue(onBlueLightSet);
        waitingCitizenCount++;
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
                (bool b) => 
                {
                    if (b && waitingCitizenCount > 0)
                        StartCoroutine(LetCitizensOut());
                };
        }
    }

    //make citizen go out from this node with deltatime
    private IEnumerator LetCitizensOut()
    {
        WaitForSeconds citizenOutflowDelta = new WaitForSeconds(CITIZEN_OUTFLOW_DELTATIME);
        while(waitingCitizenCount > 0)
        {
            (OnBlueLightSet.Dequeue()).Invoke();
            waitingCitizenCount--;
            yield return citizenOutflowDelta;
        }
    }
}
