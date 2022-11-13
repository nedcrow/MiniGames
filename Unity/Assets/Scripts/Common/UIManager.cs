using UnityEngine;
using UnityEngine.UI;

public enum EStateOfUI
{
    edit = 0,
    play = 1,
    pause = 2
}

public class UIManager : MonoBehaviour
{
    public GameObject mainCanvas;
    public EStateOfUI currentState;
    protected GameObject editModeButton;
    protected GameObject playModeButton;

    protected virtual void Start()
    {
        // Set Common Properties
        mainCanvas = GameObject.Find("Canvas");

        // Mode buttons
        editModeButton = GameObject.Find("EditModeButton");
        playModeButton = GameObject.Find("PlayModeButton");
        editModeButton.GetComponent<Button>().onClick.AddListener(OnClickedEditButton);
        playModeButton.GetComponent<Button>().onClick.AddListener(OnClickedPlayButton);
        playModeButton.SetActive(false);

        OnClickedEditButton();
    }

    protected virtual void OnClickedEditButton()
    {
        editModeButton.SetActive(false);
        playModeButton.SetActive(true);
        currentState = EStateOfUI.edit;
    }

    protected virtual void OnClickedPlayButton()
    {
        editModeButton.SetActive(true);
        playModeButton.SetActive(false);
        currentState = EStateOfUI.play;
    }
}
