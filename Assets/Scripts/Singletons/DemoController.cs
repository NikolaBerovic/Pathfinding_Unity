using System.Collections.Generic;
using UnityEngine;

///<summary>Class controller for test pathfinding</summary>
public class DemoController : Singleton<MonoBehaviour>
{
    //references
    [SerializeField] private MapData _mapData;
    [SerializeField] private Graph _graph;
    [SerializeField] private Pathfinder _pathfinder;

    //test coordinates
    [SerializeField] private int _startX = 0;
    [SerializeField] private int _startY = 0;
    [SerializeField] private int _tagetX = 8;
    [SerializeField] private int _targetY = 1;

    void Start()
    {
        if (!CheckInitialRequirements())
        { return; }

        int[,] map = _mapData.GetLoadedMap();
        _graph.Init(map);

        GraphView _graphVisualized = _graph.gameObject.GetComponent<GraphView>();
        if (_graphVisualized != null)
        { _graphVisualized.Init(_graph); }

        if (!CheckPathfindingRequirements())
        { return; }

        Node startNode = _graph.Nodes[_startX, _startY];
        Node targetNode = _graph.Nodes[_tagetX, _targetY];

        List<Node> exploredNodes = new List<Node>();
        List<Node> path = new List<Node>();

        path = _pathfinder.FindPath(startNode, targetNode, out exploredNodes);
        _graphVisualized.VisualizeFullPath(path, exploredNodes);
    }

    ///<summary>Checks all required references for controller</summary>
    bool CheckInitialRequirements()
    {
        if (_mapData == null)
        {
            Debug.LogError("Map Data not set");
            return false;
        }
        if (_graph == null)
        {
            Debug.LogError("Graph not set");
            return false;
        }
        if (_pathfinder == null)
        {
            Debug.LogError("Pathfinder not set");
            return false;
        }
        return true;
    }

    ///<summary>Checks all coordinate inputs for pathfinding</summary>
    bool CheckPathfindingRequirements()
    {
        if (!_graph.IsWithinBounds(_startX, _startY) || !_graph.IsValidNode(_startX, _startY))
        {
            Debug.LogError("Start coordinates not valid");
            return false;
        }
        if (!_graph.IsWithinBounds(_tagetX, _targetY) || !_graph.IsValidNode(_tagetX, _targetY))
        {
            Debug.LogError("Target coordinates not valid");
            return false;
        }
        return true;
    }
}
