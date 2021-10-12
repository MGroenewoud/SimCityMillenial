using System.Text;

public class WorkplaceDetails : IFocus
{
    public WorkPlaceBase WorkplaceFocus;

    public WorkplaceDetails(WorkPlaceBase _workplace)
    {
        WorkplaceFocus = _workplace;
    }

    public string GetDetailsString()
    {
        var details = new StringBuilder();

        details.AppendLine(string.Format("IsActive: {0}", WorkplaceFocus.IsActive));
        details.AppendLine(string.Format("HasCapacity: {0}", WorkplaceFocus.HasCapacity));
        details.AppendLine(string.Format("ProductionRate: {0}", WorkplaceFocus.ProductionRate));

        return details.ToString();
    }

    public void Focus(WorkPlaceBase focus)
    {
        WorkplaceFocus = focus;
        GetDetailsString();
    }
}