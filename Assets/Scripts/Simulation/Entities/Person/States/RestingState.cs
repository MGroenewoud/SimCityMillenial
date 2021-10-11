using System;
using Zenject;

public class RestingState : PersonState
{
    [Inject]
    private IGridSearch GridSearch;

    public RestingState(Person person) : base(person.gameObject)
    {
        _person = person;
    }

    public override Type Tick()
    {
        if(_person.Market != null && _person.Home != null && _person.Work != null)
            return typeof(MovingState);
        return typeof(WanderState);
    }

    public override string ToString()
    {
        return "Resting";
    }
}