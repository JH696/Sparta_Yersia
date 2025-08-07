using UnityEngine;

/// <summary>
/// 특정 지역에 진입하면 지정된 배경음을 재생합니다.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class RegionBGMTrigger : MonoBehaviour
{
    [Header("이 지역에서 재생할 배경음")]
    [SerializeField] private AudioClip regionBGM;

    [Tooltip("중복 재생 방지 (이미 이 BGM이 재생 중이면 무시)")]
    [SerializeField] private bool avoidDuplicatePlay = true;

    [Tooltip("BGM 재생 전 대기 시간")]
    [SerializeField] private float delay = 0f;

    [Tooltip("기존 BGM을 페이드아웃하는 시간 (초)")]
    [SerializeField] private float fadeDuration = 1.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (SoundManager.Instance == null || regionBGM == null) return;

        if (avoidDuplicatePlay &&
            SoundManager.Instance.CurrentBGM == regionBGM &&
            SoundManager.Instance.IsBGMPlaying)
        {
            return;
        }

        SoundManager.Instance.PlayBGM(regionBGM, loop: true, delay, fadeDuration);
    }
}