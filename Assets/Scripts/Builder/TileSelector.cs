using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
    [SerializeField]
    public TileBase Tile;
    [SerializeField]
    public GridLayer Layer;
    [SerializeField]
    public TileEntity Entity;

    public float delay;

    public void SelectThisTile()
    {
        delay = Time.time + 0.1f;
        Builder.Instance.Preview.SetTileSelector(this);
    }

    public void PlaceTile(Tilemap layer, Point position)
    {
        layer.SetTile(position.AsVector3Int(), Tile);
        SimulationCore.Instance.Grid[position.X, position.Y] = Entity;
    }

    public void PlaceTiles(Tilemap destinationLayer, HashSet<Point> previewTiles)
    {
        foreach(var tile in previewTiles)
        {
            PlaceTile(destinationLayer, tile);
        }
    }
}
