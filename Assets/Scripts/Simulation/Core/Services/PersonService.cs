using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public interface IPersonService
{
    bool FindHomeForPerson(Person person);
    int CalculateWorkEffiency(Person person);
}

public class PersonService : IPersonService
{
    private IGridSearch GridSearch;

    [Inject]
    public PersonService(IGridSearch _gridSearch)
    {
        GridSearch = _gridSearch;
    }

    public int CalculateWorkEffiency(Person person)
    {
        var totalPath = person.Movement.HomeToMarketPath.Count + person.Movement.HomeToWorkPath.Count + person.Movement.WorkToMarketPath.Count;
        if (totalPath <= PersonSettings.FullWorkEfficiencyCutoff)
            return 100;
        if (totalPath >= PersonSettings.NoWorkEfficiencyCutoff)
            return 0;

        var efficiencyRange = PersonSettings.NoWorkEfficiencyCutoff - PersonSettings.FullWorkEfficiencyCutoff;

        return 100 - ((totalPath - PersonSettings.FullWorkEfficiencyCutoff) / efficiencyRange);
    }

    public bool FindHomeForPerson(Person person)
    {
        // Find all empty homes
        var homesWithSpace = SimulationCore.Instance.AllHomes.Values.Where(h => h.HasSpaceForInhabitant);
        if (homesWithSpace.Any())
        {
            // Debug.Log(string.Format("Found {0} homes with space", homesWithSpace.Count()));
            // Filter homes by homes that have access to a market within range
            var homesWithMarketInRange = new HashSet<Point>();

            var marketsInRangeOfHome = new Dictionary<Point, List<GridSearchResult>>();
            var workInRangeOfHome = new Dictionary<Point, List<GridSearchResult>>();
            var allWorkToMarketPaths = new Dictionary<Tuple<Point, Point>, List<GridSearchResult>>();

            foreach (var home in homesWithSpace)
            {
                var markets = GridSearch.AStarSearchInRange(home.Location, TileEntity.Market, GameSettings.RoadTiles, PersonSettings.MarketRange);
                if (markets.Any())
                {
                    homesWithMarketInRange.Add(home.Location);
                    marketsInRangeOfHome.Add(home.Location, markets);
                }
            }

            if (homesWithMarketInRange.Any())
            {
                // Debug.Log(string.Format("Found {0} homes with market in range", homesWithMarketInRange.Count()));
                // If there are any empty houses, with markets in range, look for work within range
                var homesWithMarketAndWorkInRange = new HashSet<Point>();
                var workplacesWithCapacity = SimulationCore.Instance.AllWorkplaces.Values.Where(wp => wp.HasCapacity).Select(wp => wp.Location).ToList();
                foreach (var home in homesWithMarketInRange)
                {
                    var workPlaces = GridSearch.AStarSearchInRange(home, GameSettings.WorkBuildings, GameSettings.RoadTiles, PersonSettings.WorkRange);
                    workPlaces = workPlaces.Where(wp => workplacesWithCapacity.Contains(wp.To)).ToList();
                    if (workPlaces.Any())
                    {
                        homesWithMarketAndWorkInRange.Add(home);
                        workInRangeOfHome.Add(home, workPlaces);
                    }
                }

                if (homesWithMarketAndWorkInRange.Any())
                {
                    // Debug.Log(string.Format("Found {0} homes with market AND work in range", homesWithMarketAndWorkInRange.Count()));
                    // We now have all homes with workplaces and markets within walking range

                    var pathsResult = CreateFullPathSearchResult(marketsInRangeOfHome, workInRangeOfHome);

                    var shortestPath = pathsResult.OrderBy(pr => pr.TotalSteps).First();

                    person.Work = shortestPath.Workplace;
                    person.Market = shortestPath.Market;

                    person.Movement.SetPaths(shortestPath.HomeToMarket, shortestPath.HomeToWorkplace, shortestPath.WorkplaceToMarket);
                    // Set home last, as it triggers a few functions that use the paths and positions set above.
                    SimulationCore.Instance.AllHomes[shortestPath.Home].AddNewInhabitant(person);
                    SimulationCore.Instance.AllWorkplaces[shortestPath.Workplace].AssignWorker(person);
                    
                    return true;
                }
            }
        }

        return false;
    }

    private List<HomeWorkMarketSearchResult> CreateFullPathSearchResult(Dictionary<Point, List<GridSearchResult>> homeToMarketList, Dictionary<Point, List<GridSearchResult>> homeToWorkList)
    {
        var result = new List<HomeWorkMarketSearchResult>();
        var allHomes = homeToWorkList.Keys;

        foreach(var home in allHomes)
        {
            foreach(var workResult in homeToWorkList[home])
            {
                var workToMarketList = GridSearch.AStarSearchInRange(workResult.To, TileEntity.Market, GameSettings.RoadTiles, PersonSettings.MarketRange + PersonSettings.WorkRange);

                foreach (var marketResult in homeToMarketList[home])
                {
                    var workToMarketPath = workToMarketList.Where(wtm => wtm.From.Equals(workResult.To) && wtm.To.Equals(marketResult.To)).ToList();
                    if (workToMarketPath.Any())
                    {
                        result.Add(new HomeWorkMarketSearchResult()
                        {
                            Home = home,
                            Market = marketResult.To,
                            Workplace = workResult.To,
                            HomeToMarket = homeToMarketList[home].First(r => r.To.Equals(marketResult.To)),
                            HomeToWorkplace = workResult,
                            WorkplaceToMarket = workToMarketPath.First(),
                        });
                    }
                }
            }
        }

        return result;
    }


}

public class HomeWorkMarketSearchResult
{
    public Point Home;
    public Point Market;
    public Point Workplace;
    public GridSearchResult HomeToMarket;
    public GridSearchResult HomeToWorkplace;
    public GridSearchResult WorkplaceToMarket;
    public int TotalSteps
    {
        get
        {
            return HomeToMarket.Steps + HomeToWorkplace.Steps + WorkplaceToMarket.Steps;
        }
    }
}