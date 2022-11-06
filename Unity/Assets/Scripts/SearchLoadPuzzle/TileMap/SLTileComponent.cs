using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SLTileComponent : TileComponent
{
    [Header("Match3 properties")]
    [SerializeField]
    protected List<ESLTileType> stackedTypeList = new List<ESLTileType>();
    protected List<Material> stackedMaterialList = new List<Material>();
    protected int currentTypeIndex = 0;

    public bool BreakTile()
    {
        // Break effect

        // Change tile material
        currentTypeIndex += 1;
        if(currentTypeIndex < stackedMaterialList.Count)
        {
            gameObject.GetComponent<MeshRenderer>().material = stackedMaterialList[currentTypeIndex];
            return false;
        }

        gameObject.SetActive(false);
        return true;
    }
}
