using System;
using UnityEngine;

class SidewalkNode_Entrance : SidewalkNode
{
    public Area area { get; private set; }

    public SidewalkNode_Entrance(Grid grid, Vector2 position, Area area) : base(grid, position)
    {
        this.area = area;
    }

    public override SidewalkNodeType GetNodeType()
    {
        return SidewalkNodeType.Entrance;
    }
}

