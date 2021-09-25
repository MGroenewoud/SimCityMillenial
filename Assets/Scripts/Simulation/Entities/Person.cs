using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Person : MonoBehaviour
{
    public static List<Person> People = new List<Person>();

    public Point CurrentPosition;
    public Point Home;
    public float MoveSpeed;

    public PersonNeeds Needs;
    public PersonMovement Movement;
    public StateMachine StateMachine;

    void Awake()
    {
        Needs = GetComponent<PersonNeeds>();
        StateMachine = GetComponent<StateMachine>();
        Movement = GetComponent<PersonMovement>();

        InitializeNeeds();
        InitializeStateMachine();

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

    private void InitializeStateMachine()
    {
        var states = new Dictionary<Type, PersonState>
        {
            { typeof(MovingState), new MovingState(this) },
            { typeof(RestingState), new RestingState(this) },
            { typeof(WorkingState), new WorkingState(this) },
        };
        GetComponent<StateMachine>().SetStates(states);
    }

    private void OnMouseUp()
    {
        FindObjectOfType<PersonDetails>(true).Focus(this);
    }

}