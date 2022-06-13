using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    // Start is called before the first frame update
    public EGravity currentPuzzleGravity = EGravity.Down;
    public Material[] tileMaterials;


    void Start()
    {
        TileCreator tileCreator = GetComponent<TileCreator>();
        tileCreator.UpdateTileMap(tileCreator.tileMapSize.x, tileCreator.tileMapSize.y);

        Camera.main.transform.localPosition = new Vector3(
            (tileCreator.tileMapSize.x * 0.5f - 0.5f),
            (tileCreator.tileMapSize.y * 0.5f - 0.5f),
            -1.0f
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
