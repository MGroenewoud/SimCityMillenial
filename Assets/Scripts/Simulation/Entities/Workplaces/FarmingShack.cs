using System;
using System.Collections.Generic;
using UnityEngine;

public class FarmingShack : WorkPlaceBase
{
    public FarmingShack(Point location)
    {
        Location = location;
        Workers = new HashSet<Person>();
        ResourceProduced = ResourceType.Food;
        Capacity = 4;
        ProductionRate = 1;
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