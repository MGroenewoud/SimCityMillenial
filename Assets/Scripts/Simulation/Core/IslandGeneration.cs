using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IslandGeneration : MonoBehaviour
{
    [SerializeField]
    private TileBase Ocean;

    [SerializeField]
    private TileBase Grass;

    [SerializeField]
    private int IslandOffset;

    private Tilemap Map;

    public void GenerateIsland()
    {
        Initialize();
        // Lay base of ocean
        LayOceanBase();
        GenerateGenericLandMass();
    }

    private void Initialize()
    {
        Map = GeneralUtility.GetTilemap(GridLayer.BaseLayer);
    }

    private void LayOceanBase()
    {
        for (int x = 0; x < SimulationCore.Instance.width; x++)
        {
            for (int y = 0; y < SimulationCore.Instance.width; y++)
            {
                Map.SetTile(new Point(x, y).AsVector3Int(), Ocean);
            }
        }
    }

    private void GenerateGenericLandMass()
    {
        for (int x = IslandOffset; x < SimulationCore.Instance.width - IslandOffset; x++)
        {
            for (int y = IslandOffset; y < SimulationCore.Instance.width - IslandOffset; y++)
            {
                Map.SetTile(new Point(x, y).AsVector3Int(), Grass);
            }
        }
    }
}
