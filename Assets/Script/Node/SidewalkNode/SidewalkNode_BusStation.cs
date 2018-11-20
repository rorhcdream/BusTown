using System;
using UnityEngine;

class SidewalkNode_BusStation : SidewalkNode
{
    private BusLine busline;

    public SidewalkNode_BusStation(Grid grid, Vector2 position, BusLine busLine) : base(grid, position)
    {
        this.busline = busLine;
    }

    

    public override SidewalkNodeType GetNodeType()
    {
        return SidewalkNodeType.BusStation;
    }

    
}

