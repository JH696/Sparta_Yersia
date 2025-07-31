using UnityEngine;
using System.Collections;

/// <summary>
/// 게임 내 모든 사운드의 진입점. BGM, SFX 등을 분리 관리합니다.
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource bgmSource;

    /// <summary>
    /// 현재 재생 중인 BGM 클립
    /// </summary>
    public AudioClip CurrentBGM => bgmSource.clip;

    /// <summary>
    /// 현재 BGM이 재생 중인지 여부
    /// </summary>
    public bool IsBGMPlaying => bgmSource.isPlaying;

    /// <summary>
    /// 현재 설정된 BGM 볼륨
    /// </summary>
    public float BGMVolume { get; private set; } = 1f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (bgmSource == null)
            Debug.LogWarning("[SoundManager] BGM용 AudioSource가 할당되지 않았습니다.");

        // 초기 볼륨 설정 불러오기
        float savedVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        SetBGMVolume(savedVolume);
    }

    /// <summary>
    /// 배경음악(BGM)을 페이드아웃 후 교체하고 재생합니다.
    /// </summary>
    public void PlayBGM(AudioClip clip, bool loop = true, float delay = 0f, float fadeDuration = 1f)
    {
        if (clip == null)
        {
            Debug.LogWarning("[SoundManager] PlayBGM 호출 시 clip이 null입니다.");
            return;
        }

        if (bgmSource.clip == clip && bgmSource.isPlaying) return;

        StopAllCoroutines();
        StartCoroutine(TransitionBGM(clip, loop, delay, fadeDuration));
    }

    private IEnumerator TransitionBGM(AudioClip newClip, bool loop, float delay, float fadeDuration)
    {
        if (bgmSource.isPlaying)
            yield return StartCoroutine(FadeOutBGM(fadeDuration));

        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        bgmSource.clip = newClip;
        bgmSource.loop = loop;
        bgmSource.Play();

        yield return StartCoroutine(FadeInBGM(fadeDuration));

        Debug.Log($"[SoundManager] PlayBGM: {newClip.name} (delay: {delay}s, fade: {fadeDuration}s)");
    }

    private IEnumerator FadeOutBGM(float duration)
    {
        float startVolume = bgmSource.volume;

        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        bgmSource.volume = 0f;
        bgmSource.Stop();
    }

    private IEnumerator FadeInBGM(float duration)
    {
        float targetVolume = BGMVolume; // 사용자 설정 볼륨 기준으로 페이드 인
        bgmSource.volume = 0f;

        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            bgmSource.volume = Mathf.Lerp(0f, targetVolume, t / duration);
            yield return null;
        }

        bgmSource.volume = targetVolume;
    }

    /// <summary>
    /// 현재 재생 중인 배경음을 즉시 중지합니다.
    /// </summary>
    public void StopBGM() => bgmSource.Stop();

    /// <summary>
    /// 배경음 볼륨을 설정합니다.
    /// </summary>
    public void SetBGMVolume(float volume)
    {
        BGMVolume = Mathf.Clamp01(volume);
        bgmSource.volume = BGMVolume;
    }
}