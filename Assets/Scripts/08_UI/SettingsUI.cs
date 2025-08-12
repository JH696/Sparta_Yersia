using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("볼륨 조절 슬라이더")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private void Start()
    {
        // 저장된 값 불러오기
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        bgmVolumeSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1.0f);

        // 초기 볼륨 적용
        SetMasterVolume(masterVolumeSlider.value);
        SoundManager.Instance.SetBGMVolume(bgmVolumeSlider.value);
        SoundManager.Instance.SetSFXVolume(sfxVolumeSlider.value);

        // 리스너 연결
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        bgmVolumeSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private void OnMasterVolumeChanged(float value)
    {
        SetMasterVolume(value);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    private void OnBGMVolumeChanged(float value)
    {
        SoundManager.Instance.SetBGMVolume(value);
        PlayerPrefs.SetFloat("BGMVolume", value);
    }

    private void OnSFXVolumeChanged(float value)
    {
        SoundManager.Instance.SetSFXVolume(value);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    private void SetMasterVolume(float value)
    {
        AudioListener.volume = Mathf.Clamp01(value);
    }
}