using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private TileBase Grass;
    [SerializeField]
    private TileBase Water;
    [SerializeField]
    private TileBase[] Forest;

    float subtractMin = float.MaxValue;
    float subtractMax = float.MinValue;

    private void CreateRandomGrid(float[,] map, int size)
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                var tileToSet = GetTile(map[x, y]);
                SetTile(x, y, tileToSet);
            }
        }
    }

    internal void GenerateIsland(int gridsize, int scale)
    {
        var heightMap = GenerateNoise(gridsize, scale);
        var islandShape = GenerateSquareGradient(gridsize, gridsize);
        var resultMap = SubtractNoiseMaps(heightMap, islandShape);


        CreateRandomGrid(resultMap, gridsize);
    }

    private void SetTile(int x, int y, TileBase tileToSet)
    {
        var layer = GeneralUtility.GetTilemap(GridLayer.BaseLayer);
        layer.SetTile(new Point(x,y).AsVector3Int(), tileToSet);
        SimulationCore.Instance.Grid[x, y] = TileEntity.Grass;
    }

    private TileBase GetTile(float v)
    {
        var tileType = Grass;

        if (v < 0.2)
            tileType = Water;
        else if (v > 0.8)
        {
            var i = UnityEngine.Random.Range(0, Forest.Length);
            tileType = Forest[i];
        }

        return tileType;
    }

    public float[,] GenerateNoise(int sampleSize,  float scale)
    {
        float[,] noiseMap = new float[sampleSize, sampleSize];

        for (int x = 0; x < sampleSize; x++)
        {
            for (int y = 0; y < sampleSize; y++)
            {
                float posX = (float)x / scale;
                float posY = (float)y / scale;
                try
                {
                    noiseMap[x, y] = Mathf.PerlinNoise(posX, posY);
                }catch(Exception e)
                {
                    Debug.Log("nope");
                    break;
                }
            }
        }

        return noiseMap;
    }

    // https://github.com/matrikasaTR/Unity_2D_Procedural_Islands/blob/main/Assets/PerlinNoiseGenerator.cs
    private float[,] GenerateSquareGradient(int xSize, int ySize)
    {
        int halfWidth = xSize / 2;
        int halfHeight = ySize / 2;

        float[,] gradient = new float[xSize, ySize];

        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                int x = i;
                int y = j;

                float colorValue;

                x = x > halfWidth ? xSize - x : x;
                y = y > halfHeight ? ySize - y : y;

                int smaller = x < y ? x : y;
                colorValue = smaller / (float)halfWidth;

                colorValue = (1 - colorValue);
                colorValue *= colorValue * colorValue;
                gradient[i, j] = colorValue;
            }
        }

        return gradient;
    }

    private float[,] SubtractNoiseMaps(float[,] a, float[,] b)
    {
        //returns a - b
        if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1))
        {
            Debug.Log("Noise maps to subtract must have the same amount of rows and columns.");
        }

        int xSize = a.GetLength(0);
        int ySize = a.GetLength(1);
        float[,] subtractedNoiseMap = new float[xSize, ySize];

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                float s = a[x, y] - b[x, y];

                subtractedNoiseMap[x, y] = s;

                subtractMin = s < subtractMin ? s : subtractMin;
                subtractMax = s > subtractMax ? s : subtractMax;

            }
        }


        return subtractedNoiseMap;
    }
}