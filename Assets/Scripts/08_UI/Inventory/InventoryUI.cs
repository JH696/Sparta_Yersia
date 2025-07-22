using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum E_CategoryType
{
    All,
    Equip,
    Consume,
    Quest,
}

public class InventoryUI : MonoBehaviour
{
    [Header("인벤토리 주인")]
    [SerializeField] private Player player;

    [Header("인벤토리 패널")]
    [SerializeField] private GameObject inventoryPanel;

    [Header("장비 UI")]
    [SerializeField] private EquipmentUI equipment;

    [Header("인벤토리 슬롯")]
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject slotPrefab;

    [Header("현재 카테고리")]
    [SerializeField] private E_CategoryType category = E_CategoryType.All;

    [Header("카테고리 버튼")]
    [SerializeField] private List<Button> categoryBtns; // All, Equip, Consume, Quest

    [Header("오픈 / 클로즈 버튼")]
    [SerializeField] private Button openBtn; // UI 매니저
    [SerializeField] private Button closeBtn;

    [Header("인벤토리 버튼")]
    [SerializeField] private Button interactBtn;
    [SerializeField] private Button discardBtn;

    [Header("아이템 정보 패널")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemInfo;

    [Header("슬롯 리스트")]
    [SerializeField] private List<ItemSlot> slots = new List<ItemSlot>();

    [Header("선택된 슬롯")]
    [SerializeField] private ItemSlot selectedSlot;

    public event System.Action ChangeSelctedSlot;


    private void Start()
    {
        DisplaySlots();

        player.Status.inventory.InventoryChanged += RefreshInventory;

        categoryBtns[0].onClick.AddListener(() => ChangeCategory(E_CategoryType.All));
        categoryBtns[1].onClick.AddListener(() => ChangeCategory(E_CategoryType.Equip));
        categoryBtns[2].onClick.AddListener(() => ChangeCategory(E_CategoryType.Consume));
        categoryBtns[3].onClick.AddListener(() => ChangeCategory(E_CategoryType.Quest));

        interactBtn.onClick.AddListener(OnInteractButton); // 장비, 사용, 사용 불가 버튼 
        discardBtn.onClick.AddListener(OnDiscardButton); // 아이템 버리기

        closeBtn.onClick.AddListener(CloseInventory);

        RefreshInventory();
    }

    private void OnDisable()
    {
        if (player.Status == null) return;

        player.Status.inventory.InventoryChanged -= RefreshInventory;
    }

    public void OpenInventory()
    {
        RefreshInventory();
        inventoryPanel.SetActive(true);    
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        ClearSeletedSlot();
    }

    // 슬롯 생성 및 초기화
    private void DisplaySlots()
    {
        for (int i = 0; i < player.Status.inventory.MaxItemCount; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotParent);
            ItemSlot slot = slotObj.GetComponent<ItemSlot>();
            slot.OnClickSlot += SetSelectedSlot;
            slots.Add(slot);
        }
    }

    // 카테고리에 따른 인벤토리 동기화
    private void RefreshInventory()
    {
        foreach (ItemSlot slot in slots)
        {
            slot.ClearSlot();
        }

        List<ItemStatus> items;

        switch (category)
        {
            default:
                items = player.Status.inventory.Items;
                break;
            case E_CategoryType.Equip:
                items = player.Status.inventory.Items.FindAll(item => item.Data.GetCategory() == E_CategoryType.Equip);
                break;
            case E_CategoryType.Consume:
                items = player.Status.inventory.Items.FindAll(item => item.Data.GetCategory() == E_CategoryType.Consume);
                break;
            case E_CategoryType.Quest:
                items = player.Status.inventory.Items.FindAll(item => item.Data.GetCategory() == E_CategoryType.Quest);
                break;
        }

        for (int i = 0; i < slots.Count; i++)
        {
            if (i < items.Count)
            {
                slots[i].SetItem(items[i], player.Status);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    // 슬롯 선택 시
    private void SetSelectedSlot(ItemSlot slot)
    {
        selectedSlot = slot;
        ShowItemDetails(selectedSlot.Status.Data);
        UpdateButton();
    }

    // 버튼 클릭 이벤트 메서드
    private void OnInteractButton()
    {
        if (selectedSlot == null) return;

        switch (selectedSlot.Status.Data)
        {
            case EquipItemData equipData:
                if (player.Status.equipment.FindEquippedItem(equipData) != null)
                {
                    player.Status.equipment.Unequip(equipData.Type);
                    selectedSlot.DeactiveEquipSlot();
                }
                else
                {
                    player.Status.equipment.Equip(equipData);
                    selectedSlot.ActiveEquipSlot();
                }
                break;

            case ConsumeItemData consumeData:
                consumeData.Consume(player.Status);
                selectedSlot.Status.LoseItem(1);
                break;

            default:
                Debug.LogWarning("[InventoryUI] 알 수 없는 아이템 타입입니다.");
                return;
        }

        ClearSeletedSlot();
        UpdateButton();
    }

    private void OnDiscardButton()
    {
        if (selectedSlot == null) return;

        switch (selectedSlot.Status.Data)
        {
            default:
                selectedSlot.Status.LoseItem(1);
                break;

            case EquipItemData equipItem:
                player.Status.equipment.Unequip(equipItem.Type);
                selectedSlot.Status.LoseItem(1);
                break;

            case QuestItemData questItem:
                return;
        }

        ClearSeletedSlot();
        UpdateButton();
    }

    // 버튼 오브젝트 업데이트 메서드
    private void UpdateButton()
    {
        if (selectedSlot == null)
        {
            interactBtn.gameObject.SetActive(false);
            discardBtn.gameObject.SetActive(false);
            return;
        }

        var buttonText = interactBtn.GetComponentInChildren<TextMeshProUGUI>();

        switch (selectedSlot.Status.Data)
        {
            case EquipItemData equipData:
                bool isEquipped = player.Status.equipment.FindEquippedItem(equipData) != null;
                interactBtn.gameObject.SetActive(true);
                discardBtn.gameObject.SetActive(true);

                if (isEquipped)
                {
                    buttonText.text = "장비 해제";
                }
                else
                {
                    buttonText.text = "장비";
                }
                break;

            case ConsumeItemData consumeData:
                interactBtn.gameObject.SetActive(true);
                discardBtn.gameObject.SetActive(true);
                buttonText.text = "사용";
                break;

            default:
                buttonText.text = string.Empty;
                interactBtn.gameObject.SetActive(false);
                discardBtn.gameObject.SetActive(false);
                break;
        }
    }

    // 선택된 슬롯 제거
    private void ClearSeletedSlot()
    {
        selectedSlot = null;
        ResetInfoPanel();
    }

    // 아이템 정보 표시
    public void ShowItemDetails(BaseItem basItem)
    {
        ResetInfoPanel();

        switch (basItem)
        {
            case EquipItemData eData:
                itemName.text = eData.Name;
                eData.Values.ForEach
                (value => { itemInfo.text += $"{value.Stat}: {value.Value}\n"; });
                break;
            case ConsumeItemData cData:
                itemName.text = cData.Name;
                cData.Values.ForEach
                (value => { itemInfo.text += $"{value.Stat}: {value.Value}\n"; });
                break;
            case QuestItemData qData:
                itemName.text = qData.Name;
                itemInfo.text = qData.Description;
                break;
        }
    }

    // 아이템 정보창 초기화
    public void ResetInfoPanel()
    {
        itemName.text = string.Empty;
        itemInfo.text = string.Empty;
    }



    private void ChangeCategory(E_CategoryType category)
    {
        this.category = category;
        RefreshInventory();
    }
}

