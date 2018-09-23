using System.Collections.Generic;
using UnityEngine;

///<summary>Class for finding path from start to target. Utilizes four different search algorithms</summary>
public class Pathfinder : MonoBehaviour
{
    [SerializeField] private PathfindingType _pathfindingType;

    [Tooltip("Does not affect Breadth First and Dijkstra")]
    [SerializeField] private HeuriticsType _heuriticsType;

    [Tooltip("Max iterations for while loop - safety mesure")]
    [SerializeField] private int _maxIterations = 20000;

    private Node _startNode;
    private Node _targetNode;

    private List<Node> _path;
    private List<Node> _closedSet; //explored nodes
    private PriorityQueue<Node> _openSet; //opened nodes - currently in search

    ///<summary>Sets pathfinder to initial values for search</summary>
    private void Init(Node start, Node target)
    {
        _startNode = start;
        _targetNode = target;

        _startNode.CostSoFar = 0;

        _path = new List<Node>();
        _closedSet = new List<Node>();

        _openSet = new PriorityQueue<Node>();
        _openSet.Enqueue(_startNode);
    }

    ///<summary><para>Returns a list of nodes representing path. Out parameter is a list of explored nodes during search</para>
    ///<para>Path depends of pathfinding type and heuristics type used by Pathfinder.</para></summary>
    public List<Node> FindPath(Node start, Node target, out List<Node> exploredNodes)
    {
        Init(start, target);

        int iterations = 0;
        bool isComplete = false;

        while (!isComplete)
        {
            if (_openSet.Count > 0)
            {
                iterations++;

                Node currentNode = _openSet.Dequeue();

                if (!_closedSet.Contains(currentNode))
                { _closedSet.Add(currentNode); }

                for (int i = 0; i < currentNode.Neighbors.Count; i++)
                {
                    //pathfinding depending of type
                    switch (_pathfindingType)
                    {
                        case PathfindingType.BreadthFirstSearch:
                            SearchPathBreadthFirst(currentNode);
                            break;

                        case PathfindingType.Dijkstra:
                            SearchPathDijkstra(currentNode);
                            break;

                        case PathfindingType.GreedyBestFirst:
                            SearchPathGreedyBestFirst(currentNode);
                            break;

                        case PathfindingType.AStar:
                            SearchPathAStar(currentNode);
                            break;
                    }
                }

                //if target is found
                if (_openSet.Contains(_targetNode))
                {
                    _path = GetPathNodes(_targetNode);
                    isComplete = true;
                }
                //if reached max iterations for pathfinding
                else if (iterations == _maxIterations)
                {
                    Debug.LogError(_maxIterations.ToString() + " iterations reached. Breaking pathfinding...");
                    isComplete = true;
                }
            }
            else
            {
                isComplete = true;
            }
        }

        exploredNodes = _closedSet;
        return _path;
    }
    ///<summary><para>Searches path using Breadth First Algorithm. Scan each node in the first level starting from the leftmost node, moving towards the right</para>
    ///<para>Doesn't use heuristics for search. </para></summary>
    private void SearchPathBreadthFirst(Node node)
    {
        if (node != null)
        {
            for (int i = 0; i < node.Neighbors.Count; i++)
            {
                //if the current neighbor is non explored node and is non opened node
                if (!_closedSet.Contains(node.Neighbors[i]) && !_openSet.Contains(node.Neighbors[i]))
                {
                    float costToNeighbour = PathMath.GetDistance(node, node.Neighbors[i]);
                    float newCostSoFar = costToNeighbour + node.CostSoFar + node.Weight;

                    node.Neighbors[i].CostSoFar = newCostSoFar;
                    node.Neighbors[i].Previous = node;

                    node.Neighbors[i].Priority = _closedSet.Count;
                    _openSet.Enqueue(node.Neighbors[i]);
                }
            }
        }
    }

    ///<summary><para>Searches path using Dijkstra Algorithm. Finds the shortest path to target</para>
    ///<para>Doesn't use heuristics for search. Can be non-performant but finds the shortest path</para></summary>
    private void SearchPathDijkstra(Node node)
    {
        if (node != null)
        {
            for (int i = 0; i < node.Neighbors.Count; i++)
            {
                //if the current neighbor is non explored node
                if (!_closedSet.Contains(node.Neighbors[i]))
                {
                    float costToNeighbour = PathMath.GetDistance(node, node.Neighbors[i]);
                    float newCostSoFar = costToNeighbour + node.CostSoFar + node.Weight;

                    //re-route if a shorter path exists, positive inifinity is non opened node
                    if (float.IsPositiveInfinity(node.Neighbors[i].CostSoFar) || newCostSoFar < node.Neighbors[i].CostSoFar)
                    {
                        node.Neighbors[i].Previous = node;
                        node.Neighbors[i].CostSoFar = newCostSoFar;
                    }

                    //enqueue if this neighbor is not in currently opened set
                    if (!_openSet.Contains(node.Neighbors[i]))
                    {
                        node.Neighbors[i].Priority = node.Neighbors[i].CostSoFar;
                        _openSet.Enqueue(node.Neighbors[i]);
                    }
                }
            }
        }
    }

    ///<summary><para>Searches path using Greedy Best First Algorithm. Finds the path to target using heuristics</para>
    ///<para>Nodes are prioritized by heuristic of how far is node from target. Can be very performant but not the shortest</para></summary>
    private void SearchPathGreedyBestFirst(Node node)
    {
        if (node != null)
        {
            for (int i = 0; i < node.Neighbors.Count; i++)
            {
                //if the current neighbor is non explored node and is non opened node
                if (!_closedSet.Contains(node.Neighbors[i]) && !_openSet.Contains(node.Neighbors[i]))
                {

                    float costToNeighbor = PathMath.GetDistance(node, node.Neighbors[i]);
                    float newCostSoFar = costToNeighbor + node.CostSoFar + node.Weight;

                    node.Neighbors[i].CostSoFar = newCostSoFar;
                    node.Neighbors[i].Previous = node;

                    node.Neighbors[i].Priority = PathMath.GetHeuristicDistance(node.Neighbors[i], _targetNode, _heuriticsType);

                    _openSet.Enqueue(node.Neighbors[i]);
                }
            }
        }
    }

    ///<summary><para>Searches path using A* Algorithm. Finds the path to target using heuristics</para>
    ///<para>Nodes are prioritized by heuristic of how far is node from target and distance traveled from the start. Finds performant reliable path</para></summary>
    private void SearchPathAStar(Node node)
    {
        if (node != null)
        {
            for (int i = 0; i < node.Neighbors.Count; i++)
            {
                //if the current neighbor is non explored node
                if (!_closedSet.Contains(node.Neighbors[i]))
                {
                    float costToNeighbor = PathMath.GetDistance(node, node.Neighbors[i]);
                    float newCostSoFar = costToNeighbor + node.CostSoFar + node.Weight;

                    //re-route if a shorter path exists, positive inifinity is non opened node
                    if (float.IsPositiveInfinity(node.Neighbors[i].CostSoFar) || newCostSoFar < node.Neighbors[i].CostSoFar)
                    {
                        node.Neighbors[i].Previous = node;
                        node.Neighbors[i].CostSoFar = newCostSoFar;
                    }

                    //enqueue if this neighbor is not in currently opened set
                    if (!_openSet.Contains(node.Neighbors[i]))
                    {
                        float distanceToGoal = PathMath.GetHeuristicDistance(node.Neighbors[i], _targetNode, _heuriticsType);
                        node.Neighbors[i].Priority = node.Neighbors[i].CostSoFar + distanceToGoal;

                        _openSet.Enqueue(node.Neighbors[i]);
                    }
                }
            }
        }
    }

    ///<summary>Returns a list of nodes representing path. Input is end node</summary>
    private List<Node> GetPathNodes(Node endNode)
    {
        List<Node> path = new List<Node>();

        if (endNode == null)
        { return path; }

        path.Add(endNode);
        Node currentNode = endNode.Previous;

        while (currentNode != null)
        {
            path.Insert(0, currentNode);
            currentNode = currentNode.Previous;
        }

        return path;
    }
}
