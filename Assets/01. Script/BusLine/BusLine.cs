using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BusLine : MonoBehaviour
{
    private LinkedList<Bus> busList;

    private List<DrivewayNode> path;


    /// <summary>
    /// busStation이 추가될 때 DirectGraph의 Weight들의 수정을 
    /// busStation에서 Grid를 받아온 뒤 이를 Map에 제공하면서 Map에게 제안함
    /// 그렇게 하기 위해서는 busStationList에는 경로 순서대로 저장되어야 함
    /// </summary>
    private LinkedList<BusStation> busStationList;


}
