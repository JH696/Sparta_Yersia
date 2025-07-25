using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public RectTransform backGround;
    public Vector2 startPosition;      // 시작 위치
    public Vector2 endPosition;        // 도착 위치
    public float duration = 1.5f;        // 이동 시간 (초)

    private float elapsedTime = 0f;
    private bool isMoving = false;

    void Start()
    {
        backGround.anchoredPosition = startPosition;
    }

    public IEnumerator Scroll()
    {
        if (isMoving) yield break;

        isMoving = true;
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = Mathf.Clamp01(elapsedTime / duration); // 0~1 보간값
            //부드럽게 이동
            t = Mathf.SmoothStep(0f, 1f, t);
            // 선형 보간 (Lerp)으로 위치 이동
            backGround.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (elapsedTime >= duration)
        {
            isMoving = false;
        }
    }
}
