using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Person : MonoBehaviour
{
    public static List<Person> People = new List<Person>();

    public Point CurrentPosition;
    public Point Home;
    public float MoveSpeed = 1;

    public PersonNeeds Needs => GetComponent<PersonNeeds>();
    public StateMachine StateMachine => GetComponent<StateMachine>();

    // Start is called before the first frame update
    void Awake()
    {
        InitializeStateMachine();

        CurrentPosition = GeneralUtility.MainGrid.LocalToCell(transform.position).AsPoint();
        
        People.Add(this);
    }

    private void InitializeStateMachine()
    {
        var states = new Dictionary<Type, PersonState>
        {
            { typeof(MovingState), new MovingState(this) },
            { typeof(RestingState), new RestingState(this) },
        };
        GetComponent<StateMachine>().SetStates(states);
    }

    private void OnMouseUp()
    {
        Debug.Log("Focus on: " + CurrentPosition.ToString());
    }

}