using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public static FadeScreen Instance { get; private set; }

    [TextArea]
    public List<string> texts;

    public TextMeshProUGUI Text;
    public CanvasGroup IntroCanvas;

    [Header("페이드 인/아웃 설정")]
    [Tooltip("페이드 진입 시간")] public float FadeInDuration = 0.5f;   // 빠르게 어두워짐
    [Tooltip("페이드 유지 시간")] public float StayDuration = 1.5f;     // 어두운 상태 유지
    [Tooltip("페이드 이탈 시간")] public float FadeOutDuration = 0.3f;  // 빠르게 밝아짐

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
        while (elapsed < FadeInDuration)
        {
            float t = elapsed / FadeInDuration;
            IntroCanvas.alpha = Mathf.Pow(t, 0.5f); // Ease-out: 빠르게 어두워짐
            elapsed += Time.deltaTime;
            yield return null;
        }

        IntroCanvas.alpha = 1f; // 완전 어두운 상태
    }

    public IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(StayDuration); // 어두운 상태 유지

        float elapsed = 0f;
        while (elapsed < FadeOutDuration)
        {
            float t = elapsed / FadeOutDuration;
            IntroCanvas.alpha = 1f - Mathf.Pow(t, 2f); // Ease-in: 빠르게 밝아짐
            elapsed += Time.deltaTime;
            yield return null;
        }

        IntroCanvas.alpha = 0f;
        Text.text = string.Empty;
        gameObject.SetActive(false);
    }

    // 전체 연출을 한번에 실행하는 메서드 (선택사항)
    public void PlayFadeSequence()
    {
        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        yield return FadeIn();
        yield return FadeOut();
    }
}