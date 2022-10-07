using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawnerComponent : MonoBehaviour
{
    public ETileType[] usingTypes;

    //public GameObject SpawnTileAt(GameObject tile, ETileType[] usingTypes, float tileDistanceUnit = 1.0f, float tileScaleUnit = 1.0f)
    //{
    //    // TileComponent
    //    TileComponent tileCompnent = tile.GetComponent<TileComponent>();
    //    if (tileCompnent == null) tileCompnent = tile.AddComponent<TileComponent>();
    //    tileCompnent.currentType = (ETileType)usingTypes.GetValue(Random.Range(0, usingTypes.Length));
    //    tileCompnent.currentTilePosition = currentPosition;

    //    // Tile
    //    tile.transform.localPosition = new Vector3(currentPosition.x, currentPosition.y, .0f) * tileDistanceUnit;
    //    tile.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
    //    tile.transform.localScale = Vector3.one * tileScaleUnit;
    //    tile.name = "Tile_" + tileCompnent.currentType.ToString();

    //    if (PuzzleManager.instance.tileMaterials.Length < 1)
    //    {
    //        Debug.LogError("Empty material list of PuzzleManager");
    //        return null;
    //    }

    //    // Material from type
    //    tile.GetComponent<MeshRenderer>().material = PuzzleManager.instance.tileMaterials[
    //         Mathf.Clamp((int)tileCompnent.currentType, 0, PuzzleManager.instance.tileMaterials.Length)
    //    ];



    //    return tile;
    //}
}
