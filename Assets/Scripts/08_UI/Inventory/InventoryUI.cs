using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;

    [Header("아이템 정보 패널")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemInfo;

    [Header("슬롯 리스트")]
    [SerializeField] private List<ItemSlot> slots = new List<ItemSlot>();

    [Header("선택된 슬롯")]
    [SerializeField] private ItemSlot selectedSlot;

    public List<BaseItem> test;

    private void OnEnable()
    {
        player.Status.inventory.InventoryChanged += RefreshInventory;
        // equipment.EquipmentChanged += RefreshInventory;
    }

    private void OnDisable()
    {
        player.Status.inventory.InventoryChanged -= RefreshInventory;
        // equipment.EquipmentChanged -= RefreshInventory;
    }

    private void Start()
    {
        DisplaySlots();

        categoryBtns[0].onClick.AddListener(() => ChangeCategory(E_CategoryType.All));
        categoryBtns[1].onClick.AddListener(() => ChangeCategory(E_CategoryType.Equip));
        categoryBtns[2].onClick.AddListener(() => ChangeCategory(E_CategoryType.Consume));
        categoryBtns[3].onClick.AddListener(() => ChangeCategory(E_CategoryType.Quest));

        button1.onClick.AddListener(OnClick1);
        button2.onClick.AddListener(OnClick2);

        closeBtn.onClick.AddListener(CloseInventory);

        foreach (BaseItem item in test)
        {
            player.Status.inventory.AddItem(item);
        }

        RefreshInventory();
    }

    public void OpenInventory()
    {
        RefreshInventory();
        this.gameObject.SetActive(true);    
    }

    public void CloseInventory()
    {
        this.gameObject.SetActive(false);
        ClearSeletedSlot();
    }

    // 슬롯 생성 및 초기화
    private void DisplaySlots()
    {
        for (int i = 0; i < player.Status.inventory.MaxItemCount; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotParent);
            ItemSlot slot = slotObj.GetComponent<ItemSlot>();
            slot.OnClickSlot += SetSeletedSlot;
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
                slots[i].SetItem(items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }


    // 슬롯 선택 시
    private void SetSeletedSlot(ItemSlot slot)
    {
        selectedSlot = slot;
        ShowItemDetails(slot.Status.Data);
        
        if (slot.Status.Data is EquipItemData equipData)
        {
            if (slot.Status.IsEquiped == true)
            {
                button1.GetComponentInChildren<TextMeshProUGUI>().text = "장착 해제";
                return;
            }

            button1.GetComponentInChildren<TextMeshProUGUI>().text = "장착";
        }
        else
        {
            button1.GetComponentInChildren<TextMeshProUGUI>().text = "사용";
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
        button1.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;
    }

    private void OnClick1()
    {
        if (selectedSlot == null) return;

        ItemStatus status = selectedSlot.Status;
        ItemEquipment playerEquipment = player.Status.equipment;

        switch (status.Data)
        {
            default:
                Debug.Log("사용 가능한 형태의 아이템이 아닙니다.");
                return;

            case EquipItemData equipItem:
                if (status.IsEquiped == true)
                {
                    playerEquipment.Unequip(equipItem.Type);
                }
                else
                {
                    playerEquipment.Equip(selectedSlot.Status);
                }
                break;

            case ConsumeItemData consumeItem:

                break;
        }

        ClearSeletedSlot();
    }

    private void OnClick2()
    {
        if (selectedSlot == null) return;

        switch (selectedSlot.Status.Data)
        {
            default:
                selectedSlot.Status.LoseItem(1);
                break;

            case QuestItemData questItem:
                Debug.Log("퀘스트 아이템은 버릴 수 없습니다.");
                return;
        }
    }

    private void ChangeCategory(E_CategoryType category)
    {
        this.category = category;
        RefreshInventory();
    }
}

