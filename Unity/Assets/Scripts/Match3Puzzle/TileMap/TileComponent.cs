using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileComponent : MonoBehaviour
{
    public ETileType currentType = ETileType.Apple;
    public Vector3 currentPosition;

    private IEnumerator moveTo_co;

    public void MoveTo(Vector2Int targetTilePosion, float targetSpeed = 1.0f)
    {
        moveTo_co = MoveTo_Co(new Vector3(targetTilePosion.x, targetTilePosion.y, 0f));
        StartCoroutine(moveTo_co);
    }

    private IEnumerator MoveTo_Co(Vector3 targetPosition, float targetSpeed = 1.0f)
    {
        float fps = 1 / 24f;
        int countOfStep = 0;

        while (true)
        {
            float lerp = fps * targetSpeed * countOfStep;
            if (lerp > 1)
            {
                gameObject.transform.position = currentPosition = targetPosition;
                break;

            }
            Vector3 tempPosition = Vector3.Lerp(gameObject.transform.position, targetPosition, lerp);
            gameObject.transform.position = tempPosition;

            yield return new WaitForSeconds(fps);
            countOfStep++;
        }
    }
}
