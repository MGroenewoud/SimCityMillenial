using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private Dictionary<Type, PersonState> _allStates;

    public PersonState CurrentState { get; private set; }
    public event Action<PersonState> OnStateChanged;
    public string ActiveState => CurrentState.ToString();

    public void SetStates(Dictionary<Type, PersonState> states)
    {
        _allStates = states;
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentState == null)
        {
            CurrentState = _allStates[typeof(RestingState)];
        }

        var nextState = CurrentState.Tick();

        if(nextState != null && nextState != CurrentState.GetType())
        {
            SwitchToNewState(nextState);
        }
    }

    private void SwitchToNewState(Type nextState)
    {
        CurrentState = _allStates[nextState];
        OnStateChanged?.Invoke(CurrentState);
        CurrentState.OnStateEnter();
    }
}
