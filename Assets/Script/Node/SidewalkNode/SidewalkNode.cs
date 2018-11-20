using UnityEngine;
using System.Collections;

//SidewalkNodeType.Default
public class SidewalkNode : Node
{
    public Grid Grid
    {
        get;
        private set;
    }

    public SidewalkNode(Grid grid, Vector2 position)
    {
        Grid = grid;
        Position = position;
    }

    public virtual SidewalkNodeType GetNodeType()
    {
        return SidewalkNodeType.Default;
    }
}
