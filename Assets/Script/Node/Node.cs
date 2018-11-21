using UnityEngine;
using System;

public abstract class Node : MonoBehaviour, IPosition
{
    public Node(Grid grid, Vector2 position)
    {
        Grid = grid;
        Position = position;
    }

    public Grid Grid
    {
        get;
        protected set;
    }

    public Vector2 Position { get; protected set; }
    
    
    public event Action<bool> OnPassableSet;
    //통행 가능한 노드인가?
    private bool passable = true;
    public bool Passable
    {
        get { return passable; }
        protected set
        {
            passable = value;
            if (OnPassableSet != null)
                OnPassableSet(value);
        }
    }    
}
