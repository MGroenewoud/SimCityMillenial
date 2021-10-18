using System;
using System.Collections.Generic;
using UnityEngine;

public class HerdingShed : WorkPlaceBase
{
    public HashSet<Point> GrazingTiles;

    public HerdingShed(Point location)
    {
        Location = location;
        Workers = new HashSet<Person>();
        ResourceProduced = ResourceType.Wool;
        Capacity = 2;
        ProductionRate = 0.5f;
        WorkState = typeof(HerdingState);
    }

    public override void UpdateProduction()
    {
        if (IsActive)
        {
            float production = 0;

            foreach (var worker in Workers)
            {
                production += ProductionRate * (worker.WorkEfficiency / 100) * Time.deltaTime;
            }

            CurrentProduction += production;

            if (CurrentProduction >= 1)
            {
                var amountProduced = (int)Math.Floor(CurrentProduction);
                SimulationCore.Instance.AddResource(ResourceProduced, amountProduced);

                CurrentProduction -= amountProduced;

                Debug.Log("Produced " + amountProduced);
            }
        }
    }

    public override void AssignWorker(Person person)
    {
        Workers.Add(person);
    }
}