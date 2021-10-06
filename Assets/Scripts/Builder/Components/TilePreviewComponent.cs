using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePreviewComponent : MonoBehaviour
{
    private TileSelector CurrentTileSelector;
    private Tilemap DestinationLayer;

    private Point[] _previewTiles = new Point[] { };
    private Vector3Int MousePosition;
    private Point OriginPosition;

    private bool _previewMode;

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
                if (!_previewMode)
                {
                    OriginPosition = GeneralUtility.GetGridLocationOfMouse();
                    MousePosition = OriginPosition.AsVector3Int();
                    _previewTiles = new Point[] { MousePosition.AsPoint() };
                    _previewMode = true;
                }
            }
            else if (Input.GetMouseButtonUp(0) && CurrentTileSelector.delay < Time.time && _previewMode)
            {
                CurrentTileSelector.PlaceTiles(DestinationLayer, _previewTiles);
                _previewMode = false;
                DestinationLayer.ClearAllEditorPreviewTiles();
                _previewTiles = new Point[] { };

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
        _previewTiles = new Point[] { };
    }

    private void UpdatePreview()
    {
        var cellPos = DestinationLayer.WorldToCell(GeneralUtility.GetMousePosition());
        MousePosition = cellPos;
        if (_previewMode)
        {
            if (cellPos.AsPoint() != OriginPosition)
            {
                DestinationLayer.ClearAllEditorPreviewTiles();
                switch (CurrentTileSelector.PreviewMode)
                {
                    case PreviewModeType.Line:
                        ShowLineTilePreview();
                        break;
                    case PreviewModeType.SquareOutline:
                        ShowSquareOutlinePreview();
                        break;
                    case PreviewModeType.SquareFull:
                        ShowFullSquarePreview();
                        break;
                }
            }
        }
        else
        {
            ShowSingleTilePreview();
        }
    }

    private void ShowFullSquarePreview()
    {
        _previewTiles = SimulationCore.Instance.Grid.GetAllPointsInbetween(OriginPosition, MousePosition.AsPoint());
        SetPreviewTiles(_previewTiles);
    }

    private void ShowSquareOutlinePreview()
    {
        var path = GridSearch.AStarSearch(OriginPosition, MousePosition.AsPoint(), GameSettings.WalkableTiles);
        var pathReverse = GridSearch.AStarSearch(MousePosition.AsPoint(), OriginPosition, GameSettings.WalkableTiles);

        while(pathReverse.Count != 0)
        {
            path.Push(pathReverse.Pop()); 
        }

        _previewTiles = path.ToArray();
        SetPreviewTiles(path);
    }

    private void ShowLineTilePreview()
    {
        _previewTiles = GridSearch.AStarSearch(OriginPosition, MousePosition.AsPoint(), GameSettings.WalkableTiles).ToArray();
        SetPreviewTiles(_previewTiles);
    }

    private void ShowSingleTilePreview()
    {
        if (!DestinationLayer.HasEditorPreviewTile(MousePosition))
        {
            DestinationLayer.ClearAllEditorPreviewTiles();

            DestinationLayer.SetEditorPreviewTile(MousePosition, CurrentTileSelector.GetPreviewTile());
        }
    }

    private void SetPreviewTiles(IEnumerable<Point> points)
    {
        foreach (var p in points)
        {
            DestinationLayer.SetEditorPreviewTile(p.AsVector3Int(), CurrentTileSelector.GetPreviewTile());
        }
    }
}

public enum PreviewModeType
{
    Single,
    Line,
    SquareOutline,
    SquareFull,
}