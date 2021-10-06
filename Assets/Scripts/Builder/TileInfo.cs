using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileInfo : MonoBehaviour
{
    [SerializeField]
    public GridLayer Layer;
    [SerializeField]
    public TileEntity Entity;

    [SerializeField]
    public TileBase[] Tile;

}
