using UnityEngine;
using System.Collections;


public class DrivewayTrafficLight : MonoBehaviour
{
    private TrafficLight trafficLight;

    public void SetTrafficLight(TrafficLight trafficLight)
    {
        this.trafficLight = trafficLight;
    }
}
