using System.Collections.Generic;
using UnityEngine;
using System;

public class Node: IComparable<Node>
{
    public List<Node> Neighbors { get; set; }
    public Vector3 Position { get; set; }
    public Node Previous { get; set; }
    public float Priority { get; set; }

    public int XIndex
    { get { return _xIndex; } private set { _xIndex = value; } }
    public int YIndex
    { get { return _yIndex; } private set { _yIndex = value; } }
    public NodeType NodeType
    { get { return _nodeType; } private set { _nodeType = value; } }
    public float Weight
    { get { return _weight; } private set { _weight = value; } }
    public float CostSoFar
    { get { return _costSoFar; } set { _costSoFar = value; } }

    private int _xIndex = -1;
    private int _yIndex = -1;
    private NodeType _nodeType = NodeType.Open;
    private float _weight = 0;
    private float _costSoFar = Mathf.Infinity;

    ///<summary>Node constructor</summary>
    public Node(int xIndex, int yIndex, NodeType nodeType)
    {
        _xIndex = xIndex;
        _yIndex = yIndex;
        _nodeType = nodeType;

        _weight = MapData.GetWeightByNodeType(nodeType);
    }

    ///<summary>Compare node priorty with other node priority</summary>
    public int CompareTo(Node other)
    {
        if (Priority < other.Priority)
        {
            return -1;
        }
        else if (Priority > other.Priority)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    ///<summary>Sets Previous propery to null</summary>
    public void ResetPrevious()
    {
        Previous = null;
    }

}
