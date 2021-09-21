using System.Collections;
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

    public static bool IsNullOrEmpty(this IEnumerable<object> list)
    {
        return list == null || !list.Any();
    }
}
