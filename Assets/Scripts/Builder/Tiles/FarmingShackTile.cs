using UnityEngine;

public class FarmingShackTile : TileSelector
{
    public override void OnPlaced(Point placementPosition)
    {
        Debug.Log("Farming shack placed");

        var newFarmShack = new FarmingShack(placementPosition);

        var allFarmTiles = GridSearch.FindConnectingTilesOfType(placementPosition, TileEntity.FarmDirt);
        allFarmTiles.GetInnerTiles();

        newFarmShack.Farmland = allFarmTiles;

        SimulationCore.Instance.AllWorkplaces.Add(placementPosition, newFarmShack);
    }
}
