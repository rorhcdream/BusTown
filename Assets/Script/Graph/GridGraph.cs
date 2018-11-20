using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//위치정보를 갖고 있는 클래스만 GridGraph에 담을 수 있다.
public class GridGraph<T> where T : IPosition 
{

    private int vertexNumber;
    private Dictionary<Grid, Vertex<T>> vertexDictionary;

    
    protected bool vertexDictionary_TryGetValue(Grid key, out Vertex<T> value, bool ignore_key_value_error)
    {
        if (ignore_key_value_error)
        {
            return vertexDictionary.TryGetValue(key, out value);
        }
        else
        {
            if (!vertexDictionary.TryGetValue(key, out value))
            {
                throw new System.Exception();
            }
            return true;
        }

    }

    public GridGraph()
    {
        vertexNumber = 0;
        vertexDictionary = new Dictionary<Grid, Vertex<T>>();
    }

    public void AddVertex(T item, Grid grid)
    {
        Vertex<T> vertex = new Vertex<T>(item);
        vertexNumber++;
        vertexDictionary.Add(grid, vertex);
    }
    public bool AddEdgebyPosition(Grid start, Grid end, float weightCoefficient, bool ignore_key_value_error = false)
    {
        Vertex<T> vertex_start;
        Vertex<T> vertex_end;

        bool ret1 = vertexDictionary_TryGetValue(start, out vertex_start, ignore_key_value_error);
        bool ret2 = vertexDictionary_TryGetValue(end, out vertex_end, ignore_key_value_error);

        if (!(ret1 && ret2)) return false;

        float distance = Vector2.Distance(vertex_start.GetItem().Position, vertex_end.GetItem().Position);
        vertex_start.AddEdge(new GridEdge(end, distance * weightCoefficient));
        return true;
    }
    public bool AddEdge(Grid start, Grid end, float weight, bool ignore_key_value_error = false)
    {
        Vertex<T> vertex_start;
        Vertex<T> vertex_end;

        bool ret1 = vertexDictionary_TryGetValue(start, out vertex_start, ignore_key_value_error);
        bool ret2 = vertexDictionary_TryGetValue(end, out vertex_end, ignore_key_value_error);

        if (!(ret1 && ret2)) return false;

        vertex_start.AddEdge(new GridEdge(end, weight));
        return true;
    }
    public bool RemoveEdge(Grid start, Grid end, bool ignore_key_value_error = false)
    {
        Vertex<T> vertex_start;

        bool ret1 = vertexDictionary_TryGetValue(start, out vertex_start, ignore_key_value_error);

        if (!ret1) return false;

        bool ret2 = vertex_start.RemoveEdge(end, ignore_key_value_error);
        return ret2;
    }


    public void ChangeEdgeWeight(Grid start, Grid end, float weight)
    {
        Vertex<T> vertex_start;

        bool ret1 = vertexDictionary_TryGetValue(start, out vertex_start,false);

        List<GridEdge> edges = vertex_start.GetEdges();
        foreach(GridEdge edge in edges)
        {
            if (edge.EndVertexGrid == end) edge.Weight = weight;
        }

    }

    /// <summary>
    /// start와 end 사이에 item을 추가한다.
    /// edge는 단방향으로 생성한다.
    /// 
    /// 사실필요없음
    /// </summary>
    /// <param name="item">Item.</param>
    /// <param name="start">Vertex0.</param>
    /// <param name="end">Vertex1.</param>
    public void AddVertexBetweenVertex(T item, Grid key, Grid start, Grid end, float weightCoefficient)
    {
        AddVertex(item, key);
        AddEdgebyPosition(start, key, weightCoefficient);
        AddEdgebyPosition(key, end, weightCoefficient);
    }

    public void DestoryIsolatedVertex()
    {
        foreach(KeyValuePair<Grid, Vertex<T>> keyValuePair in vertexDictionary)
        {
            Vertex<T> vertex = keyValuePair.Value;

            if(vertex.GetEdges().Count == 0)
            {
                vertexDictionary.Remove(keyValuePair.Key);
            }
        }
    }



    //간선을 나타내는 클래스이다.
    public class GridEdge
    {
        //끝 정점의 ID를 관리하고
        //W는 Weight 인터페이스를 구현하는 클래스라면 뭐든 올 수 있다.
        public Grid EndVertexGrid { get; private set; }
        public float Weight { get; set; }

        public GridEdge(Grid endVertexGrid, float weight)
        {
            this.EndVertexGrid = endVertexGrid;
            this.Weight = weight;
        }


    }


    //정점을 나타내는 클래스이다.
    public class Vertex<X>
    {
        //간선을 원소로 갖는 링크드리스트, 역의 고유번호, 역의 이름, 그리고 정수 ID 하나를 멤버로 갖는다.
        //정수 ID는 인스턴스가 생성될 때마다 1씩 증가하여 부여된다.
        //정수 ID를 멤버로 가짐으로서 array 를 통한 dijkstra 의 구현이 간편해진다.
        private List<GridEdge> edges;
        private X item;

        public Vertex(X item)
        {
            edges = new List<GridEdge>();
            this.item = item;
        }

        //간선을 추가한다.
        public void AddEdge(GridEdge newEdge)
        {
            //목적지가 같은 Edge를 허용하지 않음.
            foreach (GridEdge gridEdge in edges)
            {
                if (gridEdge.EndVertexGrid == newEdge.EndVertexGrid)
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
                if (edges[i].EndVertexGrid == end)
                {
                    edges.RemoveAt(i);
                    return true;
                }
            }
            if (ignore_key_value_error) return false;
            else throw new System.Exception();
        }

        public X GetItem()
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
}
