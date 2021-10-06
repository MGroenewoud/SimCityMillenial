using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
    [SerializeField]
    public GridLayer Layer;
    [SerializeField]
    public TileEntity Entity;
    [SerializeField]
    public PreviewModeType PreviewMode;

    [SerializeField]
    private TileBase[] Tile;
    
    public float delay;

    public void PlaceTile(Tilemap layer, Point position)
    {
        var canBePlaced = TilePlacementRuleProcessor.CanBePlaced(Entity, position);
        if (canBePlaced)
        {
            layer.SetTile(position.AsVector3Int(), GrabRandomTile());
            SimulationCore.Instance.Grid[position.X, position.Y] = Entity;
        } else
        {
            Debug.Log("Can't be placed there.");
        }
    }

    public void PlaceTiles(Tilemap destinationLayer, Point[] previewTiles)
    {
        foreach(var tile in previewTiles)
        {
            PlaceTile(destinationLayer, tile);
        }
    }

    public TileBase GetPreviewTile()
    {
        return Tile.First();
    }

    private void OnMouseUp()
    {
        delay = Time.time + 0.1f;
        Builder.Instance.Preview.SetTileSelector(this);
    }

    private TileBase GrabRandomTile()
    {
        if (Tile.Length == 1)
            return Tile.First();

        var random = UnityEngine.Random.Range(0, Tile.Length);
        return Tile[random];
        
    }
}
