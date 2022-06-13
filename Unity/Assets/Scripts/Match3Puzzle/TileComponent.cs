using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETileType
{
    Apple = 0,
    Banana = 1,
    Grape = 2,
    Orange = 3,
}
public class TileComponent : MonoBehaviour
{
    public ETileType currentType;
    public Vector2 currentLocation;
}
