using System.Collections.Generic;

public class Home
{
    public Point Location;
    public bool HasSpaceForInhabitant => MaxOccupancy > Inhabitants.Count;

    private int MaxOccupancy = 2;
    private HashSet<Person> Inhabitants= new HashSet<Person>();

    public Home(Point location)
    {
        Location = location;
    }

    public void AddNewInhabitant(Person inhabitant)
    {
        Inhabitants.Add(inhabitant);
        inhabitant.Home = Location;
        inhabitant.OnHomeChanged();
    }

    public void RemoveInhabitant(Person inhabitant)
    {
        Inhabitants.Remove(inhabitant);
        inhabitant.OnHomeChanged();
    }
}