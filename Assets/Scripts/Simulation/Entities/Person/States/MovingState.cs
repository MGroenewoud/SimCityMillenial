using System;
using System.Collections.Generic;

public class MovingState : PersonState
{
    private NeedType DestinationNeedType;

    private IGridSearch GridSearch;

    public MovingState(Person person, IGridSearch _gridSearch) : base(person.gameObject)
    {
        GridSearch = _gridSearch;
        _person = person;
    }

    public override void OnStateEnter()
    {
        DestinationNeedType = NeedType.None;
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
        if (DestinationNeedType != NeedType.None)
            _person.Needs.Needs[DestinationNeedType].Weight = 0;
        if (DestinationNeedType == NeedType.None && _person.Work != null)
        {
            return SimulationCore.Instance.AllWorkplaces[_person.Work].WorkState;
        }
        else
        {
            return typeof(RestingState);
        }
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
        }
        else
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
            return GridSearch.AStarSearch(_person.CurrentPosition, _person.Work);
        }
        else
        {
            return GridSearch.AStarSearch(_person.CurrentPosition, _person.Market);
        }
    }

    private Stack<Point> FindWorkPath()
    {
        return GridSearch.AStarSearch(_person.CurrentPosition, _person.Work);
    }

    public override string ToString()
    {
        return "Moving";
    }
}