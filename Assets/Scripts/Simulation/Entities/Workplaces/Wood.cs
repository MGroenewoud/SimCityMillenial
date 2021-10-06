using UnityEditor;
using UnityEngine;

public abstract class WorkPlace
{
    public Point Location;

    public WorkPlace()
    {
        Location = GeneralUtility.GetGridLocationOfMouse();
    }
}