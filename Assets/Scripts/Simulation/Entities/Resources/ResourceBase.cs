using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class ResourceBase
{
    [SerializeField]
    private TileBase[] Full;
    [SerializeField]
    private TileBase[] HalfFull;
    [SerializeField]
    private TileBase[] Empty;

    public ResourceType Type;
}

public enum ResourceType
{
    Wood,
    Stone,
    Food,
    Wool,
}