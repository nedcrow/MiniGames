using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileMapComponent : MonoBehaviour
{
    [Header("Basic Properties")]
    public string currentTileMapID = "map01";
    public Vector2Int CurrentMapSize;
    public GameObject tilePrefab = null;

    [HideInInspector]
    public float tileScaleUnit = 1;
    [HideInInspector]
    public float tileDistanceUnit = 1;

    public List<GameObject> tileObjectList;
    public List<List<int>> tileIndexesList;


    private void Start()
    {
        UpdateTileMap(CurrentMapSize.x, CurrentMapSize.y);
    }

 
    public virtual void UpdateTileMap(int sizeX, int sizeY)
    {
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
                tileObjectList[index].SetActive(true);
                tileObjectList[index].name = "tile_" + index.ToString();
                tileObjectList[index].transform.localPosition = new Vector3(x, y, 0);
                index++;
            }
        }

        for(int i=targetTileCount; i< tileObjectList.Count; i++)
        {
            tileObjectList[i].SetActive(false);
        }
        
    }

    protected List<GameObject> GetAllChildTiles()
    {
        List<GameObject> tiles = new List<GameObject>();
        foreach (GameObject tileObj in GameObject.FindGameObjectsWithTag("Tile"))
        {
            string parentName = gameObject.name;
            if(tileObj.transform.parent.name == parentName) tiles.Add(tileObj);
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
