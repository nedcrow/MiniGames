using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Match3UIManager : UIManager
{
    #region instance
    private static Match3UIManager _instance;
    public static Match3UIManager instance
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
                        obj.AddComponent<Match3UIManager>();
                    }
                    return obj.GetComponent<Match3UIManager>();
                }
                else
                {
                    return _instance;
                }
            }
        }
    }
    #endregion

    public BrushButtonsWidget brushButtonsWidget;

    Match3TileMapComponent tileMapComponent;

    GameObject saveButton;
    GameObject saveButtonInPanel;
    GameObject exitButton_FileName;
    GameObject saveAsButton;    
    GameObject loadButton;
    Button loadButtonInPanel;
    public GameObject fileViewer;
    public GameObject fileNamePanel;

    protected override void Start()
    {
        base.Start();

        // Match3 properties
        tileMapComponent = PuzzleManager.instance.tileMapComponent;

        // Brush widget
        brushButtonsWidget = mainCanvas.gameObject.AddComponent<BrushButtonsWidget>();

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

        editModeButton.GetComponent<Button>().onClick.AddListener(OnClickedEditButton);
        playModeButton.GetComponent<Button>().onClick.AddListener(OnClickedPlayButton);
    }

    protected override void OnClickedEditButton()
    {
        base.OnClickedEditButton();
        brushButtonsWidget.ActiveBrushButtons();
    }

    protected override void OnClickedPlayButton()
    {
        base.OnClickedPlayButton();
        brushButtonsWidget.ActiveBrushButtons();
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

