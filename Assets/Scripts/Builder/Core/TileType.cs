using System;

public enum TileEntity
{
    // Terrain
    [TileTypeAttribute(TileType.Terrain)]
    Grass = 0,
    [TileTypeAttribute(TileType.Terrain)]
    Road = 1,





    // Buildings
    [TileTypeAttribute(TileType.Building)]
    Home = 1000,
    [TileTypeAttribute(TileType.Building)]
    Shop = 1001,
    [TileTypeAttribute(TileType.Building)]
    Entertainment = 1002,
}

public enum TileType
{
    Terrain = 1,
    Building = 2,

}

public class TileTypeAttribute : Attribute
{
    public TileType Type { get; set; }

    public TileTypeAttribute(TileType t)
    {
        Type = t;
    }
}