using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileMapComponent : MonoBehaviour
{
    public List<List<GameObject>> tileList;
    public List<GameObject> demolishingTargetList;
    public List<Vector3> SpawningPosList_SpecialTile1;
    public List<Vector3> SpawningPosList_SpecialTile2;
    public List<Vector3> SpawningPosList_SpecialTile3;
    public RestedTilles restedTilesObject = null;

    [HideInInspector]
    public float tileScaleUnit = 1;
    [HideInInspector]
    public float tileDistanceUnit = 1;

    [HideInInspector]
    [SerializeField]
    public Vector2Int tileMapSize = Vector2Int.one;

    public GameObject currentSelectedTile;
    public GameObject swapingTargetTile;

    public void UpdateTileMap(int x, int y)
    {
        tileMapSize = new Vector2Int(x, y);
        if (tileList == null) tileList = new List<List<GameObject>>();
        if (restedTilesObject == null) restedTilesObject = new RestedTilles();

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

                SpawnTile(tile.value, new Vector2Int(tiles.index, tile.index), PuzzleManager.instance.globalUsingTypes);
            }
        }
        SortTileList();
    }

    public void MatchTileList()
    {
        demolishingTargetList = new List<GameObject>();
        
        SortTileList();
        int countOfSameless = 0;
        // 선택 타일 가로(tileList[y-1]), 세로(tileList[0~n-1][x-1])
        // 타겟 타일 가로(tileList[y-1]), 세로(tileList[0~n-1][x-1]) - x 위치 같으면 세로 제외, y 같으면 가로 제외
        // 폭발 및 스페셜타일 생성위치(스와핑 및 폭발 있었던 타일 위치 근처로) 계산
        EMatchType matchType = EMatchType.None;
        if (IsMatchNormal())
        {
            if (isMatchStraight())
            {
                ChangeTileType(currentSelectedTile, ETileType.S1);
            }
            else if (isMatchFive())
            {
                ChangeTileType(currentSelectedTile, ETileType.S2);
            }
            else if (isMatchArrow_H())
            {
                ChangeTileType(currentSelectedTile, ETileType.S3);
            }
            else if (IsMatchArrow_V())
            {
                ChangeTileType(currentSelectedTile, ETileType.S4);
            }
            else if (isMatchBox())
            {
                ChangeTileType(currentSelectedTile, ETileType.S5);
            }
        }
        else
        {
            if (isMatchBox())
            {
                ChangeTileType(currentSelectedTile, ETileType.S5);
            }
        }
        if (isMatchStraight())
        {
            ChangeTileType(currentSelectedTile, ETileType.S6);
        }
    }
    public void SortTileList()
    {
        List<GameObject> mergedTileList = new List<GameObject>();
        foreach(List<GameObject> tiles in tileList)
        {
            mergedTileList.AddRange(tiles);
        }
        mergedTileList.OrderBy(x => x.GetComponent<TileComponent>().currentTilePosition.x).ToList();
        // 다시 분리
    }

    private bool IsMatchNormal() { 
        // 한 줄에 3개 이상 시 return true;
        return false; }
    private bool IsMatchArrow_V() {
        // 세로 줄에 4개 이상 시 return true;
        return false; }
    private bool isMatchArrow_H() {
        // 가로 줄에 4개 이상 시 return true;
        return false; }
    private bool isMatchBox() {
        // 세로 줄에 4개 이상 시 return true;
        return false; }
    private bool isMatchFive() { return false; }
    private bool isMatchStraight() { return false; }

    private void ChangeTileType(GameObject tile, ETileType tileType)
    {
        TileComponent tileCompnent = tile.GetComponent<TileComponent>();
        tileCompnent.ChangeTileType(tileType);
    }

    // 맵 생성에 스포너 추가
    private GameObject SpawnTile(GameObject tile, Vector2Int spawnPosition, ETileType[] usingTypes)
    {
        TileComponent tileCompnent = tile.GetComponent<TileComponent>();
        if (tileCompnent)
        {
            tileCompnent.currentType = (ETileType)usingTypes.GetValue(Random.Range(0, usingTypes.Length));
            tileCompnent.currentTilePosition = spawnPosition;
        }

        tile.transform.localPosition = new Vector3(spawnPosition.x, spawnPosition.y, .0f) * tileDistanceUnit;
        tile.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        tile.transform.localScale = Vector3.one * tileScaleUnit;
        tile.name = "Tile_" + tileCompnent.currentType.ToString();

        if(PuzzleManager.instance.tileMaterials.Length < 1)
        {
            Debug.LogError("Empty material list of PuzzleManager");
            return null;
        }

        // Material from type
        tile.GetComponent<MeshRenderer>().material = PuzzleManager.instance.tileMaterials[
             Mathf.Clamp((int)tileCompnent.currentType, 0, PuzzleManager.instance.tileMaterials.Length)
        ];

        return tile;
    }


}
