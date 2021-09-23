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
        if (Input.mousePosition.x > Screen.width - EdgeSize)
            position.x += Movespeed * Time.deltaTime;
        if (Input.mousePosition.x < EdgeSize)
            position.x -= Movespeed * Time.deltaTime;
        if (Input.mousePosition.y > Screen.height - EdgeSize)
            position.y += Movespeed * Time.deltaTime;
        if (Input.mousePosition.y < EdgeSize)
            position.y -= Movespeed * Time.deltaTime;
        Cam.transform.position = position;
    }
}
