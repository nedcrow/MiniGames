using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileSpawnerList : MonoBehaviour
{
    public Vector2Int currentPosition;

    List<GameObject> activatedTileSpawnerList = new List<GameObject>();
    List<GameObject> restedTileSpawnerList = new List<GameObject>();

    public void CreateSpawnerGameObjects(int sizeX, int sizeY, float tileDistanceUnit = 1.0f, float tileScaleUnit = 1.0f)
    {
        int distance_a = sizeX - activatedTileSpawnerList.Count;
        int distance_r = 0;
        
        if(distance_a > 0)
        {
            distance_r = distance_a - restedTileSpawnerList.Count;
            if (distance_r > 0)
            {
                GameObject spawner = GameObject.CreatePrimitive(PrimitiveType.Plane);
                for (int i = 0; i < distance_r; i++) restedTileSpawnerList.Add(spawner);
            }
            for(int i = 0; i < distance_a; i++) activatedTileSpawnerList.Add(restedTileSpawnerList[0]);
        }
        else
        {
            for (int i = 0; i < distance_a * -1; i++) restedTileSpawnerList.Add(activatedTileSpawnerList[0]);
        }

        foreach (var spawner in activatedTileSpawnerList.Select((value, index) => (value, index)))
        {
            TileSpawnerComponent tileSpawnerComponent = spawner.value.GetComponent<TileSpawnerComponent>();
            if (tileSpawnerComponent == null) tileSpawnerComponent = spawner.value.AddComponent<TileSpawnerComponent>();
            tileSpawnerComponent.gameObject.transform.position = new Vector3(spawner.index, sizeY, .0f) * tileDistanceUnit;
            tileSpawnerComponent.gameObject.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
            tileSpawnerComponent.gameObject.transform.localScale = Vector3.one * tileScaleUnit;

            // material
            //tileSpawnerComponent.gameObject.GetComponent<MeshRenderer>().material = 
        }


    }

    public void SpawnTiles(GameObject tileList, ETileType[] usingTypes, float tileDistanceUnit=1.0f, float tileScaleUnit = 1.0f)
    {
        foreach(GameObject spawner in activatedTileSpawnerList)
        {
            TileSpawnerComponent tileSpawnerComponent = spawner.GetComponent<TileSpawnerComponent>();
            //spawner.SpawnTileAt(usingTypes);
        }

        
    }
}
