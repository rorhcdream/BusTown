using System.Collections;
using System.Collections.Generic;

//정점을 나타내는 클래스이다.
public class Vertex<T>
{
    //간선을 원소로 갖는 링크드리스트, 역의 고유번호, 역의 이름, 그리고 정수 ID 하나를 멤버로 갖는다.
    //정수 ID는 인스턴스가 생성될 때마다 1씩 증가하여 부여된다.
    //정수 ID를 멤버로 가짐으로서 array 를 통한 dijkstra 의 구현이 간편해진다.
    private List<GridEdge> edges;
    private T item;

    public Vertex(T item)
    {
        edges = new List<GridEdge>();
        this.item = item;
    }

    //간선을 추가한다.
    public void AddEdge(GridEdge newEdge)
    {
        //목적지가 같은 Edge를 허용하지 않음.
        foreach(GridEdge gridEdge in edges)
        {
            if(gridEdge.EndVertexGrid == newEdge.EndVertexGrid)
            {
                throw new System.NotSupportedException();
            }
        }

        edges.Add(newEdge);
    }

    public bool RemoveEdge(Grid end, bool ignore_key_value_error = false)
    {
        for (int i = 0; i < edges.Count; i++)
        {
            if(edges[i].EndVertexGrid == end)
            {
                edges.RemoveAt(i);
                return true;
            }
        }
        if (ignore_key_value_error) return false;
        else throw new System.Exception();
    }

    public T GetItem()
    {
        return item;
    }

    override public string ToString()
    {
        return item.ToString();
    }

    //간선의 링크드리스트를 반환한다.
    public List<GridEdge> GetEdges()
    {
        return edges;
    }
}
