using System.Collections.Generic;
using UnityEngine;

///<summary>Class that manages all nodes</summary>
public class Graph : MonoBehaviour
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public Node[,] Nodes { get; private set; }

    private int[,] _mapData;
    private List<Node> _walls = new List<Node>();

    //directions for neighbour check
    private readonly Vector2[] allDirections =
    {
        new Vector2(0f,1f),
        new Vector2(1f,1f),
        new Vector2(1f,0f),
        new Vector2(1f,-1f),
        new Vector2(0f,-1f),
        new Vector2(-1f,-1f),
        new Vector2(-1f,0f),
        new Vector2(-1f,1f)
    };

    ///<summary>Initializes the graph using numerical map data</summary>
    public void Init(int[,] mapData)
    {
        _mapData = mapData;

        Width = _mapData.GetLength(0);
        Height = _mapData.GetLength(1);

        Nodes = new Node[Width, Height];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                NodeType type = (NodeType)_mapData[x, y];

                Node newNode = new Node(x, y, type);
                Nodes[x, y] = newNode;

                newNode.Position = new Vector3(x, y, 0);

                if (type == NodeType.Blocked)
                {
                    _walls.Add((newNode));
                }
            }
        }

        SetupNeighbourNodes();
    }

    ///<summary>Sets up neighbor Nodes for each node</summary>
    void SetupNeighbourNodes()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (Nodes[x, y].NodeType != NodeType.Blocked)
                {
                    Nodes[x, y].Neighbors = GetNeighbors(x, y);
                }
            }
        }
    }

    ///<summary>Checks if x and y are in the Graph bounds</summary> 
    public bool IsWithinBounds(int x, int y)
    {
        return (x >= 0 && x < Width && y >= 0 && y < Height);
    }

    ///<summary>Checks if x and y are valid nodes</summary> 
    public bool IsValidNode(int x, int y)
    {
        return (Nodes[x, y].NodeType != NodeType.Blocked);
    }

    ///<summary>Returns a list of neighours from x and y, array of Nodes and given directions</summary>
    List<Node> GetNeighbors(int x, int y, Node[,] nodeArray, Vector2[] directions)
    {
        List<Node> neighborNodes = new List<Node>();

        foreach (Vector2 dir in directions)
        {
            int newX = x + (int)dir.x;
            int newY = y + (int)dir.y;

            if (IsWithinBounds(newX, newY) && nodeArray[newX, newY] != null && nodeArray[newX, newY].NodeType != NodeType.Blocked)
            {
                neighborNodes.Add(nodeArray[newX, newY]);
            }
        }

        return neighborNodes;
    }

    ///<summary>Returns a list of neighors from x and y</summary>
	List<Node> GetNeighbors(int x, int y)
    {
        return GetNeighbors(x, y, Nodes, allDirections);
    }

}
