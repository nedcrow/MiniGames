using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//[ExecuteInEditMode]
public class Match3TileMapComponent : TileMapComponent
{
    [Header("Match3 Properties")]
    public E3MTileType[] currentUsingTypes;
    public List<Vector3> SpawningPosList_SpecialTile1;
    public List<Vector3> SpawningPosList_SpecialTile2;
    public List<Vector3> SpawningPosList_SpecialTile3;

    public override void UpdateTileMap(int sizeX, int sizeY)
    {
        base.UpdateTileMap(sizeX, sizeY);
        foreach(GameObject tileObj in tileObjectList)
        {
            Match3TileComponent tileComp = tileObj.GetComponent<Match3TileComponent>();
            tileComp.ChangeTileType((E3MTileType) currentUsingTypes.GetValue(
                    Random.Range(0, currentUsingTypes.Length)
                    ));
        }
    }

    public void UpdatetileObjectListAfterBrokenThose(Vector2Int[] BrokenTilePoints)
    {
        GameObject brokenGameObject;
        List<int> moveabletileObjectList = new List<int>();
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
                add_TargetPos_X = 0; add_TargetPos_Y = -1; multi_Teleport_X = 1; multi_Teleport_Y = 0; add_Teleport_X = 0; add_Teleport_Y = currentMapSize.y;
                insertIndex = currentMapSize.y - 1;
                multi_index = 1; countOfLoop = currentMapSize.y; isVerticalGravity = true;
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
                add_TargetPos_X = -1; add_TargetPos_Y = 0; multi_Teleport_X = 0; multi_Teleport_Y = 1; add_Teleport_X = currentMapSize.x; add_Teleport_Y = 0;
                insertIndex = currentMapSize.x - 1;
                multi_index = 1; countOfLoop = currentMapSize.x; isVerticalGravity = false;
                break;
        }

        foreach (Vector2Int brokenPoint in BrokenTilePoints)
        {
            // teleport broken tile
            int brokenIndex = tileIndexesList[brokenPoint.x][brokenPoint.y];
            brokenGameObject = tileObjectList[brokenIndex].gameObject;
            teleportationLocation.x = brokenGameObject.transform.position.x * multi_Teleport_X + add_Teleport_X;
            teleportationLocation.y = brokenGameObject.transform.position.y * multi_Teleport_Y + add_Teleport_Y;
            teleportationLocation.z = brokenGameObject.transform.position.z;

            brokenGameObject.transform.position = teleportationLocation;
            brokenGameObject.GetComponent<Match3TileComponent>().tileLocation.x = (int)(brokenPoint.x * multi_Teleport_X + add_Teleport_X);
            brokenGameObject.GetComponent<Match3TileComponent>().tileLocation.y = (int)(brokenPoint.y * multi_Teleport_Y + add_Teleport_Y);


            // update tile list
            if (isVerticalGravity)
            {
                tileIndexesList[brokenPoint.x].RemoveAt(brokenPoint.y);
                tileIndexesList[brokenPoint.x].Insert(insertIndex, brokenIndex);
            }
            else
            {
                int ii = brokenPoint.x * multi_index;
                tileIndexesList[brokenPoint.x].RemoveAt(brokenPoint.y);
                while (ii < countOfLoop - 1)
                {
                    int index_X = ii * multi_index;
                    int index_Next_X = (index_X + multi_index);

                    tileIndexesList[index_X].Insert(brokenPoint.y, tileIndexesList[index_Next_X][brokenPoint.y]);
                    tileIndexesList[index_Next_X].RemoveAt(brokenPoint.y);
                    ii += 1;
                }
                tileIndexesList[insertIndex].Insert(brokenPoint.y, brokenIndex);
            }


            // slide tiles
            int i = isVerticalGravity ? brokenPoint.y * multi_index : brokenPoint.x * multi_index;
            while (i < countOfLoop)
            {
                int index_X = isVerticalGravity ? brokenPoint.x : i * multi_index;
                int index_Y = isVerticalGravity ? i * multi_index : brokenPoint.y;
                int tileIndex = tileIndexesList[index_X][index_Y];
                Debug.Log(i + "<" + countOfLoop);
                targetLocation.x = tileObjectList[tileIndex].GetComponent<Match3TileComponent>().tileLocation.x + add_TargetPos_X;
                targetLocation.y = tileObjectList[tileIndex].GetComponent<Match3TileComponent>().tileLocation.y + add_TargetPos_Y;
                tileObjectList[tileIndex].GetComponent<Match3TileComponent>().MoveTo(targetLocation);
                i += 1;
            }
        }
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
        currentMapSize.x = tilemap.GetMapSize().x;
        currentMapSize.y = tilemap.GetMapSize().y;
        PuzzleManager.instance.DrawTileMap();

        //foreach (var tile in tilemap.tileObjectList.Select((value, index) => (value, index)))
        //{
        //    GameObject currentTile = tileObjectList[tile.index / CurrentMapSize.x][tile.index % CurrentMapSize.y];
        //    currentTile.GetComponent<Match3TileComponent>().ChangeTileType((ETileType)tile.value.type);
        //    currentTile.transform.position = new Vector3(tile.value.position[0], tile.value.position[1], currentTile.transform.position.z);
        //}

        return true;
    }

    protected string GetTileMap_JSON(string mapID = "noname")
    {
        TileMap tileMap = new TileMap();
        tileMap.id = mapID;
        tileMap.description = "tile size is " + currentMapSize.x + " x " + currentMapSize.y;
        tileMap.SetMapSize(currentMapSize.x, currentMapSize.y);
        //foreach (List<GameObject> tiles in tileObjectList)
        //{
        //    for (int i = 0; i < tiles.Count; i++)
        //    {
        //        Tile tileObj = new Tile();
        //        Match3TileComponent tileComponent = tiles[i].GetComponent<Match3TileComponent>();
        //        tileObj.type = (int)tileComponent.currentType;
        //        tileObj.position = new int[2] { tileComponent.currentTilePosition.x, tileComponent.currentTilePosition.y };
        //        tileMap.tileObjectList.Add(tileObj);
        //    }
        //}
        return Newtonsoft.Json.JsonConvert.SerializeObject(tileMap);
    }

    protected void ChangeTileType(GameObject tile, E3MTileType tileType)
    {
        Match3TileComponent tileComp = tile.GetComponent<Match3TileComponent>();
        tileComp.ChangeTileType(tileType);
    }

    protected override GameObject SpawnTileObject(float scale = 1)
    {
        GameObject tileObj = base.SpawnTileObject(scale);

        Match3TileComponent tileComp = tileObj.GetComponent<Match3TileComponent>();
        if (tileComp)
        {
            tileComp.ChangeTileType((E3MTileType) currentUsingTypes.GetValue(
                Random.Range(0, currentUsingTypes.Length)
                ));
        }
        tileObj.name = "Tile_" + tileComp.GetCurrentType().ToString();
        return tileObj;
    }
}
