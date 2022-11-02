using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RestedTilles
{
    private List<GameObject> restedTileList = new List<GameObject>();
    private List<GameObject> activatedTileList = new List<GameObject>();
    public int GetCount() { return restedTileList.Count() + activatedTileList.Count(); }
    public void AddNewTile()
    {
        GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Plane);
        tile.AddComponent<Match3TileComponent>();
        tile.tag = "Tile";
        tile.transform.SetParent(GameObject.Find("PuzzleManager").transform);
        restedTileList.Add(tile);
        restedTileList[restedTileList.Count() - 1].SetActive(false);

    }

    public GameObject ActiveTile()
    {
        GameObject restedTile = restedTileList[restedTileList.Count - 1];
        restedTile.SetActive(true);
        activatedTileList.Add(restedTile);
        restedTileList.RemoveAt(restedTileList.Count() - 1);

        return restedTile;
    }

    public void RestTile(GameObject tile)
    {
        if (tile == null) return;
        restedTileList.Add(tile);
        restedTileList[restedTileList.Count - 1].SetActive(false);
        activatedTileList.RemoveAt(activatedTileList.IndexOf(tile));
    }

    public bool IsGotTile(GameObject tile)
    {
        bool isRestedTile = restedTileList.IndexOf(tile) >= 0 ? true : false;
        bool isActivatedTile = activatedTileList.IndexOf(tile) >= 0 ? true : false;
        return isRestedTile || isActivatedTile;
    }
}
