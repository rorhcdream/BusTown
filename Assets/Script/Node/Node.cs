using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour, IPosition
{
    public Vector2 Position { get; protected set; }
}
