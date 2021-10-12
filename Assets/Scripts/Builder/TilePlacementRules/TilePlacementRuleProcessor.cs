using System;
using System.Collections.Generic;
using System.Linq;

public interface ITilePlacementRuleProcessor
{
    public bool CanBePlaced(TileEntity entity, Point target);
}

public class TilePlacementRuleProcessor : ITilePlacementRuleProcessor
{
    public Dictionary<TilePlacementRule, Func<bool>> RuleLibrary;

    private bool IsInitialized = false;

    private Point Target;
    private IGridSearch GridSearch;

    public TilePlacementRuleProcessor(IGridSearch _gridSearch)
    {
        GridSearch = _gridSearch;
    }

    public bool CanBePlaced(TileEntity entity, Point target)
    {
        Target = target;
        if (!IsInitialized)
            Initialize();
        return PassesRules(entity.GetRulesFromEntity());
    }

    private void Initialize()
    {
        RuleLibrary = new Dictionary<TilePlacementRule, Func<bool>>() {
            { TilePlacementRule.MustBePlacedNextToRoad, () => PlacedAdjacentToTileType(TileEntity.Road) },
            { TilePlacementRule.PlacedOnEmptyTile, () => MustBePlacedOn(TileEntity.Grass) },
            { TilePlacementRule.MustBePlacedCloseToForest, () => MustBePlacedCloseTo(TileEntity.Forest) },
            { TilePlacementRule.MustBePlacedNextToFence, () => PlacedAdjacentToTileType(TileEntity.Fence) },
            { TilePlacementRule.MustBePlacedNextToFarmDirt, () => PlacedAdjacentToTileType(TileEntity.FarmDirt) },
            { TilePlacementRule.NotNextToOtherFence, () => !PlacedAdjacentToTileType(TileEntity.Fence) },
            { TilePlacementRule.NotNextToOtherFarmDirt, () => !PlacedAdjacentToTileType(TileEntity.FarmDirt) },
        };
    }

    private bool PassesRules(TilePlacementRule[] ruleTypes)
    {
        foreach(var ruleType in ruleTypes)
        {
            if (!RuleLibrary[ruleType]())
            {
                return false;
            }
        }

        return true;
    }

    private bool PlacedAdjacentToTileType(TileEntity type)
    {
        return SimulationCore.Instance.Grid.GetAdjacentCellsOfType(Target, type).Any();
    }

    private bool MustBePlacedOn(TileEntity type)
    {
        return SimulationCore.Instance.Grid[Target.X, Target.Y] == type;
    }

    private bool MustBePlacedCloseTo(TileEntity type)
    {
        return GridSearch.DijkstraHasEntitiesInRange(Target, type, 4);
    }
}

public enum TilePlacementRule
{
    MustBePlacedNextToRoad,
    PlacedOnEmptyTile,
    MustBePlacedCloseToForest,
    MustBePlacedNextToFence,
    MustBePlacedNextToFarmDirt,
    NotNextToOtherFence,
    NotNextToOtherFarmDirt,
}

public class PlacementRuleAttribute : Attribute
{
    public TilePlacementRule[] Types { get; set; }

    public PlacementRuleAttribute(params TilePlacementRule[] t)
    {
        Types = t;
    }
}
