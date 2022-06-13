using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RestedTilles
{
    private List<GameObject> restedTileList = new List<GameObject>();
    private List<GameObject> activatedTileList = new List<GameObject>();
    public int GetCount() { return restedTileList.Count() + activatedTileList.Count(); }
    public void AddNewTile()
    {
        GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Plane);
        tile.AddComponent<TileComponent>();
        tile.tag = "Tile";
        tile.transform.SetParent(GameObject.Find("PuzzleManager").transform);
        restedTileList.Add(tile);
        restedTileList[restedTileList.Count() - 1].SetActive(false);
        
    }

    public GameObject ActiveTile()
    {
        GameObject restedTile = restedTileList[restedTileList.Count - 1];
        restedTile.SetActive(true);
        activatedTileList.Add(restedTile);
        restedTileList.RemoveAt(restedTileList.Count() - 1);
        
        return restedTile;
    }

    public void RestTile(GameObject tile)
    {
        if (tile == null) return;
        restedTileList.Add(tile);
        restedTileList[restedTileList.Count - 1].SetActive(false);
        activatedTileList.RemoveAt(activatedTileList.IndexOf(tile));
    }

    public bool IsGotTile(GameObject tile)
    {
        bool isRestedTile = restedTileList.IndexOf(tile) >= 0 ? true : false;
        bool isActivatedTile = activatedTileList.IndexOf(tile) >= 0 ? true : false;
        return isRestedTile || isActivatedTile;
    }
}

[ExecuteInEditMode]
public class TileCreator : MonoBehaviour
{
    public List<List<GameObject>> tileList;
    public RestedTilles restedTilesObject = new RestedTilles();

    [HideInInspector]
    public float tileScaleUnit = 1;
    [HideInInspector]
    public float tileDistanceUnit = 1;

    [HideInInspector]
    [SerializeField]
    public Vector2Int tileMapSize = Vector2Int.one;

    public void UpdateTileMap(int x, int y)
    {
        Debug.Log(x + ", " + y);
        tileMapSize = new Vector2Int(x, y);
        if (tileList == null) tileList = new List<List<GameObject>>();

        // Clean trash tiles
        foreach (GameObject tile in GameObject.FindGameObjectsWithTag("Tile"))
        {
            if (restedTilesObject.IsGotTile(tile) == false) DestroyImmediate(tile);
        }

        // Rested tiles 추가
        int tileMaxCount = x * y;
        int countOfLoop = Mathf.Clamp(tileMaxCount - restedTilesObject.GetCount(), 0, tileMaxCount);
        for (int i = 0; i < countOfLoop; i++)
        {
            restedTilesObject.AddNewTile();
        }

        // X열 삭제
        int countX = x - tileList.Count();        
        if (countX < 0)
        {
            for (int i = 0; i < Mathf.Abs(countX); i++)
            {
                foreach (GameObject tile in tileList[tileList.Count() - 1])
                {
                    restedTilesObject.RestTile(tile);
                }
                tileList.RemoveAt(tileList.Count() - 1);
            }
        }

        // Y행 삭제
        int countY = tileList.Count() == 0 ? 0 : y - tileList[0].Count();
        if (countY < 0)
        {
            foreach (List<GameObject> tiles in tileList)
            {
                for (int i = 0; i < Mathf.Abs(countY); i++)
                {
                    restedTilesObject.RestTile(tiles[tiles.Count() - 1]);
                    tiles.RemoveAt(tiles.Count() - 1);
                }
            }
        }

        // X열 추가
        if (countX > 0)
        {
            int startIndex = tileList.Count();
            int countInAColumn = tileList.Count() == 0 ? y : tileList[0].Count();
            for (int i = 0; i < countX; i++)
            {
                List<GameObject> tileObjects = new List<GameObject>();

                for (int j = 0; j < countInAColumn; j++)
                {
                    tileObjects.Add(restedTilesObject.ActiveTile());
                }
                tileList.Add(tileObjects);
            }
        }

        // Y행 추가
        if (countY > 0)
        {
            for (int i = 0; i < tileList.Count(); i++)
            {
                int startIndex = tileList[i].Count();
                for (int j = 0; j < countY; j++)
                {
                    tileList[i].Add(restedTilesObject.ActiveTile());
                }
            }
        }

        foreach (var tiles in tileList.Select((value, index) => new { value, index }))
        {
            foreach (var tile in tiles.value.Select((value, index) => new {value, index}))
            {

                SpawnTile(tile.value, new Vector2Int(tiles.index, tile.index));
            }
        }
    }

    private GameObject SpawnTile(GameObject tile, Vector2Int spawnPosition)
    {
        System.Array valuses = System.Enum.GetValues(typeof(ETileType));

        TileComponent tileCompnent = tile.GetComponent<TileComponent>();
        if (tileCompnent)
        {
            tileCompnent.currentType = (ETileType)valuses.GetValue(Random.Range(0, valuses.Length));
            tileCompnent.currentPosition = new Vector3(spawnPosition.x, spawnPosition.y, 0f);
        }

        tile.transform.localPosition = new Vector3(spawnPosition.x, spawnPosition.y, .0f) * tileDistanceUnit;
        tile.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        tile.transform.localScale = Vector3.one * tileScaleUnit;
        tile.name = "Tile_" + tileCompnent.currentType.ToString();

        tile.gameObject.GetComponent<MeshRenderer>().material = gameObject.GetComponentInParent<PuzzleManager>().tileMaterials[((int)tileCompnent.currentType)];

        return tile;
    }
}
