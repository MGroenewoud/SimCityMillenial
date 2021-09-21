//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.Tilemaps;

//public static class Pathfinder
//{
//    public static Tilemap Map;

//    public static PathfinderResult GetPath(Vector3Int start, Vector3Int destination, GridLayer layer)
//    {
//        int step = 0;
//        bool destionationFound = false;
//        Map = GeneralUtility.GetTilemap(layer);
//        var visitedTiles = new HashSet<TileNode>() { new TileNode(start) { Step = step } };

//        while (visitedTiles.Where(t => t.Step == step).Any() && !destionationFound)
//        {
//            var possibles = visitedTiles.Where(t => t.Step == step);
//            var allNewNeighbours = new HashSet<Vector3Int>();
//            foreach (var tile in possibles)
//            {
//                allNewNeighbours.UnionWith(tile.GetNeighbours().Select(n => n.Location));
//            }
//            var visitedLocations = visitedTiles.Select(vt => vt.Location);
//            // Filter out already visited nodes
//            allNewNeighbours.RemoveWhere(n => visitedLocations.Contains(n));
//            if (allNewNeighbours.Contains(destination))
//            {
//                destionationFound = true;
//            }
//            else
//            {
//                // Filter out tiles that aren't roads
//                allNewNeighbours.RemoveWhere(n => !Map.GetTile(n).name.Contains("Road"));
//            }
//            step++;
//            foreach (var newNeighbour in allNewNeighbours)
//            {
//                visitedTiles.Add(new TileNode(newNeighbour) { Step = step });
//            }
//        }

//        var result = new PathfinderResult();

//        if (destionationFound)
//        {
//            result.Path = CreatePath(visitedTiles, start, destination);
//            result.Steps = step;
//        }

//        return result;
//    }

//    private static Stack<Vector3Int> CreatePath(HashSet<TileNode> visitedTiles, Vector3Int from, Vector3Int to)
//    {
//        var destinationNode = visitedTiles.First(vt => vt.Location == to);
//        var path = new Stack<Vector3Int>();
//        path.Push(destinationNode.Location);

//        var travelNode = destinationNode;
//        while(travelNode.Location != from)
//        {
//            travelNode = visitedTiles.First(vt => vt.GetNeighbours().Any(n => n.Location == travelNode.Location) && vt.Step <= travelNode.Step -1);
//            path.Push(travelNode.Location);
//        }

//        return path;
//    }
//}

//public class PathfinderResult
//{
//    public Stack<Vector3Int> Path { get; set; }
//    public int Steps { get; set; }
//}

//public class TileNode
//{
//    public Vector3Int Location { get; set; }
//    public int Step { get; set; }

//    public TileNode(Vector3Int loc)
//    {
//        Location = loc;
//    }

//    public HashSet<TileNode> GetNeighbours()
//    {
//        var neighbours = new HashSet<TileNode>();
//        //Top
//        neighbours.Add(GetNeighbour(0, 1));
//        //Right
//        neighbours.Add(GetNeighbour(1, 0));
//        //Bottom
//        neighbours.Add(GetNeighbour(0, -1));
//        //Left
//        neighbours.Add(GetNeighbour(-1, 0));

//        return neighbours;
//    }

//    private TileNode GetNeighbour(int xMod, int yMod)
//    {
//        var neighbourLocation = new Vector3Int(Location.x + xMod, Location.y + yMod, Location.z);

//        var neighbourNode = new TileNode(neighbourLocation)
//        {
//            Step = Step + 1,
//        };

//        return neighbourNode;
//    }
//}