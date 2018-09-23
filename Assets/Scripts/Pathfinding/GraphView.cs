using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>Class for graph of nodes visualization</summary>
[RequireComponent(typeof(Graph))]
public class GraphView : MonoBehaviour
{
    public NodeView[,] NodesVisualized { get; private set; }

    [SerializeField] private GameObject _nodePrefab;

    [SerializeField] private Color _startColor = Color.green;
    [SerializeField] private Color _targetColor = Color.red;
    [SerializeField] private Color _searchColor = Color.magenta;
    [SerializeField] private Color _exploredColor = Color.gray;
    [SerializeField] private Color _pathColor = Color.cyan;

    [SerializeField] private float _searchVisualDelay = 0.1f;

    [SerializeField] int _borderSize = 5;

    ///<summary>Initializes visual graph</summary>
    public void Init(Graph graph)
    {
        if (graph == null)
        {
            Debug.LogWarning("No graph to initialize!");
            return;
        }

        NodesVisualized = new NodeView[graph.Width, graph.Height];

        GameObject parent = new GameObject();
        parent.name = "NodeGroup";
        parent.transform.position = Vector3.zero;

        foreach (Node node in graph.Nodes)
        {
            GameObject instance = Instantiate(_nodePrefab, Vector3.zero, _nodePrefab.transform.rotation);
            instance.transform.SetParent(parent.transform);

            NodeView nodeVisualized = instance.GetComponent<NodeView>();

            if (nodeVisualized != null)
            {
                nodeVisualized.Init(node);
                NodesVisualized[node.XIndex, node.YIndex] = nodeVisualized;

                Color color = MapData.GetColorByNodeType(node.NodeType);
                ColorNode(node, color);     
            }
        }

        SetupCamera(graph.Width, graph.Height, _borderSize);
    }

    ///<summary>Colors specific node to input color</summary>
    public void ColorNode(Node node, Color color)
    {
        if (node != null)
        {
            NodeView nodeView = NodesVisualized[node.XIndex, node.YIndex];
            Color newColor = color;

            if (nodeView != null)
            { nodeView.ColorNode(newColor); }
        }
    }

    ///<summary>Colors list of nodes to input color</summary>
    public void ColorNodes(List<Node> nodes, Color color)
    {
        foreach (Node node in nodes)
        {
            if (node != null)
            {
                NodeView nodeView = NodesVisualized[node.XIndex, node.YIndex];
                Color newColor = color;

                if (nodeView != null)
                {  nodeView.ColorNode(newColor); }
            }
        }
    }

    ///<summary>Visually displays full path with explored nodes</summary>
    public void VisualizeFullPath(List<Node> path, List<Node> exploredNodes)
    {
        if (path == null || path.Count == 0)
        {
            Debug.LogError("No path to display. Aborting...");
            return;
        }

        if (_searchVisualDelay > 0)
        { StartCoroutine(ColorFullPathRoutine(path, exploredNodes, _searchVisualDelay)); }
        else
        { ColorFullPath(path, exploredNodes); }
    }

    ///<summary>Colors full path with explored nodes with coloring delay</summary>
    private IEnumerator ColorFullPathRoutine(List<Node> path, List<Node> exploredNodes, float timeDelay)
    {
        Node start = path[0];
        Node target = path[path.Count - 1];

        ColorNode(start, _startColor);
        ColorNode(target, _targetColor);

        path.Remove(start);
        path.Remove(target);

        for (int i = 2; i < exploredNodes.Count; i++)
        {
            ColorNode(exploredNodes[i], _searchColor);
            ColorNode(exploredNodes[i - 1], _exploredColor);
            yield return new WaitForSeconds(timeDelay);
        }

        ColorNodes(path, _pathColor);
    }

    ///<summary>Colors full path with explored nodes</summary>
    private void ColorFullPath(List<Node> path, List<Node> exploredNodes)
    {
        ColorNodes(exploredNodes, _exploredColor);
        ColorNodes(path, _pathColor);

        Node start = path[0];
        Node target = path[path.Count - 1];
        ColorNode(start, _startColor);
        ColorNode(target, _targetColor);
    }

    ///<summary>Sets up camera</summary>
    public void SetupCamera(int width, int height, int borderSize)
    {
        Camera.main.transform.position = new Vector3((float)(width - 1) / 2f, (float)(height - 1) / 2f, -10f);

        float aspectRatio = (float)Screen.width / (float)Screen.height;
        float verticalSize = (float)height / 2f + (float)borderSize;
        float horizontalSize = ((float)width / 2f + (float)borderSize) / aspectRatio;

        Camera.main.orthographicSize = (verticalSize > horizontalSize) ? verticalSize : horizontalSize;
    }
}
