using UnityEngine;

///<summary>Static class for pathfinding math functions</summary>
public static class PathMath {

    ///<summary>Returns distance between source to target node</summary>
    public static float GetDistance(Node source, Node target)
    {
        int dx = Mathf.Abs(source.XIndex - target.XIndex);
        int dy = Mathf.Abs(source.YIndex - target.YIndex);

        int min = Mathf.Min(dx, dy);
        int max = Mathf.Max(dx, dy);

        int diagonalSteps = min;
        int straightSteps = max - min;

        //1.4 is cost of diagonal move
        return (1.4f * diagonalSteps + straightSteps);
    }

    ///<summary>Returns distance between source to target node calculated by heuristics type</summary>
    public static float GetHeuristicDistance(Node start, Node target, HeuriticsType heuriticsType)
    {
        float D1 = 1f; //straight move cost
        float D2 = 1.4f; //diagonal move cost

        int dx = Mathf.Abs(start.XIndex - target.XIndex);
        int dy = Mathf.Abs(start.YIndex - target.YIndex);
        float result;

        switch (heuriticsType)
        {
            case HeuriticsType.Diagonal:
                result = D1 * (dx + dy) + (D2 - 2f * D1) * Mathf.Min(dx, dy);
                break;

            case HeuriticsType.Manhattan:
                result = D1 * (dx + dy);
                break;

            case HeuriticsType.Euclidean:
                result = (D1 * Mathf.Sqrt(dx * dx + dy * dy));
                break;

            case HeuriticsType.Chebyshev:
                result = D1 * (dx + dy) + (D1 - 2f * D1) * Mathf.Min(dx, dy);
                break;

            default:
                result = 0;
                break;
        }
        return result;
    }
}
