using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager_test : MonoBehaviour
{
    public static UIManager_test Instance { get; private set; }

    // UI 요소들
    private InventoryUI inventoryUI;

    private void Awake()
    {
        // 싱글톤 패턴
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 씬 로드시 UI 초기화 및 이벤트 구독
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 씬이 파괴될 떄 구독 해제
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 새로운 씬이 로드될 때 UI 초기화
        inventoryUI = FindObjectOfType<InventoryUI>(includeInactive: true);
    }

    public void ShowInventory()
    {
        if (inventoryUI == null)
        {
            Debug.LogWarning("[UIManager] InventoryUI를 찾을 수 없습니다.");
            return;
        }
        inventoryUI.Show();
    }

    public void HideInventory()
    {
        if (inventoryUI == null)
        {
            Debug.LogWarning("[UIManager] InventoryUI를 찾을 수 없습니다.");
            return;
        }
        inventoryUI.Hide();
    }
}
