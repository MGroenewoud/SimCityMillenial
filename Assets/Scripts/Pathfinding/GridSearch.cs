using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public interface IGridSearch
{
    public Stack<Point> AStarSearch(Point startPosition, Point endPosition, TileEntity[] walkableTiles = null);
    public List<GridSearchResult> AStarSearch(Point startPosition, HashSet<Point> pointsInRange);
    public List<GridSearchResult> AStarSearchInRange(Point from, TileEntity entityToSearchFor, TileEntity[] walkableTiles, int maxRange);
    public List<GridSearchResult> AStarSearchInRange(Point from, TileEntity[] entitiesToSearchFor, TileEntity[] walkableTiles, int maxRange);
    public Stack<Point> GeneratePath(Dictionary<Point, Point> parentMap, Point endState);
    public HashSet<Point> DijkstraGetEntitiesInRange(Point origin, TileEntity entity, int range);
    public HashSet<Point> FindConnectingTilesOfType(Point origin, TileEntity entity);
    public bool DijkstraHasEntitiesInRange(Point origin, TileEntity entity, int range);
}

/// <summary>
/// Source https://github.com/lordjesus/Packt-Introduction-to-graph-algorithms-for-game-developers
/// </summary>
public partial class GridSearch : IGridSearch
{
    public Stack<Point> AStarSearch(Point startPosition, Point endPosition, TileEntity[] walkableTiles = null)
    {
        var grid = SimulationCore.Instance.Grid;
        var path = new Stack<Point>();

        var positionsTocheck = new List<Point>();
        var costDictionary = new Dictionary<Point, float>();
        var priorityDictionary = new Dictionary<Point, float>();
        var parentsDictionary = new Dictionary<Point, Point>();

        positionsTocheck.Add(startPosition);
        priorityDictionary.Add(startPosition, 0);
        costDictionary.Add(startPosition, 0);
        parentsDictionary.Add(startPosition, null);

        if (walkableTiles == null)
        {
            walkableTiles = GameSettings.RoadTiles;
        }

        while (positionsTocheck.Count > 0)
        {
            var current = GetClosestVertex(positionsTocheck, priorityDictionary);
            positionsTocheck.Remove(current);
            if (current.Equals(endPosition))
            {
                path = GeneratePath(parentsDictionary, current);
                return path;
            }

            foreach (var neighbour in grid.GetAllAdjacentCells(current))
            {
                if (neighbour.Equals(endPosition) || walkableTiles.Contains(grid[neighbour.X, neighbour.Y]))
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
    public List<GridSearchResult> AStarSearchInRange(Point from, TileEntity entityToSearchFor, TileEntity[] pathTiles, int maxRange)
    {
        return AStarSearchInRange(from, new TileEntity[] { entityToSearchFor }, pathTiles, maxRange);
    }
    public List<GridSearchResult> AStarSearchInRange(Point from, TileEntity[] entitiesToSearchFor, TileEntity[] pathTiles, int maxRange)
    {
        var searchResults = new List<GridSearchResult>();
        var grid = SimulationCore.Instance.Grid;

        var visited = new HashSet<Point>() { from };
        var path = new Stack<Point>();
        path.Push(from);

        while (path.Any())
        {
            var currentNode = path.Peek();
            if (!visited.Contains(currentNode))
            {
                visited.Add(currentNode);
                // Check if any of the neighbours match the tile entities searched for.
                var neighbourMatches = grid.GetAdjacentCellsOfTypes(currentNode, entitiesToSearchFor).Except(visited);
                if (neighbourMatches.Any())
                {
                    // Some neighbours are the searched for entities, add them to the searchresult list
                    foreach (var match in neighbourMatches)
                    {
                        searchResults.Add(new GridSearchResult()
                        {
                            From = from,
                            To = match,
                            Steps = path.Count,
                            Path = new Stack<Point>(path),
                        });
                    }
                    visited.UnionWith(neighbourMatches);
                }
            }
            
            // Continue checking the path
            var pathNeighbours = grid.GetAdjacentCellsOfTypes(currentNode, pathTiles).Except(visited);
            if (pathNeighbours.Any() && path.Count < maxRange)
            {
                var nextNode = pathNeighbours.First();
                path.Push(nextNode);
            }
            else
            {
                path.Pop();
            }
        }


        return searchResults;
    }

    public HashSet<Point> FindConnectingTilesOfType(Point origin, TileEntity entity)
    {
        var connectingEntities = new HashSet<Point>();
        var tilesToCheck = new HashSet<Point>() { origin };

        while (tilesToCheck.Any())
        {
            var newTiles = new HashSet<Point>();
            foreach(var tile in tilesToCheck)
            {
                var newNeighbours = SimulationCore.Instance.Grid.GetAdjacentCellsOfType(tile, entity).Except(connectingEntities);
                newTiles.UnionWith(newNeighbours);
            }

            connectingEntities.UnionWith(newTiles);
            tilesToCheck = newTiles;
        }

        return connectingEntities;
    }

    public List<GridSearchResult> AStarSearch(Point startPosition, HashSet<Point> pointsInRange)
    {
        var results = new List<GridSearchResult>();

        foreach (var potentialMatch in pointsInRange)
        {
            var path = AStarSearch(startPosition, potentialMatch);
            if (!path.IsNullOrEmpty())
            {
                results.Add(new GridSearchResult()
                {
                    From = startPosition,
                    To = potentialMatch,
                    Path = path,
                    Steps = path.Count,
                });
            }
        }

        return results;
    }
    public Stack<Point> GeneratePath(Dictionary<Point, Point> parentMap, Point endState)
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

    private Point GetClosestVertex(List<Point> list, Dictionary<Point, float> distanceMap)
    {
        var candidate = list[0];
        foreach (var vertex in list)
        {
            if (distanceMap[vertex] < distanceMap[candidate])
            {
                candidate = vertex;
            }
        }
        return candidate;
    }
    private float ManhattanDiscance(Point endPos, Point point)
    {
        return Math.Abs(endPos.X - point.X) + Math.Abs(endPos.Y - point.Y);
    }

}
