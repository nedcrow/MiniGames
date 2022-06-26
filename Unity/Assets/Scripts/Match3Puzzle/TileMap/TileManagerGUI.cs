using UnityEditor;
using UnityEngine;

public class TileManagerGUI : EditorWindow
{
    [Min(1)]
    public Vector2Int mapSizeValue = new Vector2Int(1, 1);
    public float tileScaleUnit = 1.0f;
    public float tileDistanceUnit = 1.0f;

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

        tileScaleUnit = EditorGUILayout.FloatField("tileScaleUnit", tileScaleUnit);
        tileDistanceUnit = EditorGUILayout.FloatField("tileDistanceUnit", tileDistanceUnit);

        // End the code block and update the label if a change occurred
        if (EditorGUI.EndChangeCheck())
        {
            PuzzleManager puzzleManager = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>();
            if(puzzleManager != null) puzzleManager.UpdateTileMapComponent(mapSizeValue, tileScaleUnit, tileDistanceUnit);
        }
    }
}