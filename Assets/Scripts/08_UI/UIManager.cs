using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // UI 요소들
    private InventoryUI inventoryUI;
    private DialogueUI dialogueUI;
    private PlayerUI playerUI;
    private StatUIController statUIController;
    private PetUIController petUIController;
    //private SkillTreeUI skillTreeUI;

    private void Awake()
    {
        // 싱글톤 패턴
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

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
        playerUI = FindObjectOfType<PlayerUI>(includeInactive: true);
        statUIController = FindObjectOfType<StatUIController>(includeInactive: true);
        petUIController = FindObjectOfType<PetUIController>(includeInactive: true);
        //skillTreeUI = FindObjectOfType<SkillTreeUI>(includeInactive: true);
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

    public void ShowPlayerUI()
    {
        if (playerUI == null)
        {
            Debug.LogWarning("[UIManager] PlayerUI를 찾을 수 없습니다.");
            return;
        }
        playerUI.ShowPlayerUI();
    }

    public void HidePlayerUI()
    {
        if (playerUI == null)
        {
            Debug.LogWarning("[UIManager] PlayerUI를 찾을 수 없습니다.");
            return;
        }
        playerUI.HidePlayerUI();
    }

    // 스탯 UI 표시 (기본: 플레이어)
    public void ShowStatUI()
    {
        if (statUIController == null)
        {
            Debug.LogWarning("[UIManager] StatUIController를 찾을 수 없습니다.");
            return;
        }
        statUIController.ShowStatUI();
    }

    // 펫 1 스탯 UI 표시
    public void ShowPet1Stats()
    {
        if (statUIController == null)
        {
            Debug.LogWarning("[UIManager] StatUIController를 찾을 수 없습니다.");
            return;
        }
        statUIController.ShowPet1Stats();
    }

    // 펫 2 스탯 UI 표시
    public void ShowPet2Stats()
    {
        if (statUIController == null)
        {
            Debug.LogWarning("[UIManager] StatUIController를 찾을 수 없습니다.");
            return;
        }
        statUIController.ShowPet2Stats();
    }

    // 스탯 UI 숨기기
    public void HideStatUI()
    {
        if (statUIController == null)
        {
            Debug.LogWarning("[UIManager] StatUIController를 찾을 수 없습니다.");
            return;
        }
        statUIController.HideStatUI();
    }

    // 스킬트리 UI 표시/숨기기
    //public void ShowSkillTreeUI()
    //{
    //    if (skillTreeUI == null)
    //    {
    //        Debug.LogWarning("[UIManager] SkillTreeUI를 찾을 수 없습니다.");
    //        return;
    //    }
    //    skillTreeUI.Show();
    //}

    //public void HideSkillTreeUI()
    //{
    //    if (skillTreeUI == null)
    //    {
    //        Debug.LogWarning("[UIManager] SkillTreeUI를 찾을 수 없습니다.");
    //        return;
    //    }
    //    skillTreeUI.Hide();
    //}

    public void ShowPetUI()
    {
        petUIController.gameObject.SetActive(true);
    }

    public void HidePetUI()
    {
        petUIController.gameObject.SetActive(false);
    }
}
