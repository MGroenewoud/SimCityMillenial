using System;
using System.Collections.Generic;
using System.Linq;

public partial class GridSearch
{
    public static HashSet<Point> DijkstraGetEntitiesInRange(Point origin, TileEntity entity, int range)
    {
        var visited = new HashSet<Point>() { origin };
        var pointsOfInterest = new HashSet<Point>() { origin };
        var matches = new HashSet<Point>();

        int step = 0;
        while (pointsOfInterest.Any() && step < range)
        {
            step++;
            var nodesToCheck = new HashSet<Point>();
            foreach(var poi in pointsOfInterest)
            {
                var neighbours = SimulationCore.Instance.Grid.GetAllAdjacentCells(poi);
                nodesToCheck.UnionWith(neighbours);
            }
            nodesToCheck.ExceptWith(visited);

            matches.UnionWith(nodesToCheck.Where(n => SimulationCore.Instance.Grid[n.X, n.Y] == entity));
            
            pointsOfInterest = nodesToCheck;
            visited.UnionWith(nodesToCheck);
        }

        return matches;
    }

    public static bool DijkstraHasEntitiesInRange(Point origin, TileEntity entity, int range)
    {
        return DijkstraGetEntitiesInRange(origin, entity, range).Any();
    }

}
