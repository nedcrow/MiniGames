using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushButtonsWidget : MonoBehaviour
{
    public List<GameObject> brushButtonList = new List<GameObject>();
    public GameObject selectedBrush = null;

    public void ActiveBrushButtons()
    {
        if (brushButtonList.Count == 0)
        {
            DrawBrushButtons();
            return;
        }

        foreach (GameObject brushButton in brushButtonList) brushButton.SetActive(true);
    }

    public void HideBrushButtons()
    {
        if (brushButtonList.Count == 0) DrawBrushButtons();

        foreach (GameObject brushButton in brushButtonList) brushButton.SetActive(false);
    }

    public bool DrawTiles(Match3TileComponent currentTileComponent)
    {
        if (selectedBrush == null) return false;

        M3TileMapComponent tileMapComponent = PuzzleManager.instance.tileMapComponent;
        Match3TileComponent oldSelectedTileComponent = PuzzleManager.instance.selectedTileObj.GetComponent<Match3TileComponent>();
        List<GameObject> tileList = tileMapComponent.tileObjectList;
        List<List<int>> tileIndexList = tileMapComponent.tileIndexesList;

        Vector2Int startIndexes = oldSelectedTileComponent.tileLocation;
        Vector2Int endIndexes = currentTileComponent.tileLocation;
        Vector2Int distance = new Vector2Int(
            Mathf.Abs(endIndexes.x - startIndexes.x)+1,
            Mathf.Abs(endIndexes.y - startIndexes.y)+1
            );

        bool isWide = distance.x >= distance.y;
        int countOfLoop = isWide ? distance.x : distance.y;
        float tangent = isWide ?
            Mathf.Floor(100.0f * distance.y / distance.x) * 0.01f :
            Mathf.Floor(100.0f * distance.x / distance.y) * 0.01f;
        float stackedTangent = 0;
        float limitTangent = 1;

        int index_H = 0;
        int index_V = 0;
        int dir_h = startIndexes.x > endIndexes.x ? -1 : 1;
        int dir_v = startIndexes.y > endIndexes.y ? -1 : 1;

        int startX = startIndexes.x;
        int startY = startIndexes.y;
        for (int i = 0; i < countOfLoop; i++)
        {
            int indexX = startX + ((isWide ? i : index_H) * dir_h);
            int indexY = startY + ((isWide ? index_V : i) * dir_v);
            int tileIndex = tileIndexList[indexX][indexY];

            if (isWide && tangent == 1)
            {
                DrawTile(tileList[tileIndex].GetComponent<Match3TileComponent>());
                index_V += 1;
            }
            else
            {
                stackedTangent += tangent;
                DrawTile(tileList[tileIndex].GetComponent<Match3TileComponent>());

                if (Mathf.Floor(stackedTangent) == limitTangent)
                {
                    if (isWide) index_V += 1;
                    if (!isWide) index_H += 1;
                    if (stackedTangent != limitTangent)
                    {
                        DrawTile(tileList[tileIndex].GetComponent<Match3TileComponent>());
                    }
                    stackedTangent = 0;
                }
            }
        }

        return true;
    }

    public bool DrawTile(Match3TileComponent currentTileComponent)
    {
        if (selectedBrush == null) return false;

        ETileType tileType = ETileType.None;
        try
        {
            tileType = (ETileType)System.Convert.ToInt32((selectedBrush.name.Split("_")[1]));
        }
        catch
        {
            Debug.LogError("TileTypeError: " + selectedBrush.name);
            return false;
        }

        PuzzleManager.instance.UpdateCurrentSelectedTile(currentTileComponent);
        currentTileComponent.ChangeTileType(tileType);

        return true;
    }

    void DrawBrushButtons()
    {
        List<ETileType> tileTypeList = new List<ETileType>(System.Enum.GetValues(typeof(ETileType)).OfType<ETileType>());
        Sprite[] spriteArr = Resources.LoadAll<Sprite>("Sprites/Brushes");
        Vector2 brushSize = new Vector2(40f, 40f);
        Vector2 brushPivot = new Vector2(0.0f, 0.0f);

        if (spriteArr.Length <= 0)
        {
            Debug.Log("Found not brush sprites: Asset/Resources/Sprites/Brushes");
            return;
        }

        foreach (var type in tileTypeList.Select((value, index) => (value, index)))
        {
            GameObject brushButton = new GameObject();
            brushButton.name = "BrushButton_" + type.index.ToString();
            brushButton.transform.SetParent(UIManager.instance.mainCanvas.transform);

            brushButton.AddComponent<Image>().sprite = spriteArr[0];
            brushButton.AddComponent<Button>().onClick.AddListener(() =>
            {
                selectedBrush = selectedBrush == brushButton ? null : brushButton;
            });

            brushButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(10.0f, 10.0f + (type.index * brushSize.y * 0.5f));
            brushButton.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            brushButton.GetComponent<RectTransform>().anchorMax = Vector2.zero;
            brushButton.GetComponent<RectTransform>().pivot = brushPivot;
            brushButton.GetComponent<RectTransform>().sizeDelta = brushSize;

            brushButtonList.Add(brushButton);
        }
    }
}
