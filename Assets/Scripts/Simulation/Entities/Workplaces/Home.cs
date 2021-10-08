using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Home
{
    public Point Location;
    public bool HasSpaceForInhabitant => MaxOccupancy > Inhabitants.Count;

    private int MaxOccupancy = 2;
    private HashSet<Person> Inhabitants;

    public Home(Point location)
    {
        Location = location;
    }

    public void AddNewInhabitant(Person inhabitant)
    {
        Inhabitants.Add(inhabitant);
        inhabitant.Home = Location;
    }

    public void RemoveInhabitant(Person inhabitant)
    {
        Inhabitants.Remove(inhabitant);
        // Probably do something here that finds the removed inhabitant a new home.
    }
}