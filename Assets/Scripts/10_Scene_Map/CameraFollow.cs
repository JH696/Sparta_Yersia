using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public RectTransform backGround;
    public Vector2 startPosition;      // ���� ��ġ
    public Vector2 endPosition;        // ���� ��ġ
    public float duration = 1.5f;        // �̵� �ð� (��)

    private float elapsedTime = 0f;
    private bool isMoving = true;

    FadeIn fadein;

    void Start()
    {
        fadein = GetComponent<FadeIn>();
        backGround.anchoredPosition = startPosition;
        StartCoroutine(Scroll());
    }

    public IEnumerator Scroll()
    {
        if (!isMoving) yield break;

        while (elapsedTime < duration)
        {
            float t = Mathf.Clamp01(elapsedTime / duration); // 0~1 ������
                                                             //�ε巴�� �̵�
            t = Mathf.SmoothStep(0f, 1f, t);
            // ���� ���� (Lerp)���� ��ġ �̵�
            backGround.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (elapsedTime >= duration)
        {
            isMoving = false;
            yield return StartCoroutine(fadein.Fade(0f, 1f));
        }
    }
}
