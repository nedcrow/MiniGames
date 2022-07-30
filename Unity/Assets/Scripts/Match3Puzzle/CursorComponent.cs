using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CursorComponent : MonoBehaviour
{
    public Vector3 DefaultPosition = Vector3.one * -99;

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
        RaycastHit Hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool hitTile = Physics.Raycast(ray, out Hit) && Hit.collider.gameObject.CompareTag("Tile");
        if (!hitTile) return;

        TileComponent tileComponent = Hit.collider.gameObject.GetComponent<TileComponent>();
        bool pressedCtr = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        if (Input.GetMouseButtonUp(0))
        {            
            if (pressedCtr)
            {
                bool drawnTiles = UIManager.instance.brushButtonsWidget.DrawTiles(tileComponent);
                if (drawnTiles) transform.position = tileComponent.gameObject.transform.position;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!pressedCtr)
            {
                bool updatedCurrentTile = PuzzleManager.instance.UpdateCurrentSelectedTile(tileComponent);
                if (updatedCurrentTile) transform.position = tileComponent.gameObject.transform.position;
                if (!hitTile) transform.position = DefaultPosition;
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (!pressedCtr)
            {
                bool drawnTile = UIManager.instance.brushButtonsWidget.DrawTile(tileComponent);
                if (drawnTile) transform.position = tileComponent.gameObject.transform.position;
            }
        }
    }
}
