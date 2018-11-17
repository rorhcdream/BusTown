using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//간선을 나타내는 클래스이다.
public class GridEdge : MonoBehaviour
{
    //끝 정점의 ID를 관리하고
    //W는 Weight 인터페이스를 구현하는 클래스라면 뭐든 올 수 있다.
    public Grid EndVertexGrid { get; private set; }
    public float Weight { get; private set; }



    public GridEdge(Grid endVertexGrid, float weight)
    {
        this.EndVertexGrid = endVertexGrid;
        this.Weight = weight;
    }

}

//환승 최소화 탐색에 사용할 가중치이다.
/****
public class Weight
{
    private float weight;
    private int connected;

    public Weight(float weight/*, int connected*//*)
    {
        this.weight = weight;
        this.connected = connected;
    }

    public int CompareTo(Weight A, bool minimizeTransfer)
    {
        if (minimizeTransfer)
        {
            if (A == null)
                return -1;
            else
            {
                int key = this.connected - A.connected;

                if (key != 0) 
                    return key;
                else 
                    return this.weight - A.weight;
            }
        }
        else
        {
            if (A == null)
                return -1;
            else
                return this.weight - A.weight;
        }
    }

    public Weight add(Weight A)
    {
        return new Weight(this.weight + A.weight, this.connected + A.connected);
    }

    public String toString()
    {
        return "" + weight;
    }
}
**********/