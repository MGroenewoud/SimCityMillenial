using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public interface IPersonService
{
    bool FindHomeForPerson(Person person);
}

public class PersonService : IPersonService
{
    private IGridSearch GridSearch;

    [Inject]
    public PersonService(IGridSearch _gridSearch)
    {
        GridSearch = _gridSearch;
    }

    public bool FindHomeForPerson(Person person)
    {
        // Find all empty homes
        var homesWithSpace = SimulationCore.Instance.AllHomes.Values.Where(h => h.HasSpaceForInhabitant);
        if (homesWithSpace.Any())
        {
            Debug.Log(string.Format("Found {0} homes with space", homesWithSpace.Count()));

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
                Debug.Log(string.Format("Found {0} homes with market in range", homesWithMarketInRange.Count()));

                // If there are any empty houses, with markets in range, look for work within range
                var homesWithMarketAndWorkInRange = new HashSet<Point>();
                foreach (var home in homesWithMarketInRange)
                {
                    var workPlaces = GridSearch.AStarSearchInRange(home, GameSettings.WorkBuildings, GameSettings.RoadTiles, PersonSettings.WorkRange);
                    if (workPlaces.Any())
                    {
                        homesWithMarketAndWorkInRange.Add(home);
                        workInRangeOfHome.Add(home, workPlaces);
                    }
                }

                if (homesWithMarketAndWorkInRange.Any())
                {
                    Debug.Log(string.Format("Found {0} homes with market AND work in range", homesWithMarketAndWorkInRange.Count()));
                    // We now have all homes with workplaces and markets within walking range

                    var pathsResult = CreateFullPathSearchResult(marketsInRangeOfHome, workInRangeOfHome);

                    var shortestPath = pathsResult.OrderBy(pr => pr.TotalSteps).First();

                    SimulationCore.Instance.AllHomes[shortestPath.Home].AddNewInhabitant(person);
                    person.Home = shortestPath.Home;
                    person.Work = shortestPath.Workplace;
                    person.Market = shortestPath.Market;

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