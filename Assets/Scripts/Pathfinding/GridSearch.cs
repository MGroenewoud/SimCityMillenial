using System;
using System.Collections.Generic;

/// <summary>
/// Source https://github.com/lordjesus/Packt-Introduction-to-graph-algorithms-for-game-developers
/// </summary>
public class GridSearch
{

    public struct SearchResult
    {
        public List<Point> Path { get; set; }
    }

    public static Stack<Point> AStarSearch(Point startPosition, Point endPosition)
    {
        var grid = SimulationCore.Instance.Grid;
        Stack<Point> path = new Stack<Point>();

        List<Point> positionsTocheck = new List<Point>();
        Dictionary<Point, float> costDictionary = new Dictionary<Point, float>();
        Dictionary<Point, float> priorityDictionary = new Dictionary<Point, float>();
        Dictionary<Point, Point> parentsDictionary = new Dictionary<Point, Point>();

        positionsTocheck.Add(startPosition);
        priorityDictionary.Add(startPosition, 0);
        costDictionary.Add(startPosition, 0);
        parentsDictionary.Add(startPosition, null);

        while (positionsTocheck.Count > 0)
        {
            Point current = GetClosestVertex(positionsTocheck, priorityDictionary);
            positionsTocheck.Remove(current);
            if (current.Equals(endPosition))
            {
                path = GeneratePath(parentsDictionary, current);
                return path;
            }

            foreach (Point neighbour in grid.GetAllAdjacentCells(current))
            {
                if (neighbour.Equals(endPosition) || grid[neighbour.X, neighbour.Y] == TileEntity.Road)
                {
                    float newCost = costDictionary[current] + grid.GetCostOfEnteringCell(neighbour);
                    if (!costDictionary.ContainsKey(neighbour) || newCost < costDictionary[neighbour])
                    {
                        costDictionary[neighbour] = newCost;

                        float priority = newCost + ManhattanDiscance(endPosition, neighbour);
                        positionsTocheck.Add(neighbour);
                        priorityDictionary[neighbour] = priority;

                        parentsDictionary[neighbour] = current;
                    }
                }
            }
        }
        return path;
    }

    public static Stack<Point> AStarSearch(Point startPosition, List<Point> pointsInRange)
    {
        var closest = new Stack<Point>();

        foreach(var potentialMatch in pointsInRange)
        {
            var path = AStarSearch(startPosition, potentialMatch);
            if ((closest.Count == 0 || path.Count < closest.Count) && path.Count != 0)
                closest = path;
        }

        return closest;
    }

    private static Point GetClosestVertex(List<Point> list, Dictionary<Point, float> distanceMap)
    {
        Point candidate = list[0];
        foreach (Point vertex in list)
        {
            if (distanceMap[vertex] < distanceMap[candidate])
            {
                candidate = vertex;
            }
        }
        return candidate;
    }

    private static float ManhattanDiscance(Point endPos, Point point)
    {
        return Math.Abs(endPos.X - point.X) + Math.Abs(endPos.Y - point.Y);
    }

    public static Stack<Point> GeneratePath(Dictionary<Point, Point> parentMap, Point endState)
    {
        Stack<Point> path = new Stack<Point>();
        Point parent = endState;
        while (parent != null && parentMap.ContainsKey(parent))
        {
            path.Push(parent);
            parent = parentMap[parent];
        }
        return path;
    }
}
