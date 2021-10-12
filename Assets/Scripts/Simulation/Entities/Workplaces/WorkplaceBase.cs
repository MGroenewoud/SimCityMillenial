using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class WorkPlaceBase
{
    public Point Location;
    public int Capacity;
    public HashSet<Person> Workers;
    public ResourceType ResourceProduced;
    /// <summary>
    /// Returns the productionrate per second.
    /// </summary>
    public float ProductionRate;
    /// <summary>
    /// Returns a number between 0 and 1.
    /// </summary>
    protected float CurrentProduction;

    public bool IsActive => Workers.Any();
    public bool HasCapacity => Workers.Count < Capacity;
    public abstract void UpdateProduction();
    public abstract void AssignWorker(Person person);
}