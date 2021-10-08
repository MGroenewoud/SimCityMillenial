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
    
    public PersonNeeds Needs;
    public PersonMovement Movement;
    public StateMachine StateMachine;
    public PersonInventory Inventory;

    private IGridSearch GridSearch;

    [Inject]
    public void Construct(IGridSearch _gridSearch)
    {
        GridSearch = _gridSearch;
    }

    void Awake()
    {
        Needs = GetComponent<PersonNeeds>();
        StateMachine = GetComponent<StateMachine>();
        Movement = GetComponent<PersonMovement>();
        Inventory = GetComponent<PersonInventory>();

        InitializeNeeds();
        InitializeStateMachine(GridSearch);

        CurrentPosition = GeneralUtility.MainGrid.LocalToCell(transform.position).AsPoint();
        
        People.Add(this);
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

    private void InitializeStateMachine(IGridSearch _gridSearch)
    {
        var states = new Dictionary<Type, PersonState>
        {
            { typeof(MovingState), new MovingState(this, _gridSearch) },
            { typeof(RestingState), new RestingState(this, _gridSearch) },
            { typeof(WorkingState), new WorkingState(this, _gridSearch) },
        };
        GetComponent<StateMachine>().SetStates(states);
    }

    private void OnMouseUp()
    {
        FindObjectOfType<PersonDetails>(true).Focus(this);
    }

}