using UnityEngine;

public class HerdingShedTile : TileSelector
{
    public override void OnPlaced(Point placementPosition)
    {
        Debug.Log("herdingshed placed");

        SimulationCore.Instance.AllWorkplaces.Add(placementPosition, new HerdingShed(placementPosition));
    }
}
