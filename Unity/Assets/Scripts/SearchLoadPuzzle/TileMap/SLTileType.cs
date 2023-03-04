using UnityEngine;

[System.Serializable]
public class SLTileType
{
    [SerializeField]
    public string name;
    public ESLTileType type;
    public Material material;

    public SLTileType(string name, ESLTileType type, Material material)
    {
        this.name = name;
        this.type = type;
        this.material = material;
    }
}
