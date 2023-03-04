using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SLTileComponent : TileComponent
{
    [Header("SearchTile properties")]
    [SerializeField]
    //protected List<ESLTileType> stackedTypeList = new List<ESLTileType>();
    //protected List<Material> stackedMaterialList = new List<Material>();
    protected List<SLTileType> currentTypeList = new List<SLTileType>();
    protected int currentHP = 1;

    public bool BreakTile()
    {
        // Break effect

        // Change tile material
        currentHP -= 1;
        if(currentHP > 0)
        {
            SetCurrenTileType(
                currentTypeList[currentHP - 1].type,
                currentTypeList[currentHP - 1].material
                );
            return false;
        }

        gameObject.SetActive(false);
        return true;
    }

    public void ResetTileTypes(List<SLTileType> typeList)
    {
        currentTypeList = typeList;
        currentHP = typeList.Count;
        SetCurrenTileType(
            currentTypeList[currentHP - 1].type,
            currentTypeList[currentHP - 1].material
            );
    }

    //public void ResetTileTypes(List<ESLTileType> typeList, List<Material> matList)
    //{
    //    stackedTypeList = typeList;
    //    stackedMaterialList = matList;
    //    currentHP = typeList.Count;
    //    SetCurrenTileType(stackedTypeList[currentHP - 1], stackedMaterialList[currentHP - 1]);
    //}

    public void SetCurrenTileType(ESLTileType type, Material mat)
    {
        gameObject.GetComponent<MeshRenderer>().material = mat;
        SetTileTypeName(type.ToString());
    }
}
