using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileComponent : MonoBehaviour
{
    public Vector2Int tileLocation = Vector2Int.zero;
    public Vector3 originScale = Vector3.one;
    string typeName = "";

    void Awake()
    {
        gameObject.tag = "Tile";
    }

    public void SetTileTransform(Vector2Int location, float scale)
    {
        tileLocation = location;
        transform.position = new Vector3(
                location.x,
                location.y,
                transform.position.z
            );

        transform.localScale = originScale * scale;
    }

    public virtual void SetTypeName(string type)
    {
        typeName = type;
    }

    public virtual string GetTypeName()
    {
        return typeName;
    }
}
