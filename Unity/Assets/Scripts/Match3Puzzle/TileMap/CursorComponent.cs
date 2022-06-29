using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CursorComponent : MonoBehaviour
{
    private Vector3 DefaultPosition = Vector3.one * -99;
    void Start()
    {
        if (IsDuplicated()) return;
        
        Init();
    }

    void Init()
    {
        gameObject.tag = "Cursor";
        transform.position = DefaultPosition;
    }
    bool IsDuplicated()
    {
        int countOfCursor = GameObject.FindGameObjectsWithTag("Cursor").Length;
        if (countOfCursor > 0) return true;
        Debug.Log("To much cursor this count is " + countOfCursor);
        return false;
    }

    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out Hit) && Hit.collider.gameObject.CompareTag("Tile"))
            {
                TileComponent tile = Hit.collider.gameObject.GetComponent<TileComponent>();                
                UpdateCurrentSelectedTile(tile);
            }
            else {
                transform.position = DefaultPosition;
            }
        }
    }

    private void UpdateCurrentSelectedTile(TileComponent currentTile)
    {
        GameObject oldTileGameObj = PuzzleManager.instance.GetComponent<TileMapComponent>().currentSelectedTile;
        bool isDifferent = oldTileGameObj != currentTile.gameObject;
        if (!isDifferent) return;

        Vector3 pos = currentTile.currentTilePosition;
        transform.position = pos;

        Debug.Log(
            (oldTileGameObj ? oldTileGameObj.GetComponent<TileComponent>().currentType.ToString() : "Null") +
            (oldTileGameObj ? oldTileGameObj.transform.position : "(Null position)")+ 
            " -> " + 
            currentTile.currentType.ToString() +
            pos
            );

        PuzzleManager.instance.GetComponent<TileMapComponent>().currentSelectedTile = currentTile.gameObject;

    }
}
