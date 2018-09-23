using UnityEngine;

///<summary>Class for node visualization</summary>
public class NodeView : MonoBehaviour
{
    [SerializeField] private GameObject _nodeGeometry;

    [SerializeField, Range(0, 0.3f)]
    private float _geometryBorder = 0.15f;

    private Node _node;

    ///<summary>Initializes node visuals</summary>
    public void Init(Node node)
    {
        gameObject.name = "Node (" + node.XIndex + "," + node.YIndex + ")";
        gameObject.transform.position = node.Position;
        _node = node;

        if (_nodeGeometry != null)
        {
            _nodeGeometry.transform.localScale = new Vector3(1f - _geometryBorder, 1f, 1f - _geometryBorder);
        }
    }

    ///<summary>Colors game object of node</summary>
    void ColorNode(Color color, GameObject go)
    {
        if (go != null)
        {
            Renderer goRenderer = go.GetComponent<Renderer>();

            if (goRenderer != null)
            {
                goRenderer.material.color = color;
            }
        }
    }

    ///<summary>Colors geometry of node</summary>
    public void ColorNode(Color color)
    {
        ColorNode(color, _nodeGeometry);
    }

}
