using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HomeTile : TileSelector
{
    public override void OnPlaced(Point placementLocation)
    {
        Debug.Log("home placed");

        SimulationCore.Instance.AllHomes.Add(placementLocation, new Home(placementLocation));

    }
}
