using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [System.Serializable]
    public class MovingImage
    {
        public RectTransform backGround;
        public Vector2 startPosition;      // 시작 위치
        public Vector2 endPosition;        // 도착 위치
        public float duration = 1.5f;        // 이동 시간 (초)
        public string narrationText;
    }
    [Header("현재 무빙이미지 클래스 데이터")]
    public MovingImage image;
    [Header("무빙이미지 클래스 배열")]
    public MovingImage[] images;

    private float elapsedTime = 0f;
    [HideInInspector] public bool isMoving = false;

    void Start()
    {
        image.backGround.gameObject.SetActive(false);
    }

    public IEnumerator Scroll()
    {
        if (isMoving) yield break;

        isMoving = true;
        elapsedTime = 0f;

        image.backGround.gameObject.SetActive(true);
        image.backGround.anchoredPosition = image.startPosition;

        while (elapsedTime < image.duration)
        {
            float t = Mathf.Clamp01(elapsedTime / image.duration); // 0~1 보간값
                                                                   //부드럽게 이동
            t = Mathf.SmoothStep(0f, 1f, t);
            // 선형 보간 (Lerp)으로 위치 이동
            image.backGround.anchoredPosition = Vector2.Lerp(image.startPosition, image.endPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (elapsedTime >= image.duration)
        {
            isMoving = false;
        }
    }
}
