using System.Collections;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private CanvasGroup fade;  //로고+버튼들
    [SerializeField] private float fadeDuration = 1f;
    CameraFollow CameraFollow;
    private void Awake()
    {
        // 처음에 투명 상태로 초기화 (항상 활성화된 상태 유지)
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
