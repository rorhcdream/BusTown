using UnityEngine;
using System.Collections;

//SidewalkNodeType.Default
public class SidewalkNode : MonoBehaviour, IPosition
{
    public Grid Grid
    {
        get;
        private set;
    }
    public Vector2 Position { get; private set; }

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
