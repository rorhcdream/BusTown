using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen : MonoBehaviour
{
    private LinkedList<SidewalkNode> goToList;
    private Area dest;
    private bool isWaiting = false;  //신호등, 정류장 등의 이유로 이동하지 않고 대기 중인가?

    private static readonly float MOVEMENT_SPEED = 4.0f;    //사람의 이동 속도
    private static readonly float UPDATE_DELTATIME = 0.07f; //이동의 주기(초)
    

    public Citizen(LinkedList<SidewalkNode> gotoList)
    {
        SetGoToList(gotoList);
    }

    public void SetGoToList(LinkedList<SidewalkNode> gotoList)
    {
        if (gotoList == null)
            throw new ArgumentException("Citizen.gotoList is null", "gotoList");
        this.goToList = gotoList;

        //기존 이동을 멈추고 새롭게 시작
        StopCoroutine("MoveToDestination");
        StopCoroutine("Move");
        StartCoroutine(MoveToDestination());
    }

    void Start()
    {
        StartCoroutine(MoveToDestination());
    }

    //gameObject를 gotoList를 따라 이동시키는 코루틴
    IEnumerator MoveToDestination()
    {
        SidewalkNode startNode, endNode;

        startNode = goToList.First.Value;
        goToList.RemoveFirst();

        while(goToList.Count != 0)
        {
            endNode = goToList.First.Value;
            goToList.RemoveFirst();

            //현재 노드가 신호등인 경우 체크
            if (startNode.GetNodeType() == SidewalkNodeType.TrafficLight)
            {
                SidewalkNode_TrafficLight trafficLight = startNode as SidewalkNode_TrafficLight;
                if(!trafficLight.isBlueLight)
                {
                    isWaiting = true;
                    Action StopWaiting = () => isWaiting = false;
                    trafficLight.OnBlueLightSet += StopWaiting;

                    //신호등이 파란불이 될 때까지 대기
                    while(isWaiting)
                    {
                        yield return null;
                    }
                    trafficLight.OnBlueLightSet -= StopWaiting;
                }
            }

            //이동
            yield return StartCoroutine(Move(startNode.Position, endNode.Position));
            startNode = endNode;
        }
    }

    //gameObject를 start부터 end까지 이동시키는 코루틴
    IEnumerator Move(Vector2 start, Vector2 end)
    {
        gameObject.transform.position = start;      //위치를 start로 고정

        Vector2 delta = end - start;
        float distance = delta.sqrMagnitude;        //거리
        float time = distance / MOVEMENT_SPEED;     //걸리는 시간 (초)
        Vector2 speed = (end - start) / time;       //속도
        float timeTaken = 0f;                       //걸린 시간

        while(timeTaken < time)
        {
            transform.Translate(speed * UPDATE_DELTATIME);
            timeTaken += UPDATE_DELTATIME;

            yield return new WaitForSeconds(UPDATE_DELTATIME);
        }
    }
}
