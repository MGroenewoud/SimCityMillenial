using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovingState : PersonState
{
    private NeedType DestinationNeedType;

    public MovingState(Person person) : base(person.gameObject)
    {
        _person = person;
    }

    public override void OnStateEnter()
    {
        GeneratePath();
    }

    public override Type Tick()
    {
        if (_person.Movement.PathToDestination == null)
        {
            return typeof(RestingState);
        }

        var destinationReached = _person.Movement.MoveToDestination();
        if (destinationReached)
        {
            return DestinationReached();
        }
        else
        {
            return typeof(MovingState);
        }
    }

    private Type DestinationReached()
    {
        _person.Movement.PathToDestination = null;
        _person.Movement.CurrentMoveTarget = Vector3.negativeInfinity;
        if (DestinationNeedType != NeedType.None)
            _person.Needs.Needs[DestinationNeedType].Weight = 0;
        return DestinationNeedType == NeedType.None ? typeof(WorkingState) : typeof(RestingState);
    }

    private void GeneratePath()
    {
        var path = TryGetPath();
        
        if (!path.IsNullOrEmpty())
        {
            _person.Movement.PathToDestination = path;
            _person.Movement.CurrentMoveTarget = GeneralUtility.GetLocalCenterOfCell(_person.Movement.PathToDestination.Pop());
            _person.Movement.FacePosition(_person.Movement.CurrentMoveTarget);
        }
    }

    private Stack<Point> TryGetPath()
    {
        var pathFound = false;
        if (_person.Needs.HasCriticalNeed())
        {
            var blockedNeeds = new HashSet<NeedType>();
            while (!pathFound)
            {
                DestinationNeedType = _person.Needs.GetBiggestNeed(blockedNeeds);
                var result = GetPath();
                if (!result.IsNullOrEmpty())
                    return result;
                blockedNeeds.Add(DestinationNeedType);
                if (blockedNeeds.Count == _person.Needs.Needs.Count)
                    break;
            }
        } else
        {
            return FindWorkPath();
        }

        return null;
    }

    private Stack<Point> GetPath()
    {
        if (DestinationNeedType == NeedType.Home)
        {
            return GridSearch.AStarSearch(_person.CurrentPosition, _person.Home);
        }
        else if (DestinationNeedType == NeedType.None)
        {
            var buildingsInRange = SimulationCore.Instance.Grid.GetClosestBuildingOfType(_person.Home, TileEntity.LumberjackShack, 50);

            return GridSearch.AStarSearch(_person.CurrentPosition, buildingsInRange);
        }
        else
        {
            var buildingsInRange = SimulationCore.Instance.GetNearestBuildingOfType(DestinationNeedType, _person);

            return GridSearch.AStarSearch(_person.CurrentPosition, buildingsInRange);
        }
    }

    private Stack<Point> FindWorkPath()
    {
        var workInRange = SimulationCore.Instance.GetNearestWorkBuilding(_person);

        return GridSearch.AStarSearch(_person.CurrentPosition, workInRange);
    }

    public override string ToString()
    {
        return "Moving";
    }
}