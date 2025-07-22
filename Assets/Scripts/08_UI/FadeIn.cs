using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private CanvasGroup fade;  //로고+버튼들
    [SerializeField] private CanvasGroup fadeBlack;  //까만화면
    [SerializeField] private float fadeDuration = 1f;
    private void Awake()
    {

    }


    public IEnumerator Fade(CanvasGroup fade, float from, float to)
    {
        float elapsed = 0f;
        fade.alpha = 0f;
        fade.interactable = false;
        fade.blocksRaycasts = false;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fade.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }

        fade.alpha = 1f;
        fade.interactable = true;
        fade.blocksRaycasts = true;
    }

    private void SetAlpha(float alpha)
    {
        if (fade == null) return;

        fade.alpha = alpha;
    }
}
