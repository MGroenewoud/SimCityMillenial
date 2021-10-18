using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{
    public static Point AsPoint(this Vector3Int v)
    {
        return new Point(v.x, v.y);
    }

    public static int DistanceFrom(this Point origin, Point destination)
    {
        return Mathf.Abs(origin.X - destination.X) + Mathf.Abs(origin.Y - destination.Y);
    }

    public static Vector3 AsVector3(this Point p, int z = 0)
    {
        return new Vector3(p.X, p.Y, z);
    }

    public static Vector3Int AsVector3Int(this Point p, int z = 0)
    {
        return new Vector3Int(p.X, p.Y, z);
    }

    public static bool IsSamePositionAs(this Vector3 v1, Vector3 v2)
    {
        return v1.x == v2.x && v1.y == v2.y;
    }

    public static bool IsNullOrEmpty(this IEnumerable<object> list)
    {
        return list == null || !list.Any();
    }

    public static T RandomItem<T>(this IEnumerable<T> list)
    {
        var randomNumber = Random.Range(0, list.Count());
        return list.ToList()[randomNumber];
    }
}

public static class EntityExtensions
{
    public static TilePlacementRule[] GetRulesFromEntity(this TileEntity enumValue)
    {
        //Look for DescriptionAttributes on the enum field
        object[] attr = enumValue.GetType().GetField(enumValue.ToString())
            .GetCustomAttributes(typeof(PlacementRuleAttribute), false);

        if (attr.Length > 0) // a DescriptionAttribute exists; use it
            return ((PlacementRuleAttribute)attr[0]).Types;
        return new TilePlacementRule[] { };
    }

    /// <summary>
    /// This function assumes the set of tiles given, form a rectangle. This function will then trim the outer layer of that shape from the set.
    /// </summary>
    public static void GetInnerTiles(this HashSet<Point> tiles)
    {
        var minX = tiles.Min(t => t.X);
        var minY = tiles.Min(t => t.Y);
        var maxX = tiles.Max(t => t.X);
        var maxY = tiles.Max(t => t.Y);
        
        tiles.RemoveWhere(t => t.X == minX || t.X == maxX || t.Y == minY || t.Y == maxY);
    }
}