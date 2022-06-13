using UnityEditor;
using UnityEngine;

public class TileManagerGUI : EditorWindow
{
    [Min(1)]
    public Vector2Int mapSizeValue = new Vector2Int(1, 1);

    [MenuItem("Window/TileManagerGUI")]
    static void Init()
    {
        var example = (TileManagerGUI)TileManagerGUI.GetWindow(typeof(TileManagerGUI));
        example.Show();
    }

    void OnGUI()
    {
        // Start a code block to check for GUI changes
        EditorGUI.BeginChangeCheck();

        mapSizeValue = EditorGUILayout.Vector2IntField("mapSizeValue", mapSizeValue);

        // End the code block and update the label if a change occurred
        if (EditorGUI.EndChangeCheck())
        {
            TileCreator tileCreator = GameObject.Find("PuzzleManager").GetComponent<TileCreator>();
            tileCreator.SetTilleMapSize(mapSizeValue.x, mapSizeValue.y);
            tileCreator.UpdateTileMap(mapSizeValue.x, mapSizeValue.y);
        }
    }
}