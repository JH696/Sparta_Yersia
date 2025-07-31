using UnityEngine;

/// <summary>
/// ESC 키 입력으로 설정 UI를 토글하는 컨트롤러입니다.
/// </summary>
public class SettingsUIController : MonoBehaviour
{
    [SerializeField] private GameObject settingsUI; // 설정 UI 패널

    private bool isOpen = false;

    private void Start()
    {
        // 시작 시 설정 UI 비활성화
        if (settingsUI != null)
        {
            settingsUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettingsUI();
        }
    }

    /// <summary>
    /// 설정 UI를 열거나 닫습니다.
    /// </summary>
    private void ToggleSettingsUI()
    {
        if (settingsUI == null) return;

        isOpen = !isOpen;
        settingsUI.SetActive(isOpen);
    }
}