using UnityEngine;
using System.Collections;

//SidewalkNodeType.Default
public class SidewalkNode : Node
{
    public SidewalkNode(Grid grid, Vector2 position) : base(grid, position)
    {}

    public virtual SidewalkNodeType GetNodeType()
    {
        return SidewalkNodeType.Default;
    }
}
