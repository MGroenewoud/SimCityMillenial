using System;
using UnityEngine;

public class ScreenEdgeScroller : MonoBehaviour
{
    [SerializeField]
    private float EdgeSize = 30f;
    [SerializeField]
    private float Movespeed = 10f;

    private Camera Cam;

    void Start()
    {
        Cam = GetComponent<Camera>();
    }

    void Update()
    {
        var position = Cam.transform.position;
        var gridsizeWidth = SimulationCore.Instance.Grid.Width;
        var gridsizeHeight = SimulationCore.Instance.Grid.Height;

        if (Input.mousePosition.x > Screen.width - EdgeSize && position.x < (gridsizeWidth / 2))
            position.x += Movespeed * Time.deltaTime;
        if (Input.mousePosition.x < EdgeSize && position.x > -(gridsizeWidth / 2))
            position.x -= Movespeed * Time.deltaTime;
        if (Input.mousePosition.y > Screen.height - EdgeSize && position.y < (gridsizeHeight / 2))
            position.y += Movespeed * Time.deltaTime;
        if (Input.mousePosition.y < EdgeSize && position.y > 0)
            position.y -= Movespeed * Time.deltaTime;
        Cam.transform.position = position;
    }

}
