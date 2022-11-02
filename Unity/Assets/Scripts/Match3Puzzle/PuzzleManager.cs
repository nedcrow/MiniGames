using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PuzzleManager : MonoBehaviour
{
    #region instance
    private static PuzzleManager _instance;
    //private static readonly object padlock = new object();
    public static PuzzleManager instance
    {
        get
        {
            //lock (padlock)
            {
                if (_instance == null)
                {
                    GameObject obj = GameObject.Find("PuzzleManager");
                    if (obj == null)
                    {
                        obj = new GameObject("PuzzleManager");
                        obj.AddComponent<PuzzleManager>();
                    }
                    return obj.GetComponent<PuzzleManager>();
                }
                else
                {
                    return _instance;
                }
            }
        }
    }
    #endregion

    // puzzle properties
    public EGameMode currentGameMode = EGameMode.Play_Normal;
    public ETileType[] globalUsingTypes = { ETileType.Apple, ETileType.Banana, ETileType.Grape, ETileType.Orange };
    public EGravity globalGravity = EGravity.Down;
    public List<List<EGravity>> gravityMatrix = new List<List<EGravity>>();

    public GameObject selectedTileObj;

    [SerializeField]
    private float tileScale_editor = 1.0f;
    [SerializeField]
    private float tileDistance_editor = 1.0f;

    private IEnumerator startPuzzle_co;

    private CursorComponent currentCursorComponent = null;

    void Start()
    {
        DrawTileMap();

        startPuzzle_co = StartPuzzle_Co();
        StartCoroutine(startPuzzle_co);

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


    #region TileMap
    public Material[] tileMaterials;
    public M3TileMapComponent tileMapComponent = null;
    public void DrawTileMap()
    {
        if (tileMapComponent == null)
        {
            GameObject tileMapObj = GameObject.Find("TileMap");
            tileMapObj = tileMapObj != null ? tileMapObj : new GameObject();

            tileMapComponent = tileMapObj.GetComponent<M3TileMapComponent>();
            if (tileMapComponent == null) tileMapComponent = tileMapObj.AddComponent<M3TileMapComponent>();
        }

        UpdateTileMapComponent(
             new Vector2Int(tileMapComponent.CurrentMapSize.x, tileMapComponent.CurrentMapSize.y),
             tileScale_editor,
             tileDistance_editor
         );

        Camera.main.transform.localPosition = new Vector3(
            (tileMapComponent.CurrentMapSize.x * 0.5f - 0.5f),
            (tileMapComponent.CurrentMapSize.y * 0.5f - 0.5f),
            -1.0f
        );
    }

    public void UpdateTileMapComponent(Vector2Int mapSizeValue, float tileScale = 1.0f, float tileDistance = 1.0f)
    {
        if (tileMapComponent == null)
        {
            GameObject tileMapObj = GameObject.Find("TileMap");
            tileMapObj = tileMapObj != null ? tileMapObj : new GameObject();
            
            tileMapComponent = tileMapObj.GetComponent<M3TileMapComponent>();
            if (tileMapComponent == null) tileMapComponent = tileMapObj.AddComponent<M3TileMapComponent>();
        } 
            
        tileMapComponent.tileScaleUnit = tileScale_editor = tileScale;
        tileMapComponent.tileDistanceUnit = tileDistance_editor = tileDistance;
        tileMapComponent.UpdateTileMap(mapSizeValue.x, mapSizeValue.y);
    }

    public bool UpdateCurrentSelectedTile(Match3TileComponent currentTileComponent)
    {
        tileMapComponent.UpdatetileObjectListAfterBrokenThose(new Vector2Int[1] { currentTileComponent.tileLocation });

        bool isDifferent = selectedTileObj != currentTileComponent.gameObject;
        if (!isDifferent) return false;

        //GetComponent<Match3TileMapComponent>().currentSelectedTile = currentTileComponent.gameObject;

        // 현재 게임 모드가 에디트 모드일 때와 플레이 모드일 때 구분

        return true;
    }
    #endregion


    #region Puzzle Life Cycle
    public float timelimit = 0;
    public float fps = 30;
    public int remainingTurn = 999;
    private float currentTime = 0;
    [SerializeField]
    private EPuzzleState currentPuzzleState = EPuzzleState.Wait;
    private IEnumerator StartPuzzle_Co()
    {
        switch (currentPuzzleState)
        {
            case EPuzzleState.Wait:
                // 매칭 가능여부 탐색
                // 매칭 가능하지만, 매칭은 없을 때 턴 증가
                remainingTurn -= 1;
                if (remainingTurn < 0) // 게임오버
                // 매칭 가능이 없으면 재 배치 위치 루프 탐색 후 배치
                // 배치 종료 확인
                if (timelimit > 0)
                {
                    if (currentTime >= timelimit)
                    {
                        currentPuzzleState = EPuzzleState.Match;
                        currentTime = 0;
                        break;
                    }
                    currentTime += 1 / fps;
                }
                else
                {
                    currentPuzzleState = EPuzzleState.Match;
                }
                break;

            case EPuzzleState.Match:
                // 매칭 및 스패셜 타일로 인한 폭파 목록 반환
                //tileMapComponent.MatchTileList();
                break;

            case EPuzzleState.Effect: 
                // 폭파 대기 목록이 비어있으면, 유저 인터렉션 있었으면 배치 취소
                // 비어있지 않으면 폭파
                break;

            case EPuzzleState.Spawn:
                // 생성 후 배치
                break;
        }
        yield return new WaitForSeconds(1 / fps);
    }
    #endregion
}
