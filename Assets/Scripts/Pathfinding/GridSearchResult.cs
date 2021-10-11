using System.Collections.Generic;

public partial class GridSearchResult
{
    public Point From;
    public Point To;
    public int Steps;
    public Stack<Point> Path;
    public bool PathFound
    {
        get
        {
            return !Path.IsNullOrEmpty();
        }
    }
}
