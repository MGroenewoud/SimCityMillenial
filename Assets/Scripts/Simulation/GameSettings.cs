using UnityEngine;

public static class GameSettings
{
    [SerializeField]
    public static float HomeDecayRate = 0.2f;
    [SerializeField]
    public static float FoodDecayRate = 0.1f;
    [SerializeField]
    public static float EntertainmentDecayRate = 0.3f;
    [SerializeField]
    public static int CriticalNeedFactor = 1000;

    public static TileEntity[] RoadTiles = new TileEntity[] { TileEntity.Road };
    public static TileEntity[] WalkableTiles = new TileEntity[] { TileEntity.Grass, TileEntity.Road };
    public static TileEntity[] WorkBuildings = new TileEntity[] { TileEntity.LumberjackShack, TileEntity.HerdingShed, TileEntity.FarmShack };
}

public static class PersonSettings
{
    public static int WorkRange = 20;
    public static int MarketRange = 30;
    public static int FullWorkEfficiencyCutoff = 60;
    public static int NoWorkEfficiencyCutoff = 120;    
    public static float TryFindHomeDelay = 5f;
}
