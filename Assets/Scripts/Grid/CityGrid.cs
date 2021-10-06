using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityGrid
{
    private TileEntity[,] _grid;
    
    public int Width { get { return _width; } }
    private int _height;
    public int Height { get { return _height; } }
    private int _width;

    public Dictionary<TileEntity, List<Point>> AllGridEntities;
    
    public CityGrid(int width, int height)
    {
        _width = width;
        _height = height;
        _grid = new TileEntity[width, height];
        AllGridEntities = new Dictionary<TileEntity, List<Point>>();

        foreach (TileEntity k in Enum.GetValues(typeof(TileEntity)))
        {
            AllGridEntities.Add(k, new List<Point>());
        }
    }

    // Adding index operator to our Grid class so that we can use grid[][] to access specific cell from our grid. 
    public TileEntity this[int i, int j]
    {
        get
        {
            return _grid[i, j];
        }
        set
        {
            foreach(var k in AllGridEntities.Keys)
            {
                AllGridEntities[k].Remove(new Point(i, j));
            }

            AllGridEntities[value].Add(new Point(i, j));
            
            _grid[i, j] = value;
        }
    }

    public float GetCostOfEnteringCell(Point cell)
    {
        return 1;
    }

    public List<Point> GetAdjacentCellsOfType(Point cell, TileEntity type)
    {
        var adjacentCells = GetAllAdjacentCells(cell);
        for (int i = adjacentCells.Count - 1; i >= 0; i--)
        {
            if (_grid[adjacentCells[i].X, adjacentCells[i].Y] != type)
            {
                adjacentCells.RemoveAt(i);
            }
        }
        return adjacentCells;
    }

    public List<Point> GetClosestBuildingsOfTypes(Point origin, TileEntity[] types, int range)
    {
        var buildingsOfType = new List<Point>();

        foreach(var type in types)
        {
            buildingsOfType.AddRange(GetClosestBuildingOfType(origin, type, range));
        }

        return buildingsOfType;
    }

    public List<Point> GetClosestBuildingOfType(Point origin, TileEntity type, int range)
    {
        var buildingsOfType = new List<Point>();

        foreach(var potential in AllGridEntities[type])
        {
            if(origin.DistanceFrom(potential) < range)
            {
                buildingsOfType.Add(potential);
            }
        }

        return buildingsOfType;
    }

    public bool HasResourceInRange(Point origin, TileEntity resource, int range)
    {
        return GetClosestBuildingOfType(origin, resource, range).Any();
    }

    public List<Point> GetAllAdjacentCells(Point cell)
    {
        var x = cell.X;
        var y = cell.Y;
        var adjacentCells = new List<Point>();
        
        if (x > 0)
            adjacentCells.Add(new Point(x - 1, y));
        if (x < _width - 1)
            adjacentCells.Add(new Point(x + 1, y));
        if (y > 0)
            adjacentCells.Add(new Point(x, y - 1));
        if (y < _height - 1)
            adjacentCells.Add(new Point(x, y + 1));

        return adjacentCells;
    }

    public Point[] GetAllPointsInbetween(Point pointA, Point pointB)
    {
        var allPoints = new List<Point>();

        var minX = Math.Min(pointA.X, pointB.X);
        var maxX = Math.Max(pointA.X, pointB.X);
        var minY = Math.Min(pointA.Y, pointB.Y);
        var maxY = Math.Max(pointA.Y, pointB.Y);

        Debug.Log("X: " + minX + " -> " + maxX);
        Debug.Log("Y: " + minY + " -> " + maxY);
        Debug.Log("-----------------");

        for (int x = minX; x < maxX+1; x++)
        {
            for (int y = minY; y < maxY+1; y++)
            {
                allPoints.Add(new Point(x,y));
            }
        }

        return allPoints.ToArray();
    }
}
