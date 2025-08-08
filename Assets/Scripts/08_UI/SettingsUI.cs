using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Slider BGMVolumeSlider;

    private void Start()
    {
        // 저장된 값 불러오기
        BGMVolumeSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1.0f);

        // 초기 볼륨 적용
        SoundManager.Instance.SetBGMVolume(BGMVolumeSlider.value);

        // 리스너 연결
        BGMVolumeSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
    }

    private void OnBGMVolumeChanged(float value)
    {
        SoundManager.Instance.SetBGMVolume(value);
        PlayerPrefs.SetFloat("BGMVolume", value);
    }
}