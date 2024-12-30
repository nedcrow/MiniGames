using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CCManager : MonoBehaviour
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
                    GameObject obj = GameObject.Find("CCManager");
                    if (obj == null)
                    {
                        obj = new GameObject("CCManager");
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
}
