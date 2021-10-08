using System;
using System.Collections.Generic;
using UnityEngine;

public class WanderState : PersonState
{

    public WanderState(Person person) : base(person.gameObject)
    {
        _person = person;
    }

    public override Type Tick()
    {
        return typeof(WanderState);
    }


    public override string ToString()
    {
        return "Wandering";
    }
}