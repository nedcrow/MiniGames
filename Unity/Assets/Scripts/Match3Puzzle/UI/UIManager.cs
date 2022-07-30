using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region instance
    private static UIManager _instance;
    public static UIManager instance
    {
        get
        {
            {
                if (_instance == null)
                {
                    GameObject obj = GameObject.Find("Canvas");
                    if (obj == null)
                    {
                        obj = new GameObject("Canvas");
                        obj.AddComponent<UIManager>();
                    }
                    return obj.GetComponent<UIManager>();
                }
                else
                {
                    return _instance;
                }
            }
        }
    }
    #endregion

    public GameObject mainCanvas;
    public BrushButtonsWidget brushButtonsWidget;

    TileMapComponent tileMapComponent;
    GameObject editModeButton;
    GameObject playModeButton;

    void Start()
    {
        tileMapComponent = GameObject.Find("PuzzleManager").GetComponent<TileMapComponent>();
        mainCanvas = GameObject.Find("Canvas");
        brushButtonsWidget = mainCanvas.AddComponent<BrushButtonsWidget>();
        editModeButton = GameObject.Find("EditModeButton");
        playModeButton = GameObject.Find("PlayModeButton");

        editModeButton.GetComponent<Button>().onClick.AddListener(OnClickedEditButton);
        playModeButton.GetComponent<Button>().onClick.AddListener(OnClickedPlayButton);
        playModeButton.SetActive(false);
    }

    void OnClickedEditButton()
    {
        editModeButton.SetActive(false);
        playModeButton.SetActive(true);
        brushButtonsWidget.ActiveBrushButtons();

        //if (tileMapComponent.tileList.Count() > 0)
        //{
        //    tileMapComponent.tileList[0][0].GetComponent<TileComponent>().MoveTo(new Vector2Int(-1, -1));
        //}
    }

    void OnClickedPlayButton()
    {
        editModeButton.SetActive(true);
        playModeButton.SetActive(false);
        brushButtonsWidget.HideBrushButtons();
    }
}

