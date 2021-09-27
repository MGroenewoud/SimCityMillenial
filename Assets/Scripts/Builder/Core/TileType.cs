using System;

public enum TileEntity
{
    // Terrain
    Grass = 0,
    [PlacementRule(TilePlacementRule.PlacedOnEmptyTile)]
    Road = 1,
    Ocean = 2,





    // Buildings
    [PlacementRule(TilePlacementRule.PlacedOnEmptyTile, TilePlacementRule.MustBePlacedNextToRoad)]
    Home = 1000,
    [PlacementRule(TilePlacementRule.PlacedOnEmptyTile, TilePlacementRule.MustBePlacedNextToRoad)]
    Shop = 1001,
    [PlacementRule(TilePlacementRule.PlacedOnEmptyTile, TilePlacementRule.MustBePlacedNextToRoad)]
    Entertainment = 1002,
    [PlacementRule(TilePlacementRule.PlacedOnEmptyTile, TilePlacementRule.MustBePlacedNextToRoad, TilePlacementRule.MustBePlacedCloseToForest)]
    LumberjackShack = 1003,





    // Raw Resources
    Forest = 2000,
}
