using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public static Builder Instance;

    public TilePreviewComponent Preview;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Preview = GetComponent<TilePreviewComponent>();
    }
}
