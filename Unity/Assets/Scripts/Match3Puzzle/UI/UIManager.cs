using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    TileCreator tileCreator;
    void Start()
    {
        GameObject.Find("TestButton").GetComponent<Button>().onClick.AddListener(OnClickedTestButton);

        tileCreator = GameObject.Find("PuzzleManager").GetComponent<TileCreator>();
    }

    void OnClickedTestButton()
    {
        if (tileCreator.tileList.Count() > 0)
        {
            tileCreator.tileList[0][0].GetComponent<TileComponent>().MoveTo(new Vector2Int(-1, -1));
        }
    }
}

