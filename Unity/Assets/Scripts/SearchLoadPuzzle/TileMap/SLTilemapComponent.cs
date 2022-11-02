using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class SLTilemapComponent : MonoBehaviour
{
    [Min(1)]
    public int tileMapSizeX = 1;
    [Min(1)]
    public int tileMapSizeY = 1;
    [Min(0.001f)]
    public float tileScaleUnit = 0.1f;

    public List<GameObject> tileObjectList;
    public int[,] tileIndexes;
    public List<GameObject> garbageList;
  
    public void CreateTileMapAt(GameObject Parent)
    {
        garbageList = new List<GameObject>();
        foreach (GameObject tileObj in GameObject.FindGameObjectsWithTag("Tile"))
        {
            garbageList.Add(tileObj);
        }

        int totalCount = tileMapSizeX * tileMapSizeY;
        int newCount = totalCount - garbageList.Count;

        if (newCount > 0)
        {
            for(int i=0; i<newCount; i++)
            {
                GameObject tileObj = SpawnTile();
                if (Parent) tileObj.transform.SetParent(Parent.transform);
                garbageList.Add(tileObj);
            }
        }

        if (garbageList.Count > 0)
        {
            tileIndexes = new int[tileMapSizeX, tileMapSizeY];
            SetTileObjectListFrom(garbageList, tileMapSizeX, tileMapSizeY);
        }

        foreach (GameObject tile in garbageList) tile.SetActive(false);

    }

    GameObject SpawnTile()
    {
        return SpawnTileAt(Vector3.zero);
    }

    GameObject SpawnTileAt(Vector3 location)
    {
        GameObject tileObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
        tileObj.transform.position = location;
        tileObj.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        tileObj.transform.localScale = Vector3.one * tileScaleUnit;
        tileObj.transform.SetParent(gameObject.transform);
        tileObj.AddComponent<SLTileComponent>();

        return tileObj;
    }

    void SetTileObjectListFrom(List<GameObject> tiles, int x = 1, int y = 1)
    {
        if (tiles.Count < 0)
        {
            Debug.LogWarning("Has not tiles for tilemap.");
            return;
        }

        tileObjectList = new List<GameObject>();
        int index = 0;
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                int lastIndex = tiles.Count - 1;
                index += 1;
                GameObject tileObj = tiles[lastIndex];
                tileObj.SetActive(true);
                tileObj.name = "Tile_" + index.ToString();             
                tileObj.transform.position = new Vector3(i, j, 0);
                tileObjectList.Add(tileObj);

                tiles.RemoveAt(lastIndex);

                tileIndexes[i, j] = index;
            }
        }
    }
}
