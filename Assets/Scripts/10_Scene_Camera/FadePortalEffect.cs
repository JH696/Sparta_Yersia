using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadePortalEffect : MonoBehaviour, IPortalEffect
{
    [SerializeField] private Image fadeImage;
     private float fadeDuration = 0.5f;

    private void Awake()
    {
        // 처음에 투명 상태로 초기화 (항상 활성화된 상태 유지)
        SetAlpha(0f);
    }

    public IEnumerator PlayBeforeTeleport()
    {
        yield return Fade(0f, 1f);
    }

    public IEnumerator PlayAfterTeleport()
    {
        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(from, to, elapsed / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = to;
        fadeImage.color = color;
    }

    private void SetAlpha(float alpha)
    {
        if (fadeImage == null) return;

        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}