using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    TileMapComponent tileMapComponent;
    void Start()
    {
        GameObject.Find("TestButton").GetComponent<Button>().onClick.AddListener(OnClickedTestButton);

        tileMapComponent = GameObject.Find("PuzzleManager").GetComponent<TileMapComponent>();
    }

    void OnClickedTestButton()
    {
        if (tileMapComponent.tileList.Count() > 0)
        {
            tileMapComponent.tileList[0][0].GetComponent<TileComponent>().MoveTo(new Vector2Int(-1, -1));
        }
    }
}

