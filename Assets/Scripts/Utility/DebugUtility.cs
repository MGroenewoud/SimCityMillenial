using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugUtility : MonoBehaviour
{
    public void ShowPath(Stack<Vector3Int> path)
    {
        while (path.Any())
        {
            var link = path.Pop();
            var spawnPoint = GeneralUtility.GetLocalCenterOfCell(link.AsPoint());
            Instantiate(SimulationCore.Instance.Dot, spawnPoint, Quaternion.identity);
        }
    }
}
