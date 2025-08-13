using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("능력치 UI")]
    [SerializeField] private StatUIController statUIController;

    [Header("펫 UI")]
    [SerializeField] private PetUIController petUIController;

    [Header("인벤토리 UI")]
    [SerializeField] private InventoryUI inventoryUI;

    [Header("스킬 UI")]
    [SerializeField] private SkillInventoryUI skillUI;

    [Header("상점 UI")]
    [SerializeField] private ShopUI shopUI;

    [Header("설정 UI")]
    [SerializeField] private SettingsUIController settingsUI;

    [Header("게임 종료창")]
    [SerializeField] private GameObject quitPanel;

    [Header("플레이어 프로필 UI")]
    [SerializeField] private PlayerUI playerUI;

    [Header("다이얼로그 UI")]
    [SerializeField] private DialogueUI dialogueUI;

    private void Awake()
    {
        // 싱글톤 패턴
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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

    // 대화 UI 표시
    public void ShowDialogue()
    {
        if (dialogueUI == null)
        {
            Debug.LogWarning("[UIManager] DialogueUI를 찾을 수 없습니다.");
            return;
        }

        HideAllUI();
        dialogueUI.ShowDialogueUI();
    }

    // 스탯 UI 표시 (기본: 플레이어)
    public void ShowStatUI()
    {
        if (statUIController == null)
        {
            Debug.LogWarning("[UIManager] StatUIController를 찾을 수 없습니다.");
            return;
        }

        HideAllUI();
        statUIController.ShowStatUI();
    }

    public void ShowPetUI()
    {
        if (statUIController == null)
        {
            Debug.LogWarning("[UIManager] StatUIController를 찾을 수 없습니다.");
            return;
        }

        HideAllUI();
        petUIController.ShowPetUI();
    }

    public void ShowInventoryUI()
    {
        if (inventoryUI == null)
        {
            Debug.LogWarning("[UIManager] InventoryUI를 찾을 수 없습니다.");
            return;
        }
        HideAllUI();
        inventoryUI.OpenInventory();
    }

    public void ShowSKillUI()
    {
        if (inventoryUI == null)
        {
            Debug.LogWarning("[UIManager] ShowSKillUI를 찾을 수 없습니다.");
            return;
        }

        HideAllUI();
        skillUI.ShowSkillUI();
    }


    public void SetProfileIcon(Sprite icon)
    {
        if (playerUI != null)
            playerUI.SetProfileIcon(icon);
    }

    public void HideAllUI()
    {
        statUIController.HideStatUI();
        petUIController.HidePetUI();
        inventoryUI.CloseInventory();
        skillUI.ResetSkillUI();
        dialogueUI.HideDialogueUI();
        shopUI.Hide();    
        settingsUI.CloseSettingsUI();
        quitPanel.SetActive(false);
    }

    public void ShowShopUI()
    {
        if (shopUI == null)
        {
            Debug.LogWarning("[UIManager] ShopUI를 찾을 수 없습니다.");
            return;
        }

        HideAllUI();
        shopUI.Show();
    }

    public void HideShopUI()
    {
        if (shopUI == null)
        {
            Debug.LogWarning("[UIManager] ShopUI를 찾을 수 없습니다.");
            return;
        }

        shopUI.Hide();
    }

    public void ShowSettingUI()
    {
        if (shopUI == null)
        {
            Debug.LogWarning("[UIManager] SettingUI를 찾을 수 없습니다.");
            return;
        }

        HideAllUI();
        settingsUI.ToggleSettingsUI();
    }

    public void HideSettingUI()
    {
        if (shopUI == null)
        {
            Debug.LogWarning("[UIManager] SettingUI를 찾을 수 없습니다.");
            return;
        }

        settingsUI.CloseSettingsUI();
    }

    public void ShowQuitPanel()
    {
        if (quitPanel == null)
        {
            Debug.LogWarning("[UIManager] QuitPanel을 찾을 수 없습니다.");
            return;
        }

        HideAllUI();
        quitPanel.SetActive(true);
    }
    public void HideQuitPanel()
    {
        if (quitPanel == null)
        {
            Debug.LogWarning("[UIManager] QuitPanel을 찾을 수 없습니다.");
            return;
        }

        quitPanel.SetActive(false);
    }

    public void GameQuitButton()
    {
        SceneLoader.LoadScene("StartScene");
    }
}
