using System;
using System.Linq;
using UnityEngine;

public class RestingState : PersonState
{
    private IGridSearch GridSearch;

    public RestingState(Person person, IGridSearch _gridSearch) : base(person.gameObject)
    {
        _person = person;
        GridSearch = _gridSearch;
    }

    public override Type Tick()
    {
        //if (_person.Needs.HasCriticalNeed())
        return typeof(MovingState);
        //return typeof(RestingState);
    }

    public override string ToString()
    {
        return "Resting";
    }
}