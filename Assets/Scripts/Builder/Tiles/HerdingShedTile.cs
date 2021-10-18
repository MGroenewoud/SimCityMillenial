using System.Linq;
using UnityEngine;

public class HerdingShedTile : TileSelector
{
    public override void OnPlaced(Point placementPosition)
    {
        var newShed = new HerdingShed(placementPosition);
        var allFenceTiles = GridSearch.FindConnectingTilesOfType(placementPosition, TileEntity.Fence);

        var minX = allFenceTiles.Min(t => t.X);
        var minY = allFenceTiles.Min(t => t.Y);
        var maxX = allFenceTiles.Max(t => t.X);
        var maxY = allFenceTiles.Max(t => t.Y);

        newShed.GrazingTiles = GeneralUtility.GetCellsInsideRectangularShape(new Point(minX, minY), new Point(maxX, maxY));

        SimulationCore.Instance.AllWorkplaces.Add(placementPosition, newShed);

        // Now remove the part of the fence facing the shed, so player can walk through it
        // TODO: replace this with a gate in the fence
        var neighbouringFence = GridSearch.DijkstraGetEntitiesInRange(placementPosition, TileEntity.Fence, 1).First();
        SimulationCore.Instance.Grid[neighbouringFence.X, neighbouringFence.Y] = TileEntity.Grass;
    }
}
