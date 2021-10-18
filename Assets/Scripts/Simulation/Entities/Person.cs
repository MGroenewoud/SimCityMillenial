using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Person : MonoBehaviour
{
    public float MoveSpeed;
    public int InventoryLimit = 10;

    public static List<Person> People = new List<Person>();

    public Point CurrentPosition;

    public Point Home;
    public Point Work;
    public Point Market;

    public PersonNeeds Needs;
    public PersonMovement Movement;
    public StateMachine StateMachine;
    public PersonInventory Inventory;

    public int WorkEfficiency = 0;

    private IPersonService PersonService;
    private IGridSearch GridSearch;

    void Awake()
    {
        Needs = GetComponent<PersonNeeds>();
        StateMachine = GetComponent<StateMachine>();
        Movement = GetComponent<PersonMovement>();
        Inventory = GetComponent<PersonInventory>();

        InitializeNeeds();
        InitializeStateMachine();

        CurrentPosition = GeneralUtility.MainGrid.LocalToCell(transform.position).AsPoint();
        
        People.Add(this);
    }

    [Inject]
    public void Construct(IPersonService _personService, IGridSearch _gridSearch)
    {
        PersonService = _personService;
        GridSearch = _gridSearch;
    }

    public void OnHomeChanged()
    {
        // calculate efficiency
        WorkEfficiency = PersonService.CalculateWorkEffiency(this);
    }

    private void InitializeNeeds()
    {
        var needs = new Dictionary<NeedType, Need>
        {
            { NeedType.Home, new Need(GameSettings.HomeDecayRate) },
            { NeedType.Shopping, new Need(GameSettings.FoodDecayRate) },
            { NeedType.Entertainment, new Need(GameSettings.EntertainmentDecayRate) },
        };

        GetComponent<PersonNeeds>().SetNeeds(needs);
    }

    private void InitializeStateMachine()
    {
        var states = new Dictionary<Type, PersonState>
        {
            { typeof(MovingState), new MovingState(this, GridSearch) },
            { typeof(RestingState), new RestingState(this) },
            { typeof(LumberjackState), new LumberjackState(this) },
            { typeof(FarmingState), new FarmingState(this) },
            { typeof(HerdingState), new HerdingState(this) },
            { typeof(WanderState), new WanderState(this, PersonService, GridSearch) },
        };
        GetComponent<StateMachine>().SetStates(states);
    }

    private void OnMouseUp()
    {
        Debug.Log("set focus");
        FindObjectOfType<DetailsPane>(true).SetFocus(new PersonDetails(this));
    }

}