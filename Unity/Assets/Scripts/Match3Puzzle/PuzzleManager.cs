using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float tileMapUnit = 1;
    public Material[] tileMaterials;
    void Start()
    {
        TileCreator tileCreator = GetComponent<TileCreator>();
        GameObject.Find("MainCamera").transform.localPosition = new Vector3(
            (tileCreator.GetTilleMapSize().x * 0.5f - 0.5f) * tileMapUnit,
            (tileCreator.GetTilleMapSize().y * 0.5f - 0.5f) * tileMapUnit,
            -1.0f
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
