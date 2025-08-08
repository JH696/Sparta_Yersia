using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("게임내 UI")]
    [SerializeField] private StatUIController statUIController;
    [SerializeField] private PetUIController petUIController;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private SkillInventoryUI skillUI;

    [SerializeField] private PlayerUI playerUI;
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
            Debug.LogWarning("[UIManager] inventoryUI를 찾을 수 없습니다.");
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
    }
}
