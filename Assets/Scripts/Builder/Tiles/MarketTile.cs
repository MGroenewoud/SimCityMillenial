using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MarketTile : TileSelector
{
    public override void OnPlaced(Point placementPosition)
    {
        Debug.Log("market placed");

        // Spawn 4 people

    }
}
