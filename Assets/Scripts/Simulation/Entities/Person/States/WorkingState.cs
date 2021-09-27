using System;
using System.ComponentModel;
using UnityEngine;

public class WorkingState : PersonState
{
    private Point WorkBase;
    private Point WorkSpot;

    private WorkState State;
    private float HarvestRate = 10f;
    private float NextHarvestTick = 0f;

    public WorkingState(Person person) : base(person.gameObject)
    {
        _person = person;
    }

    public override void OnStateEnter()
    {
        WorkBase = _person.CurrentPosition;
        WorkSpot = SimulationCore.Instance.Grid.GetClosestBuildingOfType(WorkBase, TileEntity.Wood, 10).RandomItem();
        _person.Movement.GeneratePath(WorkBase, WorkSpot, GameSettings.WalkableTiles);
    }

    public override Type Tick()
    {
        switch (State)
        {
            case WorkState.MoveToResource:
                MoveToResource();
                break;
            case WorkState.Harvesting:
                DoingWork();
                break;
            case WorkState.MoveBackToBase:
                return ReturnResourcesToBase();
        }

        return typeof(WorkingState);
    }

    private void MoveToResource()
    {
        _person.Movement.MoveToDestination();
        var localWorkSpot = GeneralUtility.GetLocalCenterOfCell(WorkSpot);
        if (_person.gameObject.transform.position.IsSamePositionAs(localWorkSpot))
        {
            // Arrived at thing to harvest.
            State = WorkState.Harvesting;
            NextHarvestTick = Time.time + HarvestRate;
        }

    }

    private void DoingWork()
    {
        if(NextHarvestTick < Time.time)
        {
            _person.Inventory.AddItem(ItemType.Wood, 1);
            if (_person.Inventory.BagIsFull)
            {
                _person.Movement.GeneratePath(WorkSpot, WorkBase, GameSettings.WalkableTiles);
                State = WorkState.MoveBackToBase;
            }
        } 
        if (_person.Needs.HasCriticalNeed())
        {
            _person.Movement.GeneratePath(WorkSpot, WorkBase, GameSettings.WalkableTiles);
            State = WorkState.MoveBackToBase;
        }
    }

    private Type ReturnResourcesToBase()
    {
        _person.Movement.MoveToDestination();
        if (gameObject.transform.position == GeneralUtility.GetLocalCenterOfCell(WorkBase))
        {
            if (_person.Needs.HasCriticalNeed())
            {
                return typeof(RestingState);
            }
            _person.Movement.GeneratePath(WorkBase, WorkSpot);
        }
        return typeof(WorkingState);
    }

    public override string ToString()
    {
        return State.GetDescription();
    }

    private enum WorkState
    {
        [Description("Moving to resource")]
        MoveToResource,
        [Description("Harvesting")]
        Harvesting,
        [Description("Moving back to base")]
        MoveBackToBase,
    }
}


// on state entered => findwork => if found, set workbasepoint and worktile
// tick check if inventory is full or hascriticalneed
// if true, return to base and dump inventory
// 