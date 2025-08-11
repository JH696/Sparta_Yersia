using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 내 모든 사운드의 진입점. 
/// BGM은 AudioClip 직접 참조, SFX는 enum 기반과 AudioClip 직접 재생 둘 다 지원
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("BGM 설정")]
    [SerializeField] private AudioSource bgmSource;

    [Header("SFX 설정")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private List<AudioClip> sfxClips;

    [Header("기본 UI 클릭음")]
    [SerializeField] private AudioClip clickSound;

    /// <summary>현재 재생 중인 BGM 클립</summary>
    public AudioClip CurrentBGM => bgmSource.clip;
    /// <summary>현재 BGM 재생 여부</summary>
    public bool IsBGMPlaying => bgmSource.isPlaying;

    /// <summary>전체 볼륨 (0~1)</summary>
    public float MasterVolume { get; private set; } = 1f;
    /// <summary>BGM 볼륨 (0~1)</summary>
    public float BGMVolume { get; private set; } = 1f;
    /// <summary>SFX 볼륨 (0~1)</summary>
    public float SFXVolume { get; private set; } = 1f;

    private Dictionary<SFXType, AudioClip> _sfxDict;

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
        if (sfxSource == null)
            Debug.LogWarning("[SoundManager] SFX용 AudioSource가 할당되지 않았습니다.");

        InitializeSFXDictionary();

        SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 1f));
        SetBGMVolume(PlayerPrefs.GetFloat("BGMVolume", 1f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 1f));
    }

    private void InitializeSFXDictionary()
    {
        _sfxDict = new Dictionary<SFXType, AudioClip>();

        foreach (SFXType type in Enum.GetValues(typeof(SFXType)))
        {
            int index = (int)type;
            if (index >= 0 && index < sfxClips.Count)
            {
                _sfxDict[type] = sfxClips[index];
            }
            else
            {
                Debug.LogWarning($"[SoundManager] SFXType {type}에 해당하는 AudioClip이 sfxClips에 없습니다.");
            }
        }
    }

    #region BGM 재생 / 전환

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
        float targetVolume = MasterVolume * BGMVolume;
        bgmSource.volume = 0f;

        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            bgmSource.volume = Mathf.Lerp(0f, targetVolume, t / duration);
            yield return null;
        }

        bgmSource.volume = targetVolume;
    }

    public void StopBGM() => bgmSource.Stop();

    #endregion

    #region 볼륨 설정

    public void SetMasterVolume(float volume)
    {
        MasterVolume = Mathf.Clamp01(volume);
        ApplyVolumes();
        PlayerPrefs.SetFloat("MasterVolume", MasterVolume);
    }

    public void SetBGMVolume(float volume)
    {
        BGMVolume = Mathf.Clamp01(volume);
        ApplyVolumes();
        PlayerPrefs.SetFloat("BGMVolume", BGMVolume);
    }

    public void SetSFXVolume(float volume)
    {
        SFXVolume = Mathf.Clamp01(volume);
        ApplyVolumes();
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
    }

    private void ApplyVolumes()
    {
        if (bgmSource != null)
            bgmSource.volume = MasterVolume * BGMVolume;

        if (sfxSource != null)
            sfxSource.volume = MasterVolume * SFXVolume;
    }

    #endregion

    #region SFX 재생

    // enum 기반 SFX 재생
    public void PlaySFX(SFXType type)
    {
        if (_sfxDict == null)
        {
            Debug.LogWarning("[SoundManager] SFX 딕셔너리가 초기화되지 않았습니다.");
            return;
        }

        if (!_sfxDict.TryGetValue(type, out var clip) || clip == null)
        {
            Debug.LogWarning($"[SoundManager] SFXType {type}에 해당하는 클립이 없습니다.");
            return;
        }

        if (sfxSource == null)
        {
            Debug.LogWarning("[SoundManager] SFX용 AudioSource가 할당되지 않았습니다.");
            return;
        }

        sfxSource.PlayOneShot(clip, MasterVolume * SFXVolume);
    }

    // AudioClip 직접 전달받아 재생 (스킬별 개별 사운드용)
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("[SoundManager] PlaySFX 호출 시 clip이 null입니다.");
            return;
        }

        if (sfxSource == null)
        {
            Debug.LogWarning("[SoundManager] SFX용 AudioSource가 할당되지 않았습니다.");
            return;
        }

        sfxSource.PlayOneShot(clip, MasterVolume * SFXVolume);
    }

    public void PlayClick()
    {
        PlaySFX(SFXType.Click);
    }

    #endregion
}

public enum SFXType
{
    None,
    Click,
    Die,
    MissionComplete,
}