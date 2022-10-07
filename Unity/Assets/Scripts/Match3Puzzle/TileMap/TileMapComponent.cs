using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;


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

    public string currentTileMapID = "map01";
    public GameObject currentSelectedTile;
    public GameObject swapingTargetTile;

    TileSpawnerList tileSpawner = new TileSpawnerList();

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

        tileSpawner.CreateSpawnerGameObjects(x, y);


        foreach (var tiles in tileList.Select((value, index) => new { value, index }))
        {
            foreach (var tile in tiles.value.Select((value, index) => new { value, index }))
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
        Vector2Int mapSize = new Vector2Int(tileList.Count, tileList[0].Count);
        List<GameObject> mergedTileList = new List<GameObject>();
        foreach (List<GameObject> tiles in tileList)
        {
            mergedTileList.AddRange(tiles);
        }
        mergedTileList.OrderBy(x => x.GetComponent<TileComponent>().currentTilePosition.x).ToList();
        // 다시 분리
        tileList = new List<List<GameObject>>();
        int count = 0;
        for (int i = 0; i < mapSize.x; i++)
        {
            List<GameObject> tiles = new List<GameObject>();
            for (int j = 0; j < mapSize.y; j++)
            {
                int index = mapSize.y * i + j;
                tiles.Add(mergedTileList[index]);
                count++;
            }
            tileList.Add(tiles);
        }
    }

    public bool HasMovingTile()
    {
        foreach (List<GameObject> tiles in tileList)
        {
            foreach (GameObject tile in tiles)
            {
                if (tile.GetComponent<TileComponent>().isMoving) return true;
            }
        }

        return false;
    }


    public bool Save()
    {
        string tileMap_Json = GetTileMap_JSON(currentTileMapID);
        string dir = Application.dataPath + "/Save/";
        string path = dir + currentTileMapID + ".map";

        if (currentTileMapID == "" || Path.GetDirectoryName(path) == "")
        {
            Debug.LogWarning("please set file name.");
            return false;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(dir));
        File.WriteAllText(path, tileMap_Json);
        return true;
    }

    public bool LoadTileMap_JSON(string path)
    {
        // 경로 이상 체크
        if (Path.GetDirectoryName(path) == "")
        {
            Debug.LogWarning("please select file.");
            return false;
        }

        if (NedCrow.Convert.Right(path, 4) != ".map")
        {
            Debug.LogWarning("please load file has extension '.map'.");
            return false;
        }

        // Load TileMap data
        StreamReader r = new StreamReader(path);
        string jsonString = r.ReadToEnd();

        TileMap tilemap = Newtonsoft.Json.JsonConvert.DeserializeObject<TileMap>(jsonString);

        if (tilemap != null) Debug.Log("hello: " + tilemap.id);


        // Update tilemap
        tileMapSize.x = tilemap.GetMapSize().x;
        tileMapSize.y = tilemap.GetMapSize().y;
        PuzzleManager.instance.DrawTileMap();

        foreach (var tile in tilemap.tileList.Select((value, index) => (value, index)))
        {
            GameObject currentTile = tileList[tile.index / tileMapSize.x][tile.index % tileMapSize.y];
            currentTile.GetComponent<TileComponent>().ChangeTileType((ETileType)tile.value.type);
            currentTile.transform.position = new Vector3(tile.value.position[0], tile.value.position[1], currentTile.transform.position.z);
        }

        return true;
    }

    private string GetTileMap_JSON(string mapID = "noname")
    {
        TileMap tileMap = new TileMap();
        tileMap.id = mapID;
        tileMap.description = "tile size is " + tileMapSize.x + " x " + tileMapSize.y;
        tileMap.SetMapSize(tileMapSize.x, tileMapSize.y);
        foreach (List<GameObject> tiles in tileList)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                Tile tileObj = new Tile();
                TileComponent tileComponent = tiles[i].GetComponent<TileComponent>();
                tileObj.type = (int)tileComponent.currentType;
                tileObj.position = new int[2] { tileComponent.currentTilePosition.x, tileComponent.currentTilePosition.y };
                tileMap.tileList.Add(tileObj);
            }
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(tileMap);
    }

    private bool IsMatchNormal()
    {
        // 한 줄에 3개 이상 시 return true;
        return false;
    }
    private bool IsMatchArrow_V()
    {
        // 세로 줄에 4개 이상 시 return true;
        return false;
    }
    private bool isMatchArrow_H()
    {
        // 가로 줄에 4개 이상 시 return true;
        return false;
    }
    private bool isMatchBox()
    {
        // 세로 줄에 4개 이상 시 return true;
        return false;
    }
    private bool isMatchFive() { return false; }
    private bool isMatchStraight() { return false; }

    private void ChangeTileType(GameObject tile, ETileType tileType)
    {
        TileComponent tileCompnent = tile.GetComponent<TileComponent>();
        tileCompnent.ChangeTileType(tileType);
    }

    // 맵 생성에 스포너 추가
    private GameObject SpawnTile(GameObject tile, Vector2Int spawnPoint, ETileType[] usingTypes)
    {
        TileComponent tileCompnent = tile.GetComponent<TileComponent>();
        if (tileCompnent)
        {
            tileCompnent.currentType = (ETileType)usingTypes.GetValue(Random.Range(0, usingTypes.Length));
            tileCompnent.currentTilePosition = spawnPoint;
        }

        tile.transform.localPosition = new Vector3(spawnPoint.x, spawnPoint.y, .0f) * tileDistanceUnit;
        tile.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        tile.transform.localScale = Vector3.one * tileScaleUnit;
        tile.name = "Tile_" + tileCompnent.currentType.ToString();

        if (PuzzleManager.instance.tileMaterials.Length < 1)
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

    public void UpdateTileListAfterBrokenThose(Vector2Int[] BrokenTilePoints)
    {
        GameObject brokenGameObject;
        List<int> moveableTileList = new List<int>();
        Vector2Int targetLocation = Vector2Int.zero;
        Vector3 teleportationLocation;

        int add_TargetPos_X = 0;
        int add_TargetPos_Y = 0;
        int add_Teleport_X = 0;
        int add_Teleport_Y = 0;
        float multi_Teleport_X = .0f;
        float multi_Teleport_Y = .0f;
        int insertIndex = 0;
        int multi_index = 0; 
        int countOfLoop = 0;
        bool isVerticalGravity = true;

        switch (PuzzleManager.instance.globalGravity)
        {
            case EGravity.Down:
                add_TargetPos_X = 0; add_TargetPos_Y = -1; multi_Teleport_X = 1; multi_Teleport_Y = 0; add_Teleport_X = 0; add_Teleport_Y = tileMapSize.y;
                insertIndex = tileMapSize.y - 1;
                multi_index = 1;  countOfLoop = tileMapSize.y; isVerticalGravity = true;
                break;
            case EGravity.Up:
                add_TargetPos_X = 0; add_TargetPos_Y = 1; multi_Teleport_X = 1; multi_Teleport_Y = 0; add_Teleport_X = 0; add_Teleport_Y = -1;
                insertIndex = 0;
                multi_index = -1; countOfLoop = 1; isVerticalGravity = true;
                break;
            case EGravity.Right:
                add_TargetPos_X = 1; add_TargetPos_Y = 0; multi_Teleport_X = 0; multi_Teleport_Y = 1; add_Teleport_X = -1; add_Teleport_Y = 0;
                insertIndex = 0;
                multi_index = -1; countOfLoop = 1; isVerticalGravity = false;
                break;
            case EGravity.Left:
                add_TargetPos_X = -1; add_TargetPos_Y = 0; multi_Teleport_X = 0; multi_Teleport_Y = 1; add_Teleport_X = tileMapSize.x; add_Teleport_Y = 0;
                insertIndex = tileMapSize.x - 1;
                multi_index = 1; countOfLoop = tileMapSize.x; isVerticalGravity = false;
                break;
        }

        foreach (Vector2Int brokenPoint in BrokenTilePoints)
        {
            // teleport broken tile
            brokenGameObject = tileList[brokenPoint.x][brokenPoint.y].gameObject;
            teleportationLocation.x = brokenGameObject.transform.position.x * multi_Teleport_X + add_Teleport_X;
            teleportationLocation.y = brokenGameObject.transform.position.y * multi_Teleport_Y + add_Teleport_Y;
            teleportationLocation.z = brokenGameObject.transform.position.z;

            brokenGameObject.transform.position = teleportationLocation;
            brokenGameObject.GetComponent<TileComponent>().currentTilePosition.x = (int)(brokenPoint.x * multi_Teleport_X + add_Teleport_X);
            brokenGameObject.GetComponent<TileComponent>().currentTilePosition.y = (int)(brokenPoint.y * multi_Teleport_Y + add_Teleport_Y);

            
            // update tile list
            if (isVerticalGravity)
            {
                tileList[brokenPoint.x].RemoveAt(brokenPoint.y);
                tileList[brokenPoint.x].Insert(insertIndex, brokenGameObject);
            }
            else
            {
                int ii = brokenPoint.x * multi_index;
                tileList[brokenPoint.x].RemoveAt(brokenPoint.y);
                while(ii < countOfLoop-1)
                {
                    int index_X = ii * multi_index;
                    int index_Next_X = (index_X + multi_index);

                    tileList[index_X].Insert(brokenPoint.y, tileList[index_Next_X][brokenPoint.y].gameObject);
                    tileList[index_Next_X].RemoveAt(brokenPoint.y);
                    ii += 1;
                }
                tileList[insertIndex].Insert(brokenPoint.y, brokenGameObject);
            }
            

            // slide tiles
            int i = isVerticalGravity ? brokenPoint.y * multi_index : brokenPoint.x * multi_index;
            while (i < countOfLoop)
            {
                int index_X = isVerticalGravity ? brokenPoint.x : i * multi_index;
                int index_Y = isVerticalGravity ? i * multi_index : brokenPoint.y;
                Debug.Log(i + "<" + countOfLoop);
                targetLocation.x = tileList[index_X][index_Y].GetComponent<TileComponent>().currentTilePosition.x + add_TargetPos_X;
                targetLocation.y = tileList[index_X][index_Y].GetComponent<TileComponent>().currentTilePosition.y + add_TargetPos_Y;
                tileList[index_X][index_Y].GetComponent<TileComponent>().MoveTo(targetLocation);
                i += 1;
            }
        }
    }
}
