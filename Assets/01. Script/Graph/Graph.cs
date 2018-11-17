﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class Graph<T>
{
    private List<Vertex> vertexList;
    private int vertexNumber;

    public Graph()
    {
        vertexList = new List<Vertex>();
        vertexNumber = 0;
    }

    virtual public void AddVertex(T item)
    {
        Vertex vertex = new Vertex(item);
        vertexNumber++;
        vertexList.Add(vertex);
    }

    public void AddEdge(int start, int end, float weight)
    {
        vertexList[start].AddEdge(new GridEdge(end, weight));
    }

    public Weight Dijkstra(LinkedList<int> startingVertices, LinkedList<int> endVertices, bool minimizeTransfer, List<T> result)
    {
        if (startingVertices == null)
            throw new System.Exception();

        if (endVertices == null)
            throw new System.Exception();

        Weight min = new Weight(0, 0);
        //i번째 정점까지의 현재 최소 거리를 저장하는 W 배열이다.
        Weight[] distance = new Weight[vertexNumber];

        //i번째 정점에 도달하는 최단 경로에서 바로 직전에 방문한 정점의 번호를 저장하는 배열이다.
        //도착지점에서부터 출발지점에 다다를 때까지 역추적하면 전체 경로를 알 수 있다.
        int[] previousVertexID = new int[vertexNumber];

        //정점의 확정 여부를 저장하는 boolean 배열이다.
        bool[] vertexIsIncluded = new bool[vertexNumber];

        //포함시킨 정점이 도착지점인지 쉽게 조사하기 위한 boolean 배열이다.
        bool[] isEndVertex = new bool[vertexNumber];

        //dijkstra 를 효율적으로 구현하기 위한 우선순위 큐이다.
        PriorityQueue<PathWeight> priorityQueue = new PriorityQueue<PathWeight>();

        //여러 개의 가능한 도착지점들 중 하나라도 만나면 아래 변수에 그 정점을 대입하고 바로 탈출할 것이다.
        int endVertex;

        //존재하는 모든 정점들에 대해 초기화를 진행한다.
        for(int i = 0; i<vertexNumber; i++) {
            distance[i] = null;
            previousVertexID[i] = -1;
            vertexIsIncluded[i] = false;
            isEndVertex[i] = false;
        }

        foreach(int i in startingVertices) {
            //startVertices 에 들어있는 모든 정점이 출발지점이므로
            //이들에게 확정 여부를 의미하는 vertexIsIncluded = true 와 거리 0, 그리고 출발 지점을 의미하는 previousVertexID = -1 을 부여한다.
            distance[i] = min;
            previousVertexID[i] = -1;
            vertexIsIncluded[i] = true;

            //그리고 출발점들과 간선으로 연결된 정점들에 대해 거리를 업데이트한다.
            foreach(GridEdge edge in vertexList[i].getEdges()) {
                int n = edge.GetEndVertexID();
                //거리가 단축이 가능할 경우(Weight가 더 작은 경우)
                if(edge.Weight().CompareTo(distance[n], minimizeTransfer) < 0) {

                    //거리를 업데이트하고
                    distance[n] = edge.Weight();//] = edge.getWeight();

                    //그 거리를 우선순위 큐에 삽입한 다음
                    priorityQueue.Enqueue(new PathWeight(n, edge.Weight()),minimizeTransfer);

                    //최근 방문한 정점을 현재 조사중인 정점으로 설정한다.
                    previousVertexID[n] = i;
                }
            }
        }

        foreach(int i in endVertices)
            isEndVertex[i] = true;

        //************************초기화*끝*&*dijkstra*시작**********************
        while(true) 
        {
            int closestVertexID;

            //방문하지 않았던 정점들 중 거리가 최소인 것을 찾는다.
            while(true) 
            {
                PathWeight minimum = priorityQueue.delete(minimizeTransfer);
                if(vertexIsIncluded[minimum.GetDestinyVertexID()])
                    continue;
                closestVertexID = minimum.destinyVertexID;
                break;
            }

            //해당 정점을 확정짓고
            Vertex closestVertex = vertexArrayList.get(closestVertexID);
            vertexIsIncluded[closestVertexID] = true;

            //정점에 연결된 정점들에 대해 최소 거리를 업데이트한다.
            for(GridEdge edge : closestVertex.getEdges()) {
                int n = edge.GetEndVertexID();
                if(distance.get(closestVertexID).add(edge.Weight()).compareTo(distance.get(n), minimizeTransfer) < 0) {
                    distance.set(n, distance.get(closestVertexID).add(edge.Weight()));
                    previousVertexID[n] = closestVertexID;
                    priorityQueue.insert(new PathWeight(n, distance.get(n)),minimizeTransfer);
                }
            }

            //방금 포함시킨 정점이 도착지점이라면
            if(isEndVertex[closestVertexID]) {
                //그 정점을 endVertex 에 대입하고 탐색을 종료한다.
                endVertex = closestVertexID;
                break;
            }
        }

        //탐색이 끝났으므로 도착지점으로부터 previousVertexID 를 역추적하면서 경로를 알아내면 된다.
        for(int i = endVertex; i != -1; i = previousVertexID[i]) {
            result.add(vertexArrayList.get(i).getItem());
        }
        return distance.get(endVertex);
    }



    public class Vertex : MonoBehaviour
    {
        //간선을 원소로 갖는 링크드리스트, 역의 고유번호, 역의 이름, 그리고 정수 ID 하나를 멤버로 갖는다.
        //정수 ID는 인스턴스가 생성될 때마다 1씩 증가하여 부여된다.
        //정수 ID를 멤버로 가짐으로서 array 를 통한 dijkstra 의 구현이 간편해진다.
        private LinkedList<GridEdge> edges;
        private T item;

        public Vertex(T item)
        {
            edges = new LinkedList<GridEdge>();
            this.item = item;
        }

        //간선을 추가한다.
        public void AddEdge(GridEdge newEdge)
        {
            edges.AddLast(newEdge);
        }

        T GetItem()
        {
            return item;
        }

        override public string ToString()
        {
            return item.ToString();
        }

        //간선의 링크드리스트를 반환한다.
        LinkedList<GridEdge> GetEdges()
        {
            return edges;
        }
    }


}


public class PathWeight : IComparable<T>
{
    int destinyVertexID;
    Weight weight;

    public PathWeight(int destinyVertexID, Weight weight)
    {
        this.destinyVertexID = destinyVertexID;
        this.weight = weight;
    }

    public int CompareTo(PathWeight A, bool minimizeTransfer)
    {
        return this.weight.CompareTo(A.weight, minimizeTransfer);
    }

    int GetDestinyVertexID()
    {
        return destinyVertexID;
    }
}