using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3TileComponent : TileComponent
{    
    [Header("Match3 properties")]
    public bool hasMovement = true;
    public bool isMoving = false;
    public bool canExplosion = true;

    [SerializeField]
    protected int maxHP;

    [SerializeField]
    protected int currentHP;

    [SerializeField]
    protected E3MTileType currentType = E3MTileType.Apple;

    public E3MTileType GetCurrentType()
    {
        return currentType;
    }
    public void ChangeTileType(E3MTileType tileType)
    {
        currentType = tileType;

        if(PuzzleManager.instance.tileMaterials.Length <= (int)currentType)
        {
            Debug.LogWarning("Index Out Of Range Exception(PuzzleManager.tileMaterials): {" + (int)currentType + "} in ChangeTileType from TileComponent");
            return;
        }

        GetComponent<MeshRenderer>().material = PuzzleManager.instance.tileMaterials[
             Mathf.Clamp((int)currentType, 0, PuzzleManager.instance.tileMaterials.Length)
        ];
    }

    private IEnumerator moveTo_co;

    public void Move_Up() { MoveTo(new Vector2Int(tileLocation.x, tileLocation.y + 1)); }
    public void Move_Down() { MoveTo(new Vector2Int(tileLocation.x, tileLocation.y - 1)); }
    public void Move_Right() { MoveTo(new Vector2Int(tileLocation.x + 1, tileLocation.y)); }
    public void Move_Left() { MoveTo(new Vector2Int(tileLocation.x - 1, tileLocation.y)); }

    public void MoveTo(Vector2Int targetTilePosion, float targetSpeed = 1.0f)
    {
        moveTo_co = MoveTo_Co(new Vector3(targetTilePosion.x, targetTilePosion.y, 0f));
        StartCoroutine(moveTo_co);
        tileLocation = targetTilePosion;
    }

    private IEnumerator MoveTo_Co(Vector3 targetPosition, float targetSpeed = 1.0f)
    {
        float fps = 1 / 24f;
        int countOfStep = 0;

        isMoving = true;

        while (true)
        {
            float lerp = fps * targetSpeed * countOfStep;
            if (lerp >= 1)
            {
                gameObject.transform.position = targetPosition;
                isMoving = false;
                StopCoroutine(moveTo_co);
                break;
            }
            Vector3 tempPosition = Vector3.Lerp(gameObject.transform.position, targetPosition, lerp);
            gameObject.transform.position = tempPosition;

            yield return new WaitForSeconds(fps);
            countOfStep++;
        }
    }
}
