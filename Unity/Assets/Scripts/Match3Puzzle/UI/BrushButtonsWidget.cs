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
            brushButton.AddComponent<Button>().onClick.AddListener(() => { selectedBrush = brushButton; });

            brushButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(10.0f, 10.0f + (type.index * brushSize.y * 0.5f));
            brushButton.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            brushButton.GetComponent<RectTransform>().anchorMax = Vector2.zero;
            brushButton.GetComponent<RectTransform>().pivot = brushPivot;
            brushButton.GetComponent<RectTransform>().sizeDelta = brushSize;

            brushButtonList.Add(brushButton);
        }
    }
}
