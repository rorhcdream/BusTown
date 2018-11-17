using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bus : MonoBehaviour 
{
    private List<Citizen> passengers = new List<Citizen>();
    private List<DrivewayNode> path;
}