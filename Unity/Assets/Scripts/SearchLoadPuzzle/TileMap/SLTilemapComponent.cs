using System.Collections;
using System.Collections.Generic;
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
            Debug.LogError("�� ������� ¦��������.");
            return;
        }

        if(useMaterialList.Count < 1)
        {
            Debug.LogError("����� ��Ƽ���� ����.");
            return;
        }

        base.UpdateTileMap(sizeX, sizeY);
        int loopCount = (int)(tileObjectList.Count * 0.5f);        
        for (int i=0; i< loopCount; i++)
        {
            List<Material> tempMaterialList = new List<Material>();
            int randomIndexA = Random.Range(0, useMaterialList.Count);
            tempMaterialList.Add(useMaterialList[randomIndexA]);
            tileObjectList[i].GetComponent<MeshRenderer>().material = useMaterialList[randomIndexA];
        }

        for (int i = loopCount; i < tileObjectList.Count; i++)
        {
            List<Material> tempMaterialList = new List<Material>();
            int randomIndexB = Random.Range(0, tempMaterialList.Count);
            tileObjectList[i].GetComponent<MeshRenderer>().material = tempMaterialList[randomIndexB];
            tempMaterialList.RemoveAt(randomIndexB);
        }

    }
}
