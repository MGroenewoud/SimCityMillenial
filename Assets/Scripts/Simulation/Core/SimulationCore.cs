using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimulationCore : MonoBehaviour
{
    public static SimulationCore Instance;
    public CityGrid Grid;

    public List<Home> AllHomes = new List<Home>();

    public Dictionary<ResourceType, int> Resources = new Dictionary<ResourceType, int>();

    [SerializeField]
    private int width, height;
    [SerializeField]
    private GameObject Dot;
    [SerializeField]
    private Person PersonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Grid = new CityGrid(width, height);
        InitializeIsland();
        InitializeResources();
    }

    private void InitializeResources()
    {
        Resources.Add(ResourceType.Wood, 10);
    }

    private void InitializeIsland()
    {
        GetComponent<MapGenerator>().GenerateIsland(width, 5);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonUp(0))
        //{
            //Instantiate(PersonPrefab, GeneralUtility.GetMousePosition(), Quaternion.identity);
            //var path = Pathfinder.GetPath(new Vector3Int(-3, 8, 0), new Vector3Int(4, 1, 0), GridLayer.BaseLayer);

            //ShowPath(path.Path);

            //var pos = GeneralUtility.GetMousePosition();
            //var cell = GeneralUtility.GetTilemap(GridLayer.BaseLayer).LocalToCell(pos);
            //Debug.Log(cell);
        //}

        CheckIfNewPersonNeedsToSpawn();
    }

    private void CheckIfNewPersonNeedsToSpawn()
    {
        var homesNotAtCapacity = AllHomes.Where(h => h.HasSpaceForInhabitant);
        if (homesNotAtCapacity.Any())
        {
            var home = homesNotAtCapacity.First();
            var newPerson = Instantiate(PersonPrefab, GeneralUtility.GetLocalCenterOfCell(home.Location), Quaternion.identity);
            home.AddNewInhabitant(newPerson);
        }
    }

    internal void DetractResources(ResourceType costType, int costAmount)
    {
        Resources[costType] -= costAmount;
        Debug.Log(Resources[costType]);
    }

    public List<Point> GetNearestBuildingOfType(NeedType need, Person person)
    {
        var type = TileEntity.Home;
        switch (need)
        {
            case NeedType.Shopping:
                type = TileEntity.Shop;
                break;
            case NeedType.Entertainment:
                type = TileEntity.Entertainment;
                break;
        }

        // Grab all relevant buildings in range X of home (paths)

        // Do i need paths? Can just grab by coordinates i think.
        //var paths = Pathfinder.GetBuildingsInRange(type, person.CurrentLocation.Home);

        // Of all buildings in range, make paths to all of them

        // Return building that has the shortest total path between current -> target, plus target -> home

        return Grid.GetClosestBuildingOfType(person.Home, type, 50);

    }

    public List<Point> GetNearestWorkBuilding(Person person)
    {
        return Grid.GetClosestBuildingsOfTypes(person.Home, GameSettings.WorkBuildings, 50);
    }
}
