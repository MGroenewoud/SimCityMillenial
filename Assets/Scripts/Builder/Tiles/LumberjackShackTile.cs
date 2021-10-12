using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LumberjackShackTile : TileSelector
{
    public override void OnPlaced(Point placementPosition)
    {
        Debug.Log("lumberjack placed");

        SimulationCore.Instance.AllWorkplaces.Add(placementPosition, new LumberjackShack(placementPosition));
    }
}
