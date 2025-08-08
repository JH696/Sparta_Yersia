using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 내 모든 사운드의 진입점. BGM은 기존 AudioClip 직접 참조
/// SFX는 enum 기반으로 분리 관리
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

    /// <summary>현재 BGM 볼륨</summary>
    public float BGMVolume { get; private set; } = 1f;

    /// <summary>현재 SFX 볼륨</summary>
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

        // 초기 볼륨 설정 불러오기
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

    /// <summary>
    /// 배경음악(BGM)을 페이드아웃 후 교체하고 재생
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
        float targetVolume = BGMVolume;
        bgmSource.volume = 0f;

        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            bgmSource.volume = Mathf.Lerp(0f, targetVolume, t / duration);
            yield return null;
        }

        bgmSource.volume = targetVolume;
    }

    /// <summary>현재 재생 중인 배경음을 즉시 중지</summary>
    public void StopBGM() => bgmSource.Stop();

    /// <summary>배경음 볼륨을 설정</summary>
    public void SetBGMVolume(float volume)
    {
        BGMVolume = Mathf.Clamp01(volume);
        bgmSource.volume = BGMVolume;
        PlayerPrefs.SetFloat("BGMVolume", BGMVolume);
    }

    /// <summary>enum 기반 효과음 재생 (스킬, UI 등)</summary>
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

        sfxSource.PlayOneShot(clip, SFXVolume);
    }

    /// <summary>기본 UI 클릭 사운드를 재생</summary>
    public void PlayClick()
    {
        PlaySFX(SFXType.Click);
    }

    /// <summary>효과음 볼륨을 설정</summary>
    public void SetSFXVolume(float volume)
    {
        SFXVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
    }
}

/// <summary>
/// 효과음 종류(enum) 필요한 사운드명 여기에 추가
/// </summary>
public enum SFXType
{
    Click,
    SkillCast,
    SkillHit,
    EnemyDie,
    PlayerDie,
    PartyMemberDie,
    MissionComplete,
}