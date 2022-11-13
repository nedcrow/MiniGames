using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[ExecuteInEditMode]
public class SLTilemapComponent : TileMapComponent
{
    [Space(10)]
    [Header("SLTilemap properties")]
    public List<GameObject> garbageList;
    public int[,] tileIndexes;

    [SerializeField]
    public List<Material> useMaterialList = new List<Material>();
    public List<ESLTileType> useTypeList = new List<ESLTileType>();
    
    public override void UpdateTileMap(int sizeX, int sizeY)
    {
        if((sizeX * sizeY)%2 != 0)
        {
            Debug.LogError("맵 사이즈는 짝수여야함.");
            return;
        }

        if(useMaterialList.Count < 1)
        {
            Debug.LogError("사용할 머티리얼 부족.");
            return;
        }

        base.UpdateTileMap(sizeX, sizeY);
        int loopCount = (int)(tileObjectList.Count * 0.5f);

        List<int> materialIndexList = new List<int>();
        for (int i=0; i< loopCount; i++)
        {
            List<Material> tempMaterialList = new List<Material>();
            int randomIndexA = Random.Range(0, useMaterialList.Count-1);
            tempMaterialList.Add(useMaterialList[randomIndexA]);
            materialIndexList.Add(randomIndexA);
            tileObjectList[i].GetComponent<MeshRenderer>().material = useMaterialList[randomIndexA];
        }

        materialIndexList.Sort((a, b) => Random.Range(0.0f,1.0f) > 0.5f ? 1 : -1);

        for (int i = 0; i < tileObjectList.Count*0.5f; i++)
        {
            List<Material> tempMaterialList = new List<Material>();
            int randomIndexB = materialIndexList[i];
            tempMaterialList.Add(useMaterialList[randomIndexB]);
            tileObjectList[loopCount+i].GetComponent<MeshRenderer>().material = useMaterialList[randomIndexB];
        }

    }
}
