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


    // Start is called before the first frame update
    public EGravity currentPuzzleGravity = EGravity.Down;
    public List<List<EGravity>> gravityMatrix = new List<List<EGravity>>();
    public Material[] tileMaterials;
    public TileMapComponent tileMapComponent = null;
    [SerializeField]
    private float tileScale_editor = 1.0f;
    [SerializeField]
    private float tileDistance_editor = 1.0f;

    void Start()
    {
        if(tileMapComponent) UpdateTileMapComponent(
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
        tileMapComponent.UpdateTileMap(tileMapComponent.tileMapSize.x, tileMapComponent.tileMapSize.y);
        tileMapComponent.tileScaleUnit = tileScale_editor = tileScale;
        tileMapComponent.tileDistanceUnit = tileDistance_editor = tileDistance;
        tileMapComponent.UpdateTileMap(mapSizeValue.x, mapSizeValue.y);
    }
}
