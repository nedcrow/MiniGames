using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileComponent : MonoBehaviour
{
    public ETileType currentType = ETileType.Apple;
    public bool hasMovement = true;
    public bool isMoving = false;
    public bool canExplosion = true;
    public Vector2Int currentTilePosition;
    public int maxHP;
    public int currentHP;

    public void ChangeTileType(ETileType tileType)
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

    public void Move_Up() { MoveTo(new Vector2Int(currentTilePosition.x, currentTilePosition.y + 1)); }
    public void Move_Down() { MoveTo(new Vector2Int(currentTilePosition.x, currentTilePosition.y - 1)); }
    public void Move_Right() { MoveTo(new Vector2Int(currentTilePosition.x + 1, currentTilePosition.y)); }
    public void Move_Left() { MoveTo(new Vector2Int(currentTilePosition.x - 1, currentTilePosition.y)); }

    public void MoveTo(Vector2Int targetTilePosion, float targetSpeed = 1.0f)
    {
        moveTo_co = MoveTo_Co(new Vector3(targetTilePosion.x, targetTilePosion.y, 0f));
        StartCoroutine(moveTo_co);
        currentTilePosition = targetTilePosion;
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
