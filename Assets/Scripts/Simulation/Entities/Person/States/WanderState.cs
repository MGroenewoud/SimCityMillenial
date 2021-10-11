using System;
using UnityEngine;

public class WanderState : MovingState
{
    private IPersonService PersonService;

    public WanderState(Person person, IPersonService _personService, IGridSearch _gridSearch) : base(person, _gridSearch)
    {
        _person = person;
        PersonService = _personService;
    }

    public override Type Tick()
    {
        if (_person.Movement.PathToDestination == null)
        {
            // Check if person has a house
            if (_person.Home != null || PersonService.FindHomeForPerson(_person))
                return typeof(MovingState);
            else
                GenerateRandomWanderPath();
        }

        base.Tick();

        return typeof(WanderState);
    }

    private void GenerateRandomWanderPath()
    {
        var wanderTile = SimulationCore.Instance.Grid.GetAdjacentCellsOfType(_person.CurrentPosition, TileEntity.Road).RandomItem();
        _person.Movement.GeneratePath(_person.CurrentPosition, wanderTile);
    }

    public override string ToString()
    {
        return "Wandering";
    }
}