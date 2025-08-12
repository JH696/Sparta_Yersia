using System.Collections;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    public float fadeDuration = 1f;
    public IEnumerator Fade(CanvasGroup fade, float from, float to)
    {
        float elapsed = 0f;
        fade.alpha = from;
        fade.interactable = false;
        fade.blocksRaycasts = false;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fade.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }

        fade.alpha = to;
        fade.interactable = true;
        fade.blocksRaycasts = true;
    }
}
