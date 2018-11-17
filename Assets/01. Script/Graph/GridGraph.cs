using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//위치정보를 갖고 있는 클래스만 GridGraph에 담을 수 있다.
public class GridGraph<T> where T : IPosition 
{

    private int vertexNumber;
    private Dictionary<Grid, Vertex<T>> vertexDictionary;

    
    private bool vertexDictionary_TryGetValue(Grid key, out Vertex<T> value, bool ignore_key_value_error)
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

    /*******************************************
    public Vector2 GetVertexPosition(Grid grid)
    {
        Vertex<T> vertex;
        if(!vertexDictionary.TryGetValue(grid, out vertex))
        {
            throw new System.NotSupportedException();
        }
        if(!(vertex.GetItem() is IPosition))
        {
            throw new System.NotSupportedException();
        }
        return (vertex.GetItem() as IPosition).Position;
                                              
    }
    ********************************************/

    public bool AddEdge(Grid start, Grid end, float weightCoefficient, bool ignore_key_value_error = false)
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

    public bool RemoveEdge(Grid start, Grid end, bool ignore_key_value_error = false)
    {
        Vertex<T> vertex_start;

        bool ret1 = vertexDictionary_TryGetValue(start, out vertex_start, ignore_key_value_error);

        if (!ret1) return false;

        bool ret2 = vertex_start.RemoveEdge(end, ignore_key_value_error);
        return ret2;
    }


    /// <summary>
    /// start와 end 사이에 item을 추가한다.
    /// </summary>
    /// <param name="item">Item.</param>
    /// <param name="start">Vertex0.</param>
    /// <param name="end">Vertex1.</param>
    /// <param name="ratio">Ratio.</param>
    public void AddVertexBetweenVertex(T item, Grid key, Grid start, Grid end, float weightCoefficient)
    {
        AddVertex(item, key);
        AddEdge(start, key, weightCoefficient);
        AddEdge(key, end, weightCoefficient);
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

    public LinkedList<T> Dijkstra(Grid start, Area end)
    {

        return null;
    }



}
