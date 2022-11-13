using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileMapComponent : MonoBehaviour
{
    [Header("Basic Properties")]
    public string currentTileMapID = "map01";
    public Vector2Int currentMapSize;
    public GameObject tilePrefab = null;

    [Min(0.001f)]
    public float tileScaleUnit = 1;
    [HideInInspector]
    public float tileDistanceUnit = 1;

    [SerializeField]
    public List<GameObject> tileObjectList;
    public List<List<int>> tileIndexesList;


    /// <summary>
    /// minimum size is one(1x1)
    /// </summary>
    /// <param name="sizeX"> minimum is 1 </param>
    /// <param name="sizeY"> minimum is 1 </param>
    public virtual void UpdateTileMap(int sizeX, int sizeY)
    {
        sizeX = Mathf.Max(sizeX, 1);
        sizeY = Mathf.Max(sizeY, 1);
        tileObjectList = GetAllChildTiles();
        tileIndexesList = new List<List<int>>();
        int totalTileCount = tileObjectList.Count;
        int targetTileCount = sizeX * sizeY;
        int distanceOfTileCount = totalTileCount - targetTileCount;

        if (distanceOfTileCount < 0)
        {
            if(tilePrefab == null) Debug.LogWarning("Null tile prefab(Gameobject)");
            for (int i=0; i< distanceOfTileCount*-1; i++)
            {
                tileObjectList.Add(SpawnTileObject(tileScaleUnit));
            }
        }

        int index = 0;
        for(int x=0; x< sizeX; x++)
        {
            tileIndexesList.Add(new List<int>());
            for (int y = 0; y < sizeY; y++)
            {
                tileIndexesList[x].Add(index);
                tileObjectList[index].GetComponent<MeshRenderer>().enabled = true;
                tileObjectList[index].name = "tile_" + index.ToString();
                tileObjectList[index].GetComponent<TileComponent>().SetTileTransform(new Vector2Int(x, y), tileScaleUnit);
                index++;
            }
        }

        for(int i=targetTileCount; i< tileObjectList.Count; i++)
        {
            tileObjectList[i].GetComponent<MeshRenderer>().enabled = false;
        }
        
    }

    protected List<GameObject> GetAllChildTiles()
    {
        List<GameObject> tiles = new List<GameObject>();
        foreach (GameObject tileObj in GameObject.FindGameObjectsWithTag("Tile"))
        {
            string parentName = gameObject.name;
            if (tileObj.transform.parent.name == parentName) tiles.Add(tileObj);
        }
        return tiles;
    }

    protected virtual GameObject SpawnTileObject(float scale = 1)
    {
        return SpawnTileObjectAt(Vector2Int.zero, scale);
    }

    GameObject SpawnTileObjectAt(Vector2Int location, float scale)
    {
        GameObject tileObj = Instantiate<GameObject>(tilePrefab);
        tileObj.transform.SetParent(gameObject.transform);

        TileComponent tileComp = tileObj.GetComponent<TileComponent>();
        if (tileComp) tileComp.SetTileTransform(location, scale);   

        return tileObj;
    }
}
