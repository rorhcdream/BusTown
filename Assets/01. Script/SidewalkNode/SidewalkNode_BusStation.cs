using System;
using UnityEngine;

class SidewalkNode_BusStation : SidewalkNode
{
    public SidewalkNode_BusStation(Grid grid, Vector2 position) : base(grid, position)
    { }

    public override SidewalkNodeType GetNodeType()
    {
        return SidewalkNodeType.BusStation;
    }

    
}

