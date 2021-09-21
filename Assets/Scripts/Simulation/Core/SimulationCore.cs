using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimulationCore : MonoBehaviour
{
    public static SimulationCore Instance;
    public CityGrid Grid;

    public int width, height;
    public GameObject Dot;
    public Person PersonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Grid = new CityGrid(width, height);
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
        if (Grid.AllGridEntities[TileEntity.Home].Count > Person.People.Count)
        {
            var home = Grid.AllGridEntities[TileEntity.Home].Last();
            var newPerson = Instantiate(PersonPrefab, GeneralUtility.GetLocalCenterOfCell(home), Quaternion.identity);
            newPerson.Home = home;
        }
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

        return Grid.GetPathToClosestBuildingsOfType(person.CurrentPosition, type, 50);

    }
}
