using System;

public class WorkingState : PersonState
{
    public WorkingState(Person person) : base(person.gameObject)
    {
        _person = person;
    }

    public override Type Tick()
    {
        if (_person.Needs.HasCriticalNeed())
            return typeof(RestingState);

        DoWork();

        return typeof(WorkingState);
    }

    private void DoWork()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return "Working";
    }
}