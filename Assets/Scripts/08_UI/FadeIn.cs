using System.Collections;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private CanvasGroup fade;  //�ΰ�+��ư��
    [SerializeField] private float fadeDuration = 1f;
    CameraFollow CameraFollow;
    private void Awake()
    {
        // ó���� ���� ���·� �ʱ�ȭ (�׻� Ȱ��ȭ�� ���� ����)
        SetAlpha(0f);
    }


    public IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fade.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }
    }

    private void SetAlpha(float alpha)
    {
        if (fade == null) return;

        fade.alpha = alpha;
    }
}
