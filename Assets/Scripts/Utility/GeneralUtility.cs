using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

public static class GeneralUtility
{
    public static Grid MainGrid { get
        {
            if (_maingrid == null)
                _maingrid = Object.FindObjectOfType<Grid>();
            return _maingrid;
        } 
    }
    private static Grid _maingrid;

    public static Vector3 GetMousePosition()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }

    public static Point GetGridLocationOfMouse()
    {
        return MainGrid.LocalToCell(GetMousePosition()).AsPoint();
    }

    public static Vector3 GetLocalCenterOfCell(Point gridLocation)
    {
        return MainGrid.GetCellCenterLocal(new Vector3Int(gridLocation.X, gridLocation.Y, 0));
    }

    public static Tilemap GetTilemap(GridLayer layer)
    {
        return MainGrid.GetComponentsInChildren<Tilemap>().First(t => t.name == layer.GetDescription());
    }

    /// <summary>
    /// Gets the value of the description attribute of an enum.
    /// </summary>
    public static string GetDescription(this Enum enumValue)
    {
        //Look for DescriptionAttributes on the enum field
        object[] attr = enumValue.GetType().GetField(enumValue.ToString())
            .GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attr.Length > 0) // a DescriptionAttribute exists; use it
            return ((DescriptionAttribute)attr[0]).Description;
        return "";
    }
}
