using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovingState : PersonState
{
    public Stack<Point> PathToDestination;
    public Vector3 CurrentMoveTarget;

    private NeedType DestinationNeedType;

    public MovingState(Person person) : base(person.gameObject)
    {
        _person = person;
    }

    public override Type Tick()
    {
        if (PathToDestination == null)
        {
            return typeof(RestingState);
        }

        var newState = MoveToDestination();

        return newState;
    }

    public override void OnStateEnter() {
        GeneratePath();
    }

    private Type MoveToDestination()
    {
        CurrentMoveTarget.z = -(1/2);
        _person.transform.position = Vector3.MoveTowards(_person.transform.position, CurrentMoveTarget, _person.MoveSpeed * Time.deltaTime);

        if (_person.transform.position == CurrentMoveTarget)
        {
            _person.CurrentPosition = GeneralUtility.MainGrid.LocalToCell(CurrentMoveTarget).AsPoint();
            // Arrived at next tile, grab next tile
            if (PathToDestination.Count != 0)
            {
                var nextCell = PathToDestination.Pop();
                if (SimulationCore.Instance.Grid[nextCell.X, nextCell.Y] == TileEntity.Road || PathToDestination.Count == 0 )
                {
                    CurrentMoveTarget = GeneralUtility.GetLocalCenterOfCell(nextCell);
                    FacePosition();
                }
                else
                {
                    Debug.Log("Path broken. Find new thing to do.");
                    return typeof(RestingState);
                }
            } else
            {
                DestinationReached();
                return typeof(RestingState);
            }
        }
        return typeof(MovingState);
    }

    private void FacePosition()
    {
        var delta = GeneralUtility.MainGrid.LocalToCell(CurrentMoveTarget) - GeneralUtility.MainGrid.LocalToCell(transform.position);
        var direction = Direction.All.FirstOrDefault(d => d.Item1 == delta);
        if(direction != null)
            transform.rotation = Quaternion.Euler(direction.Item2);
    }

    private void DestinationReached()
    {
        PathToDestination = null;
        CurrentMoveTarget = Vector3.negativeInfinity;
        _person.Needs.Needs[DestinationNeedType].Weight = 0;
    }

    private void GeneratePath()
    {
        var path = TryGetPath();
        PathToDestination = path;
        if (!path.IsNullOrEmpty())
        {
            CurrentMoveTarget = GeneralUtility.GetLocalCenterOfCell(PathToDestination.Pop());
            FacePosition();
        }
    }

    private NeedPathResult GetPath()
    {
        var result = new NeedPathResult();
        result.Need = DestinationNeedType;

        if (DestinationNeedType == NeedType.Home)
        {
            result.Path = GridSearch.AStarSearch(_person.CurrentPosition, _person.Home);
        }
        else
        {
            var buildingsInRange = SimulationCore.Instance.GetNearestBuildingOfType(DestinationNeedType, _person);

            result.Path = GridSearch.AStarSearch(_person.CurrentPosition, buildingsInRange);
        }
        return result;
    }

    private Stack<Point> TryGetPath()
    {
        var pathFound = false;
        var blockedNeeds = new HashSet<NeedType>();
        while (!pathFound)
        {
            DestinationNeedType = _person.Needs.GetBiggestNeed(blockedNeeds);
            var result = GetPath();
            if (!result.Path.IsNullOrEmpty())
                return result.Path;
            blockedNeeds.Add(DestinationNeedType);
            if (blockedNeeds.Count == _person.Needs.Needs.Count)
                break;
        }

        return null;
    }

    public override string ToString()
    {
        return "Moving";
    }
}

public class NeedPathResult
{
    public Stack<Point> Path;
    public NeedType Need;
}

public static class Direction
{
    private static Tuple<Vector3Int, Vector3> NorthEast { get => new Tuple<Vector3Int, Vector3>(new Vector3Int(1, 0, 0), new Vector3(-14, 55, -18)); }
    private static Tuple<Vector3Int, Vector3> SouthEast { get => new Tuple<Vector3Int, Vector3>(new Vector3Int(0, -1, 0), new Vector3(14, 126, -18)); }
    private static Tuple<Vector3Int, Vector3> SouthWest { get => new Tuple<Vector3Int, Vector3>(new Vector3Int(-1, 0, 0), new Vector3(18, -128, 18)); }
    private static Tuple<Vector3Int, Vector3> NorthWest { get => new Tuple<Vector3Int, Vector3>(new Vector3Int(0, 1, 0), new Vector3(-14, -55, 18)); }

    public static List<Tuple<Vector3Int, Vector3>> All = new List<Tuple<Vector3Int, Vector3>>() {NorthEast, SouthEast, SouthWest, NorthWest };
}