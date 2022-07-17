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

    [SerializeField]
    private float tileScale_editor = 1.0f;
    [SerializeField]
    private float tileDistance_editor = 1.0f;

    private IEnumerator startPuzzle_co;

    void Start()
    {
        DrawTileMap();

        startPuzzle_co = StartPuzzle_Co();
        StartCoroutine(startPuzzle_co);
    }


    #region TileMap
    public Material[] tileMaterials;
    public TileMapComponent tileMapComponent = null;
    public void DrawTileMap()
    {
        UpdateTileMapComponent(
             new Vector2Int(tileMapComponent.tileMapSize.x, tileMapComponent.tileMapSize.y),
             tileScale_editor,
             tileDistance_editor
         );

        Camera.main.transform.localPosition = new Vector3(
            (tileMapComponent.tileMapSize.x * 0.5f - 0.5f),
            (tileMapComponent.tileMapSize.y * 0.5f - 0.5f),
            -1.0f
        );
    }

    public void UpdateTileMapComponent(Vector2Int mapSizeValue, float tileScale = 1.0f, float tileDistance = 1.0f)
    {
        if (tileMapComponent == null) tileMapComponent = gameObject.AddComponent<TileMapComponent>();
        //tileMapComponent.UpdateTileMap(tileMapComponent.tileMapSize.x, tileMapComponent.tileMapSize.y);
        tileMapComponent.tileScaleUnit = tileScale_editor = tileScale;
        tileMapComponent.tileDistanceUnit = tileDistance_editor = tileDistance;
        tileMapComponent.UpdateTileMap(mapSizeValue.x, mapSizeValue.y);
    }
    #endregion


    #region Puzzle Life Cycle
    public float timelimit = 0;
    public float fps = 30;
    private float currentTime = 0;
    [SerializeField]
    private EPuzzleState currentPuzzleState = EPuzzleState.Wait;
    private IEnumerator StartPuzzle_Co()
    {
        switch (currentPuzzleState)
        {
            case EPuzzleState.Wait:
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
                break;

            case EPuzzleState.Match:
                tileMapComponent.MatchTileList();
                break;

            case EPuzzleState.Effect:
                break;

            case EPuzzleState.Spawn:
                break;
        }
        yield return new WaitForSeconds(1 / fps);
    }
    #endregion
}
