using System;
using System.Collections.Generic;
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
        if(!path.IsNullOrEmpty())
            CurrentMoveTarget = GeneralUtility.GetLocalCenterOfCell(PathToDestination.Pop());
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
}

public class NeedPathResult
{
    public Stack<Point> Path;
    public NeedType Need;
}