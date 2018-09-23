using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

///<summary>Class for loading map from texture file</summary>
public class MapData : MonoBehaviour
{
    [SerializeField] private Texture2D _textureMap;
    [SerializeField] private string _resourcePath = "Mapdata";

    //terrain colors
    [SerializeField] private Color32 _openColor = Color.white;
    [SerializeField] private Color32 _blockedColor = Color.black;
    [SerializeField] private Color32 _lightTerrainColor = new Color32(5, 255, 0, 255);
    [SerializeField] private Color32 _mediumTerrainColor = new Color32(255, 140, 0, 255);
    [SerializeField] private Color32 _heavyTerrainColor = new Color32(140, 45, 0, 255);

    //weights
    [SerializeField] private float _lightTerrainWeight = 0.5f;
    [SerializeField] private float _mediumTerrainWeight = 1f;
    [SerializeField] private float _heavyTerrainWeight = 1.5f;

    static Dictionary<NodeType, Color32> terrainColorDict = new Dictionary<NodeType, Color32>();
    static Dictionary<NodeType, float> terrainWeightDict = new Dictionary<NodeType, float>();

    private int _width;
    private int _height;

    void Awake()
    {
        SetupDicitionaries();
    }

    void Start()
    {
        //automatically load any texture map or text file named for the current scene
        string levelName = SceneManager.GetActiveScene().name;

        if (_textureMap == null)
        {
            _textureMap = Resources.Load(_resourcePath + "/" + levelName) as Texture2D;
        }
    }

    ///<summary>Converts texture to list of strings</summary>
    public List<string> GetMapFromTexture(Texture2D texture)
    {
        List<string> lines = new List<string>();

        if (texture != null)
        {
            for (int y = 0; y < texture.height; y++)
            {
                string newLine = "";

                for (int x = 0; x < texture.width; x++)
                {
                    Color pixelColor = texture.GetPixel(x, y);

                    if (terrainColorDict.ContainsValue(pixelColor))
                    {
                        NodeType nodeType = terrainColorDict.FirstOrDefault(t => t.Value == pixelColor).Key;
                        int nodeTypeNum = (int)nodeType;
                        newLine += nodeTypeNum;
                    }
                    else
                    {
                        Debug.LogError("Color on map doesn't exist, change map image color or color variable!");
                        newLine += '0';
                    }
                }
                lines.Add(newLine);
            }
        }

        return lines;
    }

    ///<summary>Set the height and width of our map from a List of strings</summary>
    public void SetDimensions(List<string> textLines)
    {
        _height = textLines.Count;

        foreach (string line in textLines)
        {
            if (line.Length > _width)
            {
                _width = line.Length;
            }
        }
    }

    ///<summary>Returns an array of integers representing our map converted from texture file</summary>
    public int[,] GetLoadedMap()
    {
        if (_textureMap == null)
        {
            Debug.LogError("No texture map set! Aborting map creation...");
        }

        List<string> lines = new List<string>();
        lines = GetMapFromTexture(_textureMap);

        SetDimensions(lines);

        int[,] map = new int[_width, _height];
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (lines[y].Length > x)
                {
                    map[x, y] = (int)Char.GetNumericValue(lines[y][x]);
                }
            }
        }
        return map;
    }

    ///<summary>Sets up node type dictionary values</summary>
    void SetupDicitionaries()
    {
        terrainColorDict.Add(NodeType.Open, _openColor);
        terrainColorDict.Add(NodeType.Blocked, _blockedColor);
        terrainColorDict.Add(NodeType.LightTerrain, _lightTerrainColor);
        terrainColorDict.Add(NodeType.MediumTerrain, _mediumTerrainColor);
        terrainColorDict.Add(NodeType.HeavyTerrain, _heavyTerrainColor);

        terrainWeightDict.Add(NodeType.Open, 0f);
        terrainWeightDict.Add(NodeType.Blocked, -1f);
        terrainWeightDict.Add(NodeType.LightTerrain, _lightTerrainWeight);
        terrainWeightDict.Add(NodeType.MediumTerrain, _mediumTerrainWeight);
        terrainWeightDict.Add(NodeType.HeavyTerrain, _heavyTerrainWeight);
    }

    ///<summary>Returns color concerning node type</summary>
    public static Color GetColorByNodeType(NodeType nodeType)
    {
        if (terrainColorDict.ContainsKey(nodeType))
        {
            return terrainColorDict[nodeType];
        }
        return Color.white;
    }

    ///<summary>Returns weight concerning node type</summary>
    public static float GetWeightByNodeType(NodeType nodeType)
    {
        if (terrainWeightDict.ContainsKey(nodeType))
        {
            return terrainWeightDict[nodeType];
        }
        return 0f;
    }
}
