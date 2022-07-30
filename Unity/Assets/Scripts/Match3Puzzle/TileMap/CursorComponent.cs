using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CursorComponent : MonoBehaviour
{
    public Vector3 DefaultPosition = Vector3.one * -99;
    public bool usedBrush = false;

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

        bool hitTile = Physics.Raycast(ray, out Hit) && Hit.collider.gameObject.CompareTag("Tile");

        // SelectTile
        if (Input.GetMouseButtonDown(0))
        {
            if (hitTile)
            {
                TileComponent tile = Hit.collider.gameObject.GetComponent<TileComponent>();
                UpdateCurrentSelectedTile(tile);
            }
            else
            {
                transform.position = DefaultPosition;
            }
        }

        // DrawTile
        if (Input.GetMouseButtonDown(0) && UIManager.instance.brushButtonsWidget.selectedBrush != null)
        {
            usedBrush = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            usedBrush = false;
        }

        if (usedBrush)
        {
            if (hitTile)
            {
                TileComponent tile = Hit.collider.gameObject.GetComponent<TileComponent>();
                UpdateCurrentSelectedTile(tile);
                DrawTile((ETileType)System.Convert.ToInt32(UIManager.instance.brushButtonsWidget.selectedBrush.name.Split("_")[1]));
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
            (oldTileGameObj ? oldTileGameObj.transform.position : "(Null position)") +
            " -> " +
            currentTile.currentType.ToString() +
            pos
            );

        PuzzleManager.instance.GetComponent<TileMapComponent>().currentSelectedTile = currentTile.gameObject;
        // 현재 게임 모드가 에디트 모드일 때와 플레이 모드일 때 구분

    }

    private void DrawTile(ETileType tileType)
    {
        bool isNullBrush = UIManager.instance.brushButtonsWidget.selectedBrush == null;
        if (isNullBrush) return;

        TileComponent tileComponent = PuzzleManager.instance.GetComponent<TileMapComponent>().currentSelectedTile.GetComponent<TileComponent>();
        if (!tileComponent) return;

        tileComponent.ChangeTileType(tileType);

    }
}
