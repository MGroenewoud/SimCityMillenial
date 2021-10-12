using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class TileSelector : MonoBehaviour
{
    [SerializeField]
    public GridLayer Layer;
    [SerializeField]
    public TileEntity Entity;
    [SerializeField]
    public PreviewModeType PreviewMode;

    public float delay;

    [SerializeField]
    public TileBase[] Tile;

    [SerializeField]
    private ResourceType CostType;
    [SerializeField]
    private int CostAmount = 0;

    private ITilePlacementRuleProcessor RulesProcessor;

    [Inject]
    public void Construct(ITilePlacementRuleProcessor _rulesProcessor)
    {
        RulesProcessor = _rulesProcessor;
    }

    public void PlaceTile(Tilemap layer, Point position, int id = 0)
    {
        if (CanPlace(position))
        {
            PlaceTileOnMap(layer, position, id);
        }
        else
        {
            Debug.Log("Can't be placed there.");
        }
    }

    public bool CanAfford()
    {
        return SimulationCore.Instance.Resources[CostType] >= CostAmount;

    }

    public void PlaceTiles(Tilemap destinationLayer, Point[] previewTiles)
    {
        int id = 0;

        foreach(var tile in previewTiles)
        {
            if (!CanPlace(tile))
                return;
        }

        foreach(var tile in previewTiles)
        {
            PlaceTileOnMap(destinationLayer, tile, id);
            id++;
        }

        // Detract cost of placement from resources
        SimulationCore.Instance.DetractResources(CostType, CostAmount);
    }

    public TileBase GetPreviewTile(int id = 0)
    {
        return Tile[id];
    }

    public virtual bool CanPlace(Point p) {
        return RulesProcessor.CanBePlaced(Entity, p);
    }

    public virtual void OnPlaced(Point placementLocation) { }

    private void OnMouseUp()
    {
        delay = Time.time + 0.1f;
        Builder.Instance.Preview.SetTileSelector(this);
    }

    private void PlaceTileOnMap(Tilemap layer, Point position, int id)
    {
        if (PreviewMode == PreviewModeType.TwoByTwo)
        {
            layer.SetTile(position.AsVector3Int(), Tile[id]);
        }
        else
        {
            layer.SetTile(position.AsVector3Int(), GrabRandomTile());
        }

        SimulationCore.Instance.Grid[position.X, position.Y] = Entity;

        OnPlaced(position);
    }

    private TileBase GrabRandomTile()
    {
        if (Tile.Length == 1)
            return Tile.First();

        var random = UnityEngine.Random.Range(0, Tile.Length);
        return Tile[random];
        
    }
}
