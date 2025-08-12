using UnityEngine;

/// <summary>
/// ESC 키 입력, 버튼 클릭으로 설정 UI를 토글하거나 닫는 컨트롤러
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
    /// 설정 UI를 열거나 닫습니다. (ESC, 버튼 모두에서 호출 가능)
    /// </summary>
    public void ToggleSettingsUI()
    {
        if (settingsUI == null) return;

        isOpen = !isOpen;
        settingsUI.SetActive(isOpen);
    }

    /// <summary>
    /// 설정 UI를 닫습니다. (닫기 버튼 전용)
    /// </summary>
    public void CloseSettingsUI()
    {
        if (settingsUI == null) return;

        isOpen = false;
        settingsUI.SetActive(false);
    }
}