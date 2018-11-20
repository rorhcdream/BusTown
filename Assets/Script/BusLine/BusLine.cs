using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BusLine : MonoBehaviour
{
    private LinkedList<Bus> busList;
    private List<DrivewayNode> path;
    private LinkedList<BusStation> busStationList;

    public float estimatedTime { get; private set; }

    //BusLine이 변경될 때마다, Bus의 속도가 바뀔 때마다, BusLine 내의 Bus의 개수가 바뀔 때마다 호출
    private void UpdateWaitTime(float timeDelta)
    {

    }

}
