using UnityEngine;

public class FarmingShackTile : TileSelector
{
    public override void OnPlaced(Point placementPosition)
    {
        Debug.Log("Farming shack placed");

        SimulationCore.Instance.AllWorkplaces.Add(placementPosition, new FarmingShack(placementPosition));
    }
}
