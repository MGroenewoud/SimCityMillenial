using System.Text;

public class PersonDetails : IFocus
{
    private Person PersonFocus;

    public PersonDetails(Person _person)
    {
        PersonFocus = _person;
    }

    public string GetDetailsString()
    {
        var needs = new StringBuilder();

        needs.AppendLine("State: " + PersonFocus.StateMachine.ActiveState);

        foreach(var need in PersonFocus.Needs.Needs)
        {
            needs.AppendLine(string.Format("{0}: {1}", need.Key, need.Value.Weight));
        }

        return needs.ToString();
    }

    public void Focus(Person focus)
    {
        PersonFocus = focus;
        GetDetailsString();
    }
}