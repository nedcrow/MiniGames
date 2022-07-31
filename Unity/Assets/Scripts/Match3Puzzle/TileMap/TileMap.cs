using System.Collections.Generic;

public class Size2D
{
    public Size2D(int x=1, int y = 1)
    {
        this.x = x; 
        this.y = y;
    }
    public int x = 1;
    public int y = 1;
}

public class Tile
{
    public int type = -1;
    public int[] position = new int[2];
}

public class TileMap
{
    public string id = "noname";
    public string description = "description";
    public int[] mapSize = new int[2] {1, 1};

    public List<Tile> tileList = new List<Tile>();

    public void SetMapSize(int x, int y)
    {
        mapSize[0] = x;
        mapSize[1] = y;
    }
    public Size2D GetMapSize()
    {
        return new Size2D(mapSize[0], mapSize[1]);
    }
}