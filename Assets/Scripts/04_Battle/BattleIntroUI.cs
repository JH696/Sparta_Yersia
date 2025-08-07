using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.TerrainTools;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public static FadeScreen Instance { get; private set; }

    [TextArea]
    public List<string> texts;

    public TextMeshProUGUI Text;
    public CanvasGroup IntroCanvas;
    public float FadeDuration = 1.0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        gameObject.SetActive(false);
    }

    public IEnumerator FadeIn()
    {
        int index = Random.Range(0, texts.Count);

        gameObject.SetActive(true);
        Text.text = texts[index];
        IntroCanvas.alpha = 0f;

        float elapsed = 0f;
        while (elapsed < FadeDuration)
        {
            IntroCanvas.alpha = Mathf.Lerp(0f, 1f, elapsed / FadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        IntroCanvas.alpha = 1f; // 완전 어두워진 상태
    }

    public IEnumerator FadeOut()
    {
        float elapsed = 0f;
        while (elapsed < FadeDuration)
        {
            IntroCanvas.alpha = Mathf.Lerp(1f, 0f, elapsed / FadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        IntroCanvas.alpha = 0f;
        Text.text = string.Empty;
        gameObject.SetActive(false);
    }
}