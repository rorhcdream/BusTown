using System;
using System.Collections.Generic;

//길찾기를 수행할 수 있는 GridGraph<SidewalkNode> 클래스
class GridGraph_SidewalkNode : GridGraph<SidewalkNode>
{
    //to use PriorityQueue
    private class Grid_Weight_Pair : IComparable<Grid_Weight_Pair>
    {
        public Grid g { get; private set; }
        public float w { get; private set; }

        public Grid_Weight_Pair(Grid g, float w)
        {
            this.g = g;
            this.w = w;
        }
        public int CompareTo(Grid_Weight_Pair other)
        {
            return this.w.CompareTo(other.w);
        }
    }

    //use Dijkstra to find path
    public Queue<SidewalkNode> FindPath(Grid start, Area dest)
    {
        Dictionary<Grid, float> weight = new Dictionary<Grid, float>();
        Dictionary<Grid, bool> mark = new Dictionary<Grid, bool>();
        Dictionary<Grid, Grid> previousGrid = new Dictionary<Grid, Grid>(); //경로 생성을 위해 Grid로 들어온 경로 저장
        PriorityQueue<Grid_Weight_Pair> prQueue = new PriorityQueue<Grid_Weight_Pair>();

        weight.Add(start, 0f);
        mark.Add(start, true);
        prQueue.Enqueue(new Grid_Weight_Pair(start, 0));

        while(!prQueue.isEmpty())
        {
            Grid curGrid = prQueue.Dequeue().g;

            Vertex<SidewalkNode> curVertex;
            vertexDictionary_TryGetValue(curGrid, out curVertex, false);

            //원하는 Area에 도달했는지 체크
            SidewalkNode sn = curVertex.GetItem();
            if(sn is SidewalkNode_Entrance && (sn as SidewalkNode_Entrance).area == dest)
            {
                //previousGrid를 이용해 path 역추적
                LinkedList<SidewalkNode> path = new LinkedList<SidewalkNode>();
                Grid cur = curGrid, prev;
                do
                {
                    Vertex<SidewalkNode> v;
                    vertexDictionary_TryGetValue(cur, out v, false);
                    path.AddFirst(v.GetItem());
                }
                while (previousGrid.TryGetValue(cur, out prev)) ;

                return new Queue<SidewalkNode>(path);
            }


            //이미 마킹된 경우
            if (mark.ContainsKey(curGrid)) continue;
            //마킹되지 않은 vertex들 중 최소 weight를 가지는 경우
            else
            {
                mark[curGrid] = true;

                float curVertexWeight = weight[curGrid];           

                //현재 vertex에서 갈 수 있는 모든 vertex의 weight 수정, 
                //priorityQueue에 삽입
                foreach(GridEdge e in curVertex.GetEdges())
                {
                    Grid end = e.EndVertexGrid;
                    float edgeWeight = e.Weight;
                    float endVertexWeight;

                    //전에 weight를 부여했던 경우
                    if(weight.TryGetValue(end, out endVertexWeight))
                    {
                        float newWeight = curVertexWeight + edgeWeight;

                        //기존 weight보다 새 weight가 작은지 검사
                        if (newWeight <= endVertexWeight)
                        {
                            weight[end] = newWeight;
                            prQueue.Enqueue(new Grid_Weight_Pair(end, newWeight));
                            previousGrid[end] = curGrid;
                        }
                    }
                    //전에 weight를 부여하지 않았던 경우
                    else
                    {
                        float newWeight = curVertexWeight + edgeWeight;
                        weight[end] = newWeight;
                        prQueue.Enqueue(new Grid_Weight_Pair(end, newWeight));
                        previousGrid[end] = curGrid;
                    }
                }
            }
            
        }

        //dest에 해당하는 Area가 존재하지 않을 때
        throw new ArgumentException("Area doesn't exist", "dest");
    }
}
