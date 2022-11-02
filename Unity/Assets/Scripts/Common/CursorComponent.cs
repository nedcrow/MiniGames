using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class CursorComponent : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler
{
    #region Event
    public delegate void Del_SelectdTile(GameObject tileObj); 
    public event Del_SelectdTile SelectedTileEvent;

    #endregion

    public Vector3 DefaultPosition = Vector3.one * -99;
    public bool canSelect = true;
    
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

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Mouse Down: " + eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse Enter");
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

        
        if (Input.GetMouseButtonUp(0))
        {
            if (canSelect) transform.position = Hit.collider.gameObject.transform.position;
            SelectedTileEvent(Hit.collider.gameObject);
            Debug.Log("current selected tile is " + Hit.collider.gameObject.name);
        }



        //if (PuzzleManager.instance.tileMapComponent.HasMovingTile()) return;


        //Match3TileComponent tileComponent = Hit.collider.gameObject.GetComponent<Match3TileComponent>();
        bool pressedCtr = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        //if (Input.GetMouseButtonUp(0))
        //{
        //    if (pressedCtr)
        //    {
        //        bool drawnTiles = UIManager.instance.brushButtonsWidget.DrawTiles(tileComponent);
        //        if (drawnTiles) transform.position = tileComponent.gameObject.transform.position;
        //    }
        //}

        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (!pressedCtr)
        //    {
        //        bool updatedCurrentTile = PuzzleManager.instance.UpdateCurrentSelectedTile(tileComponent);
        //        if (updatedCurrentTile) transform.position = tileComponent.gameObject.transform.position;
        //        if (!hitTile) transform.position = DefaultPosition;
        //    }
        //}

        //if (Input.GetMouseButton(0))
        //{
        //    if (!pressedCtr)
        //    {
        //        bool drawnTile = UIManager.instance.brushButtonsWidget.DrawTile(tileComponent);
        //        if (drawnTile) transform.position = tileComponent.gameObject.transform.position;
        //    }
        //}
    }
}
