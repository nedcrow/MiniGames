using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SLManager : MonoBehaviour
{
    #region instance
    private static SLManager _instance;
    public static SLManager instance
    {
        get
        {
            {
                if (_instance == null)
                {
                    GameObject obj = GameObject.Find("SLManager");
                    if (obj == null)
                    {
                        obj = new GameObject("SLManager");
                        obj.AddComponent<SLManager>();
                    }
                    return obj.GetComponent<SLManager>();
                }
                else
                {
                    return _instance;
                }
            }
        }
    }
    #endregion

    [SerializeField]
    public GameObject tilemapCreatorObj;

    void Start()
    {
        tilemapCreatorObj = GameObject.Find("TilemapCreator");
        if(tilemapCreatorObj == null)
        {
            tilemapCreatorObj = new GameObject("TilemapCreator");
            tilemapCreatorObj.AddComponent<SLTilemapComponent>();
        }
        tilemapCreatorObj.transform.SetParent(gameObject.transform);

        tilemapCreatorObj.GetComponent<SLTilemapComponent>().CreateTileMapAt(tilemapCreatorObj);
    }
}
