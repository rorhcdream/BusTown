using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    public Grid BlockGrid { get; private set; }
    public Area Area { get; private set; }

    //상하좌우 중 신호등이 어디에 있는지 표시
    public List<Grid> LocalTrafficLightPos { get; private set; }
    //상하좌우 중 연결된 Block이 어디에 있는지 표시
    public List<Grid> LocalConnectedBlockPos { get; private set; }

    //Grid의 Offset이 0 이외의 값을 갖는 것을 허용하지 않는다.
    public Block(Grid blockGrid, Area area)
    {
        if ((Mathf.Abs(blockGrid.offset_x) + Mathf.Abs(blockGrid.offset_y) > GameMgr.FLOAT_MARGIN_OF_ERROR))
            throw new System.Exception();

        this.BlockGrid = blockGrid;
        this.Area = area;
    }

    public Vector2 GetRandomRespawnArea()
    {
        return Vector2.zero;
    }

    public void SetArea(Area area)
    {
        this.Area = area;
    }

    /// <summary>
    /// pos는 상하좌우 중 하나이다.
    /// 예를 들어 상의 경우 Grid(0,1)과 같이 표시하면 된다.
    /// 중복을 허용하지 않는다.
    /// </summary>
    /// <param name="pos">Position.</param>
    public void AddTrafficLight(Grid pos)
    {
        foreach(Grid grid in LocalTrafficLightPos)
        {
            if (grid == pos) throw new System.NotSupportedException();
        }
        LocalTrafficLightPos.Add(pos);
    }

    public void RemoveTrafficLight(Grid pos)
    {
        for (int i = 0; i < LocalTrafficLightPos.Count; i++)
        {
            if(LocalTrafficLightPos[i] == pos)
            {
                LocalTrafficLightPos.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// pos는 상하좌우 중 하나이다.
    /// 예를 들어 상의 경우 Grid(0,1)과 같이 표시하면 된다.
    /// 중복을 허용하지 않는다.
    /// </summary>
    /// <param name="pos">Position.</param>
    public void AddConnectedBlock(Grid pos)
    {
        foreach (Grid grid in LocalConnectedBlockPos)
        {
            if (grid == pos) throw new System.NotSupportedException();
        }
        LocalConnectedBlockPos.Add(pos);
    }

}
