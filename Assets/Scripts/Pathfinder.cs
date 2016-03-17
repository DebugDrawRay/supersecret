using UnityEngine;
using System.Collections.Generic;

public class Pathfinder
{
    public static List<Vector2> FindPath(Vector2 start, Vector2 goal, Vector2 gridSize)
    {
        float gridXSize = gridSize.x;
        float gridYSize = gridSize.y;

        Dictionary<Vector2, Vector2> visitedPoints = new Dictionary<Vector2, Vector2>();
        List<Vector2> searchPoints = new List<Vector2>();
        List<Vector2> newPath = new List<Vector2>();

        searchPoints.Add(start);

        while (searchPoints.Count > 0)
        {
            Vector2 currentPoint = searchPoints[0];

            if (currentPoint == goal)
            {
                Vector2 lastPoint = currentPoint;
                while(lastPoint != start)
                {
                    newPath.Add(lastPoint);
                    Vector2 pathPoint = visitedPoints[lastPoint];
                    lastPoint = pathPoint;
                }
                newPath.Add(start);
                newPath.Reverse();
                break;
            }

            List<Vector2> newNeighbors = Neighbors(currentPoint, gridXSize, gridYSize);
            for (int i = 0; i < newNeighbors.Count; ++i)
            {
                Vector2 newPoint = newNeighbors[i];
                if(!searchPoints.Contains(newPoint) && !visitedPoints.ContainsKey(newPoint))
                {
                    searchPoints.Add(newPoint);
                    visitedPoints.Add(newPoint, currentPoint);
                }
            }
            searchPoints.RemoveAt(0);
        }
        return newPath;
    }

    private static List<Vector2> Neighbors(Vector2 point, float gridXSize, float gridYSize)
    {
        List<Vector2> points = new List<Vector2>();
        List<Vector2> approvedPoints = new List<Vector2>();

        points.Add(new Vector2(point.x, point.y + 1));
        points.Add(new Vector2(point.x, point.y - 1));
        points.Add(new Vector2(point.x + 1, point.y));
        points.Add(new Vector2(point.x - 1, point.y));

        for(int i = 0; i < points.Count; ++i)
        {
            Vector2 currentPoint = points[i];

            if(currentPoint.x < 0 ||
               currentPoint.y < 0 ||
               currentPoint.x >= gridXSize ||
               currentPoint.y >= gridYSize)
            {
                continue;
            }
            approvedPoints.Add(currentPoint);
        }
        return approvedPoints;
    }
}
