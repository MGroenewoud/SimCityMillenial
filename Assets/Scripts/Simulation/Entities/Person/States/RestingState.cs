using System;
using System.Linq;
using UnityEngine;

public class RestingState : PersonState
{

    public RestingState(Person person) : base(person.gameObject)
    {
        _person = person;
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