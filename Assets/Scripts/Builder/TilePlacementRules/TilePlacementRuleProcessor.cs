using System;
using System.Collections.Generic;
using System.Linq;

public static class TilePlacementRuleProcessor
{
    public static Dictionary<TilePlacementRule, Func<bool>> RuleLibrary;

    private static bool IsInitialized = false;

    private static Point Target => GeneralUtility.GetGridLocationOfMouse();


    public static bool CanBePlaced(TileEntity entity)
    {
        if (!IsInitialized)
            Initialize();
        return PassesRules(entity.GetRulesFromEntity());
    }

    private static void Initialize()
    {
        RuleLibrary = new Dictionary<TilePlacementRule, Func<bool>>() {
            { TilePlacementRule.MustBePlacedNextToRoad, () => MustBePlacedAdjacentToTileType(TileEntity.Road) },
            { TilePlacementRule.PlacedOnEmptyTile, () => MustBePlacedOn(TileEntity.Grass) },
            { TilePlacementRule.MustBePlacedCloseToForest, () => MustBePlacedCloseTo(TileEntity.Forest) }
        };
    }

    private static bool PassesRules(TilePlacementRule[] ruleTypes)
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

    private static bool MustBePlacedAdjacentToTileType(TileEntity type)
    {
        return SimulationCore.Instance.Grid.GetAdjacentCellsOfType(Target, type).Any();
    }

    private static bool MustBePlacedOn(TileEntity type)
    {
        return SimulationCore.Instance.Grid[Target.X, Target.Y] == type;
    }

    private static bool MustBePlacedCloseTo(TileEntity type)
    {
        return GridSearch.DijkstraHasEntitiesInRange(Target, type, 4);
    }

    private static TilePlacementRule[] GetRulesFromEntity(this TileEntity enumValue)
    {
        //Look for DescriptionAttributes on the enum field
        object[] attr = enumValue.GetType().GetField(enumValue.ToString())
            .GetCustomAttributes(typeof(PlacementRuleAttribute), false);

        if (attr.Length > 0) // a DescriptionAttribute exists; use it
            return ((PlacementRuleAttribute)attr[0]).Types;
        return new TilePlacementRule[] { };
    }
}

public enum TilePlacementRule
{
    MustBePlacedNextToRoad,
    PlacedOnEmptyTile,
    MustBePlacedCloseToForest,
}

public class PlacementRuleAttribute : Attribute
{
    public TilePlacementRule[] Types { get; set; }

    public PlacementRuleAttribute(params TilePlacementRule[] t)
    {
        Types = t;
    }
}
