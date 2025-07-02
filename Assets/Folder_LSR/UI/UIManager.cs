using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // UI 요소들
    private InventoryUI inventoryUI;
    private DialogueUI dialogueUI;

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
        dialogueUI = FindObjectOfType<DialogueUI>(includeInactive: true);
    }

    // 인벤토리 UI 표시
    public void ShowInventory()
    {
        if (inventoryUI == null)
        {
            Debug.LogWarning("[UIManager] InventoryUI를 찾을 수 없습니다.");
            return;
        }
        inventoryUI.Show();
    }

    // 인벤토리 UI 숨기기
    public void HideInventory()
    {
        if (inventoryUI == null)
        {
            Debug.LogWarning("[UIManager] InventoryUI를 찾을 수 없습니다.");
            return;
        }
        inventoryUI.Hide();
    }

    // 대화 UI 표시
    public void ShowDialogue()
    {
        if (dialogueUI == null)
        {
            Debug.LogWarning("[UIManager] DialogueUI를 찾을 수 없습니다.");
            return;
        }
        dialogueUI.ShowDialogueUI();
    }

    public void HideDialogue()
    {
        if (dialogueUI == null)
        {
            Debug.LogWarning("[UIManager] DialogueUI를 찾을 수 없습니다.");
            return;
        }
        dialogueUI.HideDialogueUI();
    }
}
