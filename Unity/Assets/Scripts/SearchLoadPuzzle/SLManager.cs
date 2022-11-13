using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SLManager : MonoBehaviour
{
    #region instance
    private static SLManager _instance;
    public static SLManager instance
    {
        get
        {
            {
                if (_instance == null)
                {
                    GameObject obj = GameObject.Find("SLManager");
                    if (obj == null)
                    {
                        obj = new GameObject("SLManager");
                        obj.AddComponent<SLManager>();
                    }
                    return obj.GetComponent<SLManager>();
                }
                else
                {
                    return _instance;
                }
            }
        }
    }
    #endregion

    [SerializeField, Header("TilemapData")]
    public GameObject tilemapCreatorObj;
    public Vector2Int currentTileMapSize = Vector2Int.one;

    [Header("CursorData")]
    public GameObject selectedTileObj;
    private CursorComponent currentCursorComponent = null;

    void Start()
    {
        SpawnCursor();
    }

    public void UpdateTileMap()
    {
        // Set tilemapCreatorObj & SLTilemapComponent
        tilemapCreatorObj = GameObject.Find("TilemapCreator");
        if (tilemapCreatorObj == null)
        {
            Debug.LogError("null tilemap comp");
            tilemapCreatorObj = new GameObject("TilemapCreator");
            tilemapCreatorObj.AddComponent<SLTilemapComponent>();
        }
        tilemapCreatorObj.transform.SetParent(gameObject.transform);

        SLTilemapComponent sltilemapComp = tilemapCreatorObj.GetComponent<SLTilemapComponent>();

        // update tilemap
        sltilemapComp.UpdateTileMap(currentTileMapSize.x, currentTileMapSize.y);

        // camera
        Vector3 basePos = tilemapCreatorObj.transform.position;
        SLTilemapComponent tilemapComp = tilemapCreatorObj.GetComponent<SLTilemapComponent>();
        Camera.main.transform.position = new Vector3(
            basePos.x + tilemapComp.currentMapSize.x,
            basePos.y + tilemapComp.currentMapSize.y,
            basePos.z - 1
            );
    }

    void SpawnCursor()
    {
        GameObject cursorObj = GameObject.Find("Cursor");
        if (cursorObj == null) cursorObj = new GameObject();
        cursorObj.name = "Cursor";

        currentCursorComponent = cursorObj.GetComponent<CursorComponent>();
        currentCursorComponent = currentCursorComponent == null ? cursorObj.AddComponent<CursorComponent>() : currentCursorComponent;

        currentCursorComponent.SelectedTileEvent += (GameObject tileObj) =>
        {
            selectedTileObj = tileObj;
        };
    }

    // 페어파인더
    // 선택타일 오브젝트는 다른데 타입이 같으면 페어확정
}
