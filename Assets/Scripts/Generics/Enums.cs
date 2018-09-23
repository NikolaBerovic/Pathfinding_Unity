public enum HeuriticsType
{
    Diagonal,
    Manhattan,
    Euclidean,
    Chebyshev
}

public enum PathfindingType
{
    BreadthFirstSearch,
    Dijkstra,
    GreedyBestFirst,
    AStar
}

public enum NodeType
{
    Open = 0,
    Blocked = 1,
    LightTerrain = 2,
    MediumTerrain = 3,
    HeavyTerrain = 4
}
