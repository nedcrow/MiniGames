using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class SLTilemapComponent : TileMapComponent
{
    [Space(10)]
    [Header("SLTilemap properties")]
    public List<GameObject> garbageList;
    public int[,] tileIndexes;

    [SerializeField]
    public List<SLTileType> specialTileTypeList = new List<SLTileType>();
    [SerializeField]
    public List<SLTileType> normalTileTypeList = new List<SLTileType>();

    [SerializeField]
    List<string> DBTileList = new List<string>();

    public int maxDepth = 1;
    public int specialTypeCount = 0;
    public int commonTypeCount_max = 0;

    public void UpdateSLTileMap(int sizeX, int sizeY, string mapID="") {
        if ((sizeX * sizeY) % 2 != 0)
        {
            Debug.LogError("�� ������� ¦��������.");
            return;
        }

        if (mapID != "")
        {
            // �μ� �� �ִ� ��� Ÿ�� ������ ¦��������.
        }

        currentMapSize = new Vector2Int(sizeX, sizeY);

        // �ش� �ʿ� ���� ��Ƽ����� Ÿ���� DataTable���� ������
        //DBTileList = new List<string>(); // DB �迭 ��������

        base.UpdateTileMap(sizeX, sizeY);

        int tileCount = sizeX * sizeY;
        List<string> SLTileTypeList_str = System.Enum.GetNames(typeof(ESLTileType)).ToList();
        if (mapID == "") // �Ǵ� DB �迭 ���̰� 1 �̸��� ��� �߰� 
        {
            // basic mode - all rendom
            DBTileList = Enumerable.Repeat("1", tileCount).ToList();
            commonTypeCount_max = tileCount;
        }
        else
        {
            // �迭 �� layer ������ 1�̻��� Ÿ���� �ϳ��� �ִ� ��� maxDepth Ȯ��            
            for (int i = 0; i < DBTileList.Count; i++)
            {
                string depth = DBTileList[i];
                int depth_int = 0;
                bool isNumber = int.TryParse(depth, out depth_int);

                if (isNumber)
                {
                    commonTypeCount_max += depth_int;
                    maxDepth = depth_int > maxDepth ? depth_int : maxDepth;
                }
                else
                {
                    DBTileList[i] = "1";
                    specialTypeCount += SLTileTypeList_str.Contains(depth) ? 1 : 0;                    
                }
            }
        }

        // Random type
        List<SLTileType> randomIndexList = new List<SLTileType>();
        for(int i=0; i< commonTypeCount_max/2; i++)
        {
            int index = UnityEngine.Random.Range(0, normalTileTypeList.Count);
            randomIndexList.Add(normalTileTypeList[index]);
        }

        // TypeList_perTile
        List<List<SLTileType>> typeList_perTile = new List<List<SLTileType>>();
        int commonTypeCount_current = 0;
        foreach (string tileData in DBTileList)
        {
            typeList_perTile.Add(new List<SLTileType>());

            int lastIndex = typeList_perTile.Count - 1;
            int tileData_int = 0;
            bool isNumber = int.TryParse(tileData, out tileData_int);            
            if (isNumber)
            {
                if(commonTypeCount_current == commonTypeCount_max / 2)
                {
                    randomIndexList.OrderBy(_ => UnityEngine.Random.Range(0f, 1.0f)).ToList();
                    commonTypeCount_current = 0;
                }
                for (int i=0; i< tileData_int; i++)
                {
                    typeList_perTile[lastIndex].Add(randomIndexList[commonTypeCount_current]);
                    commonTypeCount_current++;
                }
            }
            else
            {
                // special type                
                SLTileType tileType = specialTileTypeList.Find(type => type.name == tileData);
                typeList_perTile[lastIndex].Add(tileType);
            }
        }

        // Reset Tiles
        for (int i = 0; i < typeList_perTile.Count; i++)
        {
            tileObjectList[i].GetComponent<SLTileComponent>().ResetTileTypes(typeList_perTile[i]);
        }
        Debug.Log("Complete");
    }
}
