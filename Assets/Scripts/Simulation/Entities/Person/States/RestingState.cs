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
        return typeof(MovingState);
    }
}