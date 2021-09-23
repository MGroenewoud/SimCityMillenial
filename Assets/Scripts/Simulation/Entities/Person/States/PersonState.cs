using System;
using UnityEngine;

public abstract class PersonState
{
    protected GameObject gameObject;
    protected Transform transform;
    protected Person _person;

    public virtual void OnStateEnter() { }
    public abstract Type Tick();
    public abstract string ToString();

    public PersonState(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.transform = gameObject.transform;
    }
}