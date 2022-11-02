using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileComponent : MonoBehaviour
{
    public Vector2Int tileLocation = Vector2Int.zero;
    public void SetTileTransform(Vector2Int location, float scale)
    {
        tileLocation = location;
        transform.position = new Vector3(
                location.x * scale,
                location.y * scale,
                transform.position.z
            );
    }
}
