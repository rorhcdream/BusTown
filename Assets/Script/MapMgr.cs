using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Map mgr.
/// </summary>
public class MapMgr : MonoBehaviour
{
    //Graph를 (-MAX_i_GRID, +MAX_i_GRID)까지 생성
    public static readonly int MAX_X_GRID = 10;
    public static readonly int MAX_Y_GRID = 10;
    public static readonly int CONNECTED_BLOCK_NUM = 10;
    public static readonly float SIDEWALK_WIDTH = 0.5f;
    public static readonly float DRIVEWAY_WIDTH = 1.5f;
    public static readonly float BLOCK_WIDTH = 3f;
    public static readonly float BLOCK_HEIGHT = 2.4f;
    public static readonly float RANGE_TRAFFICLIGHT_MAY_EXIST = 0.8f;

    public readonly float walking_weight_coefficient = 1 / GameMgr.WALKING_SPEED;
    public readonly float bus_weight_coefficient = 1 / GameMgr.BUS_SPEED;


    private GridGraph_Node sidewalkGraph;
    private GridGraph<DrivewayNode> drivewayGraph;
    private Dictionary<Grid,Block> blockTable;
    private Frame frame;

    //set frame
    public void Start()
    {
        frame = new Frame();
    }

    //blockTable에서 key값을 찾을 수 없으면 에러를 발생시킨다.
    private Block blockTable_TryGetValue(Grid key)
    {
        Block block;
        if(!blockTable.TryGetValue(key, out block))
        {
            throw new System.Exception();
        }
        return block;
    }

    private Vector2 GetSidewalkNodePos(int block_x, int block_y, float offset_x, float offset_y)
    {
        return new Vector2((BLOCK_HEIGHT + SIDEWALK_WIDTH * 2 + DRIVEWAY_WIDTH) * block_x + (BLOCK_HEIGHT + SIDEWALK_WIDTH) * offset_x,
                           (BLOCK_WIDTH + SIDEWALK_WIDTH * 2 + DRIVEWAY_WIDTH) * block_y + (BLOCK_WIDTH + SIDEWALK_WIDTH) * offset_y);
    }
    private Vector2 GetSidewalkNodePos(Grid grid)
    {
        return GetSidewalkNodePos(grid.block_x, grid.block_y, grid.offset_x, grid.offset_y);
    }

    public void InitializeMapGraph()
    {
        sidewalkGraph = new GridGraph_Node();
        drivewayGraph = new GridGraph<DrivewayNode>();
        blockTable = new Dictionary<Grid, Block>();

        //sidewalkGraph 노드를 전부 생성한다.
        //block별로 입구 노드를 만들어준뒤, block과 연결해준다.
        //block을 생성한 후 blockTable에 넣는다.
        for (int x = -MAX_X_GRID; x <= MAX_X_GRID; x++)
        {
            for (int y = -MAX_Y_GRID; y <= MAX_Y_GRID; y++)
            {
                Grid blockGrid = new Grid(x, y);
                blockTable.Add(blockGrid, new Block(blockGrid, Area.Default));

                for (int i = 0; i < 4; i++)
                {
                    float offset_x = (float)(i % 2);
                    float offset_y = (float)((i>>1) % 2);

                    Grid grid = new Grid(x, y, offset_x, offset_y);
                    Vector2 pos = GetSidewalkNodePos(x, y, offset_x, offset_y);
                    sidewalkGraph.AddVertex(new SidewalkNode(grid, pos), grid);
                }


                //set enterenceGrid
                float randomOffsetX = Random.Range(0f, 1f);
                float randomOffsetY = Random.Range(0f, 1f);

                bool horizontalOffset = Random.Range(0, 2) == 0 ? true : false; //horizontal or vertical
                if (horizontalOffset)
                    randomOffsetY = Mathf.Round(randomOffsetY);
                else
                    randomOffsetX = Mathf.Round(randomOffsetX);

                Grid enterenceGrid = new Grid(x, y, randomOffsetX, randomOffsetY);
                sidewalkGraph.AddVertex(new SidewalkNode(enterenceGrid, GetSidewalkNodePos(enterenceGrid)),enterenceGrid);
               }
        }

        //blockGraph의 ConnectedBlock을 설정한다.
        for (int i = 0; i < CONNECTED_BLOCK_NUM; i++)
        {
            int x = Random.Range(-MAX_X_GRID, MAX_Y_GRID + 1);
            int y = Random.Range(-MAX_Y_GRID, MAX_Y_GRID + 1);

            int setx = Random.Range(0, 2);
            int sety = 1 - setx;
            int value = Random.Range(0, 2) * 2 - 1;
            int offx = value * setx;
            int offy = value * sety;
            

            Block pair1, pair2;

            //pair1과 pair2에 Block을 연결해준다.
            //불가능한 Grid가 설정되었을 때에는 재시도한다.
            if (!(blockTable.TryGetValue(new Grid(x, y), out pair1) 
                  &&(blockTable.TryGetValue(new Grid(x + offx, y + offy), out pair2))))
            {
                i--;
                continue;
            }

            //이미 연결되어 있을 때에는 재시도한다.
            foreach(Grid grid in pair1.LocalConnectedBlockPos)
            {
                if(grid == pair2.BlockGrid)
                {
                    i--;
                    continue;
                }
            }

            pair1.AddConnectedBlock(pair2.BlockGrid);
            pair2.AddConnectedBlock(pair1.BlockGrid);
        }

        //sidewalkGraph 노드를 연결한다.
        for (int x = -MAX_X_GRID; x <= MAX_X_GRID; x++)
        {
            for (int y = -MAX_Y_GRID; y <= MAX_Y_GRID; y++)
            {
                for (int i = 0; i < 4; i++)
                {
                    float offset_x = (float)(i % 2);
                    float offset_y = (float)((i >> 1) % 2);

                    Grid homeGrid = new Grid(x, y, offset_x, offset_y);
                    Grid grid1 = new Grid(x, y, 1.0f - offset_x, offset_y);
                    Grid grid2 = new Grid(x, y, offset_x, 1 - offset_y);

                    sidewalkGraph.AddEdge(homeGrid, grid1, walking_weight_coefficient);
                    sidewalkGraph.AddEdge(homeGrid, grid2, walking_weight_coefficient);
                }

                //연결된 블록끼리 연결해준다.
                //연결된 블록 사이에 있는 노드는 삭제한다.
                Block block = blockTable_TryGetValue(new Grid(x, y));

                foreach (Grid grid in block.LocalConnectedBlockPos)
                {
                    Grid connectedBlockGrid = new Grid(x + grid.block_x, y + grid.block_y);
                    Block connectedBlock = blockTable_TryGetValue(connectedBlockGrid);

                    Grid nodeGrid1;
                    Grid nodeGrid2;
                    Grid cnodeGrid1;
                    Grid cnodeGrid2;

                    //grid에 따라 nodeGrid1과 nodeGrid2를 결정해준다.
                    //상
                    if (grid == Grid.up)
                    {
                        nodeGrid1 = new Grid(0, 1);
                        nodeGrid2 = new Grid(1, 1);
                        cnodeGrid1 = new Grid(0, 0);
                        cnodeGrid2 = new Grid(1, 0);
                    }
                    //하
                    else if (grid == Grid.down)
                    {
                        nodeGrid1 = new Grid(0, 0);
                        nodeGrid2 = new Grid(0, 1);
                        cnodeGrid1 = new Grid(1, 0);
                        cnodeGrid2 = new Grid(1, 1);
                    }
                    //좌
                    else if (grid == Grid.left)
                    {
                        nodeGrid1 = new Grid(0, 0);
                        nodeGrid2 = new Grid(1, 0);
                        cnodeGrid1 = new Grid(0, 1);
                        cnodeGrid2 = new Grid(1, 1);
                    }
                    //우
                    else if (grid == Grid.right)
                    {
                        nodeGrid1 = new Grid(1, 0);
                        nodeGrid2 = new Grid(1, 1);
                        cnodeGrid1 = new Grid(0, 0);
                        cnodeGrid2 = new Grid(0, 1);
                    }
                    else throw new System.Exception();
                    
                    sidewalkGraph.RemoveEdge(nodeGrid1, nodeGrid2);
                    sidewalkGraph.RemoveEdge(nodeGrid2, nodeGrid1);

                    sidewalkGraph.AddEdge(nodeGrid1, cnodeGrid1, walking_weight_coefficient);
                    sidewalkGraph.AddEdge(nodeGrid2, cnodeGrid2, walking_weight_coefficient);
                }
            }
        }

        //dirvewayGraph 노드를 전부 생성한다.
        for (int x = -MAX_X_GRID+1; x <= MAX_X_GRID; x++)
        {
            for (int y = -MAX_Y_GRID+1; y <= MAX_Y_GRID; y++)
            {
                Grid grid = new Grid(x, y);
                Vector2 pos = new Vector2((BLOCK_HEIGHT + SIDEWALK_WIDTH * 2 + DRIVEWAY_WIDTH) * x - DRIVEWAY_WIDTH * 0.5f,
                                          (BLOCK_WIDTH + SIDEWALK_WIDTH * 2 + DRIVEWAY_WIDTH) * y - DRIVEWAY_WIDTH * 0.5f);
                drivewayGraph.AddVertex(new DrivewayNode(DrivewayNodeType.Default, grid, pos), grid);                
            }
        }

        //drivewayGraph 노드를 전부 연결한다.
        for (int x = -MAX_X_GRID + 1; x <= MAX_X_GRID; x++)
        {
            for (int y = -MAX_Y_GRID + 1; y <= MAX_Y_GRID; y++)
            {
                Grid grid = new Grid(x, y);
                foreach (Grid udlr in Grid.UDLR)
                {
                    //first나 end가 존재하지 않는다면 연결하지 않는다.
                    drivewayGraph.AddEdge(grid, grid + udlr, bus_weight_coefficient, true);
                }
            }
        }

        //연결된 Block 사이에 연결된 drivewaynode를 끊어준다.
        for (int x = -MAX_X_GRID + 1; x <= MAX_X_GRID; x++)
        {
            for (int y = -MAX_Y_GRID + 1; y <= MAX_Y_GRID; y++)
            {
                Grid blockGrid = new Grid(x, y);
                Block block = blockTable_TryGetValue(blockGrid);
                List<Grid> connectedBlockPos = block.LocalConnectedBlockPos;

                foreach(Grid grid in connectedBlockPos)
                {
                    if(grid == Grid.left)
                    {
                        drivewayGraph.RemoveEdge(blockGrid, blockGrid + Grid.up, true);
                        drivewayGraph.RemoveEdge(blockGrid + Grid.up, blockGrid, true);
                    }
                    if(grid == Grid.down)
                    {
                        drivewayGraph.RemoveEdge(blockGrid, blockGrid + Grid.right, true);
                        drivewayGraph.RemoveEdge(blockGrid + Grid.right, blockGrid, true);
                    }
                }
            }
        }
        drivewayGraph.DestoryIsolatedVertex();

        //TrafficLight 위치를 설정한다.
        {
            bool[,] isConnected = new bool[2 * MAX_X_GRID + 1, 2 * MAX_Y_GRID + 1];


            int x = Random.Range(-MAX_X_GRID, MAX_X_GRID + 1);
            int y = Random.Range(-MAX_Y_GRID, MAX_Y_GRID + 1);
            Grid udlr = Grid.GetRandomUDLR();

            Stack<Grid> gridStack = new Stack<Grid>();
            isConnected[x + MAX_X_GRID, y + MAX_Y_GRID] = true;
            gridStack.Push(new Grid(x, y));
            foreach(Grid connectedBlockPos in blockTable_TryGetValue(new Grid(x,y)).LocalConnectedBlockPos)
            {
                isConnected[x + connectedBlockPos.block_x + MAX_X_GRID, y + connectedBlockPos.block_y + MAX_Y_GRID] = true;
            }


            //DFS
            while (gridStack.Count != 0)
            {
                Grid nowGrid = gridStack.Pop();

                //blockGrid가 Connected일 경우
                if (isConnected[nowGrid.block_x + MAX_X_GRID, nowGrid.block_y + MAX_Y_GRID])
                {
                    for (int i = 0; i < 4; i++)
                    {
                        //90도 회전변환 한다.
                        udlr = new Grid(-udlr.block_y, udlr.block_x);
                        Grid grid = nowGrid + udlr;

                        if (Mathf.Abs(grid.block_x) > MAX_X_GRID || Mathf.Abs(grid.block_y) < MAX_Y_GRID)
                        {
                            continue;
                        }


                        //주변에 unconnected가 있을 경우 연결하고, 연결한 블록에 대해 재귀한다.
                        if (isConnected[grid.block_x + MAX_X_GRID, grid.block_y + MAX_Y_GRID] == false)
                        {
                            blockTable_TryGetValue(nowGrid).AddTrafficLight(grid);
                            blockTable_TryGetValue(grid).AddTrafficLight(nowGrid);
                            isConnected[grid.block_x + MAX_X_GRID, grid.block_y + MAX_Y_GRID] = true;
                            gridStack.Push(grid);
                            foreach (Grid connectedBlockPos in blockTable_TryGetValue(grid).LocalConnectedBlockPos)
                            {
                                isConnected[x + connectedBlockPos.block_x + MAX_X_GRID, y + connectedBlockPos.block_y + MAX_Y_GRID] = true;
                            }
                        }
                    }
                }
            }
        }

        //TrafficLight를 생성한다.
        for (int x = -MAX_X_GRID; x <= MAX_X_GRID; x++)
        {
            for (int y = -MAX_Y_GRID; y <= MAX_Y_GRID; y++)
            {
                Grid blockGrid = new Grid(x, y);
                foreach(Grid trafficPos in blockTable_TryGetValue(blockGrid).LocalTrafficLightPos)
                {
                    if(trafficPos == Grid.up)
                    {
                        float ratio = Random.Range((1 - RANGE_TRAFFICLIGHT_MAY_EXIST) * 0.5f, (1 + RANGE_TRAFFICLIGHT_MAY_EXIST) * 0.5f);
                        Vector2 sNodePos1 = GetSidewalkNodePos(x, y, ratio, 1);
                        Vector2 sNodePos2 = GetSidewalkNodePos(x, y + 1, ratio, 0);
                        Vector2 dNodePos = (sNodePos1 + sNodePos2) * 0.5f;


                        TrafficLightTimer trafficLightTimer = new TrafficLightTimer();
                        SidewalkNode sidewalkNode1 = new SidewalkNode(SidewalkNodeType.TrafficLight,
                                                                      new Grid(x, y, ratio, 1),
                                                                      sNodePos1);
                        SidewalkNode sidewalkNode2 = new SidewalkNode(SidewalkNodeType.TrafficLight,
                                                                      new Grid(x, y + 1, ratio, 0),
                                                                      sNodePos2);
                        DrivewayNode drivewayNode = new DrivewayNode(DrivewayNodeType.TrafficLight,
                                                                     new Grid(x, y + 1, ratio, 0),
                                                                     dNodePos);


                    }
                }

            }
        }






    }

    /// <summary>
    /// 랜덤 노드를 생성하고, 그 노드부터 dest까지의 최단거리를 반환한다.
    /// </summary>
    /// <returns>The path.</returns>
    /// <param name="dest">Destination.</param>
    public Queue<SidewalkNode> GetPathFromRandomNode(Area dest)
    {
        Grid newGrid = frame.GetRandomGridInFrame();

        return new Queue<SidewalkNode>(sidewalkGraph.FindPath(newGrid, dest));
    }
}