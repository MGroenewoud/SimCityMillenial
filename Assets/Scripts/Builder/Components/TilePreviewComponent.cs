using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePreviewComponent : MonoBehaviour
{
    private TileSelector CurrentTileSelector;
    private Tilemap DestinationLayer;

    private bool _previewMode = false;
    private HashSet<Point> _previewTiles = new HashSet<Point>();

    public void SetTileSelector(TileSelector selector)
    {
        DestinationLayer = GeneralUtility.GetTilemap(selector.Layer);
        ClearTileSelector();
        CurrentTileSelector = selector;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentTileSelector != null)
        {
            UpdatePreview();

            if (Input.GetMouseButtonDown(0))
            {
                if(CurrentTileSelector.Entity == TileEntity.Road)
                    _previewMode = true;
            }
            else if (Input.GetMouseButtonUp(0) && CurrentTileSelector.delay < Time.time)
            {
                var mousePos = GeneralUtility.GetMousePosition();
                var position = DestinationLayer.WorldToCell(mousePos);
                if(_previewMode)
                    CurrentTileSelector.PlaceTiles(DestinationLayer, _previewTiles);
                else
                    CurrentTileSelector.PlaceTile(DestinationLayer, position.AsPoint());
                _previewMode = false;
                DestinationLayer.ClearAllEditorPreviewTiles();
                _previewTiles.Clear();

            }
            else if (Input.GetKey(KeyCode.Escape))
            {
                ClearTileSelector();
            }
        }
    }

    private void ClearTileSelector()
    {
        CurrentTileSelector = null;
        _previewMode = false;
        DestinationLayer.ClearAllEditorPreviewTiles();
        _previewTiles.Clear();
    }

    private void UpdatePreview()
    {
        var cellPos = DestinationLayer.WorldToCell(GeneralUtility.GetMousePosition());
        if (!DestinationLayer.HasEditorPreviewTile(cellPos) || _previewMode)
        {
            if (_previewMode)
                _previewTiles.Add(cellPos.AsPoint());
            else
                DestinationLayer.ClearAllEditorPreviewTiles();

            DestinationLayer.SetEditorPreviewTile(cellPos, CurrentTileSelector.GetPreviewTile());
        }
    }
}
