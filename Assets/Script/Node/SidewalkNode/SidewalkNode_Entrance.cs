using System;
using UnityEngine;

class SidewalkNode_Entrance : SidewalkNode
{
    public SidewalkNode_Entrance(Grid grid, Vector2 position, Area area) : base(grid, position)
    {
        this.area = area;
    }
    public Area area { get; private set; }

    public override SidewalkNodeType GetNodeType()
    {
        return SidewalkNodeType.Entrance;
    }
}

