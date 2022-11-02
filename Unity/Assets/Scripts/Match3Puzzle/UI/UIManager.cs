using System.Linq;
using TMPro;
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

    M3TileMapComponent tileMapComponent;
    GameObject editModeButton;
    GameObject playModeButton;
    GameObject saveButton;
    GameObject saveButtonInPanel;
    GameObject exitButton_FileName;
    GameObject saveAsButton;    
    GameObject loadButton;
    Button loadButtonInPanel;
    public GameObject fileViewer;
    public GameObject fileNamePanel;

    void Start()
    {
        // Set Common Properties
        mainCanvas = GameObject.Find("Canvas");
        tileMapComponent = PuzzleManager.instance.tileMapComponent;

        // Brush widget
        brushButtonsWidget = mainCanvas.AddComponent<BrushButtonsWidget>();

        // Mode buttons
        editModeButton = GameObject.Find("EditModeButton");
        playModeButton = GameObject.Find("PlayModeButton");
        editModeButton.GetComponent<Button>().onClick.AddListener(OnClickedEditButton);
        playModeButton.GetComponent<Button>().onClick.AddListener(OnClickedPlayButton);
        playModeButton.SetActive(false);

        // Save/Load buttons
        saveButton = GameObject.Find("SaveButton");
        saveButtonInPanel = GameObject.Find("SaveButtonInPanel");
        exitButton_FileName = GameObject.Find("ExitButton_FileName");
        saveAsButton = GameObject.Find("SaveAsButton");
        loadButton = GameObject.Find("LoadButton");
        loadButtonInPanel = GameObject.Find("SubmitButton").GetComponent<Button>();
        fileViewer = GameObject.Find("SimpleFileBrowserCanvas");
        fileNamePanel = GameObject.Find("FileNamePanel");
        
        saveButton.GetComponent<Button>().onClick.AddListener(OnClickedSaveButton);
        saveButtonInPanel.GetComponent<Button>().onClick.AddListener(OnClickedSaveButtonInPanel);
        exitButton_FileName.GetComponent<Button>().onClick.AddListener(OnClickedExitPanelButton);
        saveAsButton.GetComponent<Button>().onClick.AddListener(OnClickedSaveAsButton);
        loadButton.GetComponent<Button>().onClick.AddListener(OnClickedLoadButton);
        loadButtonInPanel.onClick.AddListener(OnClickedLoadButtonInPanel);
        fileViewer.SetActive(false);
        fileNamePanel.SetActive(false);

        SimpleFileBrowser.FileBrowser.SetFilters(true, ".map");
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

    void OnClickedSaveButton() {
        // 일시정지 
        bool succeedSave = tileMapComponent.Save();
        if (succeedSave) return;
        OnClickedSaveAsButton();
    }

    void OnClickedSaveButtonInPanel()
    {
        GameObject inputTextMesh = GameObject.Find("InputFileNameText");
        if (inputTextMesh == null)
        {
            Debug.LogWarning("Null inputTextMesh.");
            return;
        }

        TMP_Text textMeshComponent = inputTextMesh.GetComponent<TMP_Text>();
        Debug.Log(textMeshComponent.text);
        
        tileMapComponent.currentTileMapID = textMeshComponent.text;
        OnClickedSaveButton();
        OnClickedExitPanelButton();
    }

    void OnClickedExitPanelButton()
    {
        fileNamePanel.SetActive(false);
    }

    void OnClickedSaveAsButton()
    {
        // 일시정지
        fileNamePanel.SetActive(true);
    }

    void OnClickedLoadButton() {
        
        if (fileViewer != null)
        {
            fileViewer.SetActive(true);
            SimpleFileBrowser.FileBrowser fileBrowser = fileViewer.GetComponent<SimpleFileBrowser.FileBrowser>();
            fileBrowser.OnPathChanged(Application.dataPath + "/Save");
        }        
    }

    void OnClickedLoadButtonInPanel()
    {
        if (SimpleFileBrowser.FileBrowser.Result == null) return;
        tileMapComponent.LoadTileMap_JSON(SimpleFileBrowser.FileBrowser.Result[0]);
    }


}

