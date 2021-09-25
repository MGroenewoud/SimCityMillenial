using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PersonMovement : MonoBehaviour
{
    public Stack<Point> PathToDestination;
    public Vector3 CurrentMoveTarget;

    private Person _person;

    private void Awake()
    {
        _person = GetComponent<Person>();
    }

    public void FacePosition(Vector3 target)
    {
        var delta = GeneralUtility.MainGrid.LocalToCell(target) - GeneralUtility.MainGrid.LocalToCell(transform.position);
        var direction = Direction.All.FirstOrDefault(d => d.Item1 == delta);
        if (direction != null)
            transform.rotation = Quaternion.Euler(direction.Item2);
    }

    public bool MoveToDestination()
    {
        CurrentMoveTarget.z = -(1 / 2);
        transform.position = Vector3.MoveTowards(_person.transform.position, CurrentMoveTarget, _person.MoveSpeed * Time.deltaTime);

        if (_person.transform.position == CurrentMoveTarget)
        {
            _person.CurrentPosition = GeneralUtility.MainGrid.LocalToCell(CurrentMoveTarget).AsPoint();
            // Arrived at destination tile, grab next tile
            if (PathToDestination.Count != 0)
            {
                var nextCell = PathToDestination.Pop();
                if (SimulationCore.Instance.Grid[nextCell.X, nextCell.Y] == TileEntity.Road || PathToDestination.Count == 0)
                {
                    CurrentMoveTarget = GeneralUtility.GetLocalCenterOfCell(nextCell);
                    FacePosition(CurrentMoveTarget);
                }
                else
                {
                    Debug.Log("Path broken. Find new thing to do.");
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        return false;
    }
}

public static class Direction
{
    private static Tuple<Vector3Int, Vector3> NorthEast { get => new Tuple<Vector3Int, Vector3>(new Vector3Int(1, 0, 0), new Vector3(-14, 55, -18)); }
    private static Tuple<Vector3Int, Vector3> SouthEast { get => new Tuple<Vector3Int, Vector3>(new Vector3Int(0, -1, 0), new Vector3(14, 126, -18)); }
    private static Tuple<Vector3Int, Vector3> SouthWest { get => new Tuple<Vector3Int, Vector3>(new Vector3Int(-1, 0, 0), new Vector3(18, -128, 18)); }
    private static Tuple<Vector3Int, Vector3> NorthWest { get => new Tuple<Vector3Int, Vector3>(new Vector3Int(0, 1, 0), new Vector3(-14, -55, 18)); }

    public static List<Tuple<Vector3Int, Vector3>> All = new List<Tuple<Vector3Int, Vector3>>() { NorthEast, SouthEast, SouthWest, NorthWest };
}