
using UnityEngine;
using UnityEngine.UI;

public class SLUIManager : UIManager
{
    #region instance
    private static SLUIManager _instance;
    public static SLUIManager instance
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
                        obj.AddComponent<SLUIManager>();
                    }
                    return obj.GetComponent<SLUIManager>();
                }
                else
                {
                    return _instance;
                }
            }
        }
    }
    #endregion

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnClickedEditButton()
    {
        base.OnClickedEditButton();
        SLManager.instance.UpdateTileMap();
    }

    protected override void OnClickedPlayButton()
    {
        base.OnClickedPlayButton();
        SLManager.instance.UpdateTileMap();
    }

}
