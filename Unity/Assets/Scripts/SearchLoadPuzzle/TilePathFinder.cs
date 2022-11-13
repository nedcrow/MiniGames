using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PathOnTilemap
{
    public bool hasWall;
    public bool allowDiagonal;
    public int maxCountOfCorner;
    public int maxCountOfStep;
    public List<Vector2Int> points;

    public PathOnTilemap(
        bool hasWall = false,
        bool allowDiagonal = false,
        int maxCountOfCorner = -1,
        int maxCountOfStep = -1
        )
    {
        this.hasWall = hasWall;
        this.allowDiagonal = hasWall;
        this.maxCountOfCorner = maxCountOfCorner;
        this.maxCountOfStep = maxCountOfStep;
        this.points = new List<Vector2Int>();
    }
    public void addPoints(Vector2Int point) {
        if(this.points == null)
        {
            this.points = new List<Vector2Int>();
        }
        this.points.Add(point); 
    }
    public List<Vector2Int> getPoints() { return points; }
}

public class TilePathFinder : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="option"> 벽 타일 조건</param>
    public PathOnTilemap DrawPathAFromB(
        Vector2Int startLocation,
        Vector2Int endLocation,
        List<GameObject> tileList,
        bool hasWall = false,
        bool allowDiagonal = false,
        int maxCountOfCorner = -1,
        int maxCountOfStep = -1
        )
    {
        // 타입이름으로 같은 타일인지 확인 또는 특정 타일이 아닌지 확인
        if (!hasWall) maxCountOfCorner = -1;

        PathOnTilemap pathOnTilemap = new PathOnTilemap();
        pathOnTilemap.hasWall = hasWall;
        pathOnTilemap.allowDiagonal = hasWall;
        pathOnTilemap.maxCountOfCorner = maxCountOfCorner;
        pathOnTilemap.maxCountOfStep = maxCountOfStep;
        pathOnTilemap.addPoints(startLocation);
        pathOnTilemap.addPoints(endLocation);

        return pathOnTilemap;
    }

    public void FindPathOf(PathOnTilemap pathInfomation, List<GameObject> tileList, Vector2Int currentPoint)
    {
        int countOfPoints = pathInfomation.getPoints().Count;
        if (countOfPoints < 1)
        {
            Debug.LogError("found not points of path");
            return;
        }
        Vector2Int pointBegin = pathInfomation.getPoints()[0];
        Vector2Int pointEnd = pathInfomation.getPoints()[countOfPoints - 1];

        //Vector2Int nextPoint = 

        
    }

}
