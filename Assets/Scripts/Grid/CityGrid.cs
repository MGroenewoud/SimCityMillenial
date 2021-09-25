using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityGrid
{

    private TileEntity[,] _grid;
    private int _width;
    public int Width { get { return _width; } }
    private int _height;
    public int Height { get { return _height; } }

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
        List<Point> adjacentCells = GetAllAdjacentCells(cell);
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
            buildingsOfType.AddRange(GetClosestBuildingsOfType(origin, type, range));
        }

        return buildingsOfType;
    }

    public List<Point> GetClosestBuildingsOfType(Point origin, TileEntity type, int range)
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
        return GetClosestBuildingsOfType(origin, resource, range).Any();
    }

    public List<Point> GetAllAdjacentCells(Point cell)
    {
        var x = cell.X;
        var y = cell.Y;
        List<Point> adjacentCells = new List<Point>();
        
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
}
