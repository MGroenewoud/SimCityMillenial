using System;

public enum TileEntity
{
    // Terrain
    Grass = 0,
    [PlacementRule(TilePlacementRule.PlacedOnEmptyTile)]
    Road = 1,
    Ocean = 2,
    [PlacementRule(TilePlacementRule.PlacedOnEmptyTile, TilePlacementRule.NotNextToOtherFarmDirt)]
    FarmDirt = 3,





    // Buildings
    [PlacementRule(TilePlacementRule.PlacedOnEmptyTile, TilePlacementRule.MustBePlacedNextToRoad)]
    Home = 1000,
    [PlacementRule(TilePlacementRule.PlacedOnEmptyTile, TilePlacementRule.MustBePlacedNextToRoad)]
    Shop = 1001,
    [PlacementRule(TilePlacementRule.PlacedOnEmptyTile, TilePlacementRule.MustBePlacedNextToRoad)]
    Entertainment = 1002,
    [PlacementRule(TilePlacementRule.PlacedOnEmptyTile, TilePlacementRule.MustBePlacedNextToRoad, TilePlacementRule.MustBePlacedCloseToForest)]
    LumberjackShack = 1003,
    [PlacementRule(TilePlacementRule.PlacedOnEmptyTile, TilePlacementRule.MustBePlacedNextToRoad, TilePlacementRule.MustBePlacedNextToFence)]
    HerdingShed = 1004,
    [PlacementRule(TilePlacementRule.PlacedOnEmptyTile, TilePlacementRule.MustBePlacedNextToRoad, TilePlacementRule.MustBePlacedNextToFarmDirt)]
    FarmShack = 1005,
    [PlacementRule(TilePlacementRule.PlacedOnEmptyTile, TilePlacementRule.MustBePlacedNextToRoad)]
    Market = 1006,




    // Raw Resources
    Forest = 2000,



    //Obstacles
    [PlacementRule(TilePlacementRule.PlacedOnEmptyTile, TilePlacementRule.NotNextToOtherFence)]
    Fence = 3000,
}
