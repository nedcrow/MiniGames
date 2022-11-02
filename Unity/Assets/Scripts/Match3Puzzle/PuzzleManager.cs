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

        // ���� ���� ��尡 ����Ʈ ����� ���� �÷��� ����� �� ����

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
                // ��Ī ���ɿ��� Ž��
                // ��Ī ����������, ��Ī�� ���� �� �� ����
                remainingTurn -= 1;
                if (remainingTurn < 0) // ���ӿ���
                // ��Ī ������ ������ �� ��ġ ��ġ ���� Ž�� �� ��ġ
                // ��ġ ���� Ȯ��
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
                // ��Ī �� ���м� Ÿ�Ϸ� ���� ���� ��� ��ȯ
                //tileMapComponent.MatchTileList();
                break;

            case EPuzzleState.Effect: 
                // ���� ��� ����� ���������, ���� ���ͷ��� �־����� ��ġ ���
                // ������� ������ ����
                break;

            case EPuzzleState.Spawn:
                // ���� �� ��ġ
                break;
        }
        yield return new WaitForSeconds(1 / fps);
    }
    #endregion
}
