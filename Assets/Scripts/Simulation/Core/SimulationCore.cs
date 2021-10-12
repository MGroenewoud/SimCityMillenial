using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SimulationCore : MonoBehaviour
{
    public static SimulationCore Instance;
    public CityGrid Grid;

    public Dictionary<Point, Home> AllHomes = new Dictionary<Point, Home>();
    public Dictionary<Point, WorkPlaceBase> AllWorkplaces = new Dictionary<Point, WorkPlaceBase>();

    public Dictionary<ResourceType, int> Resources = new Dictionary<ResourceType, int>();

    [SerializeField]
    private int width, height;
    [SerializeField]
    private GameObject Dot;
    [SerializeField]
    private Person PersonPrefab;

    [Inject]
    private DiContainer Container;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Grid = new CityGrid(width, height);
        InitializeIsland();
        InitializeResources();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            Container.InstantiatePrefab(PersonPrefab, new GameObjectCreationParameters() { Position = GeneralUtility.GetMousePosition(), Rotation = Quaternion.identity });

            //var path = Pathfinder.GetPath(new Vector3Int(-3, 8, 0), new Vector3Int(4, 1, 0), GridLayer.BaseLayer);

            //ShowPath(path.Path);

            //var pos = GeneralUtility.GetMousePosition();
            //var cell = GeneralUtility.GetTilemap(GridLayer.BaseLayer).LocalToCell(pos);
            //Debug.Log(cell);
        }

        TickWorkplaces();
    }

    private void TickWorkplaces()
    {
        foreach (var workplace in AllWorkplaces.Values)
        {
            if (workplace.IsActive)
            {
                workplace.UpdateProduction();
            }
        }
    }

    private void InitializeResources()
    {
        Resources.Add(ResourceType.Wood, 10);
    }

    private void InitializeIsland()
    {
        GetComponent<MapGenerator>().GenerateIsland(width, 5);
    }

    internal void AddResource(ResourceType type, int amount)
    {
        if (Resources.ContainsKey(type))
            Resources[type] += amount;
        else
            Resources.Add(type, amount);
    }

    internal void DetractResources(ResourceType costType, int costAmount)
    {
        Resources[costType] -= costAmount;
    }
}
