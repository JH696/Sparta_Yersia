using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public enum E_CategoryType
{
    All,
    Equip,
    Consume,
    Quest,
}

public class InventoryUI : MonoBehaviour
{
    //public static Color SelectedSlotColor;
    //public static Color NormalSlotColor;

    [Header("인벤토리")]
    [SerializeField] private ItemInventory inventory;

    [Header("오픈 / 클로즈 버튼")]
    [SerializeField] private Button openBtn;
    [SerializeField] private Button closeBtn;

    [Header("카테고리 버튼")]
    [SerializeField] private Button all;
    [SerializeField] private Button equip;
    [SerializeField] private Button consume;
    [SerializeField] private Button quest;

    [Header("사용 / 해제(버리기) 버튼")]
    [SerializeField] private Button actionBtn; // 장착/사용 버튼
    [SerializeField] private Button cancelBtn; // 해제/버리기 버튼

    [Header("아이템 정보 패널")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemInfo;

    [Header("인벤토리 Slot")]
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private int slotCount = 20;

    [Header("장착아이템 Slot")]
    [SerializeField] private Transform equipPanel;

    private List<ItemSlot> slots = new List<ItemSlot>();
    private Dictionary<E_EquipType, ItemSlot> equipSlots = new Dictionary<E_EquipType, ItemSlot>();

    private ItemSlot selectSlot;

    public void Start()
    {
        inventory.InventoryChanged += RefreshUI;
        DisplaySlots();
    }

    // 아이템을 슬롯에 표시
    public void DisplaySlots()
    {
        for (int i = 0; i < slotCount; i++)
        {
            var slot = Instantiate(slotPrefab, slotParent).GetComponent<ItemSlot>();
            slot.SlotClear();
            slots.Add(slot);
        }
    }

    // 인벤토리 UI 갱신
    public void RefreshUI()
    {
        List<ItemStatus> items = inventory.Items;

        if (items.Count <= 0) return;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null)
            {
                slots[i].SlotClear();
                slots[i].SetSlot(items[i], items[i].Stack, _ => ShowItemDetails(slots[i]));
            }
            else
            {
                var emptyslot = slots[i];
                emptyslot.SlotClear();
                emptyslot.OnClickEmptySlot(_ => ClearSelect());
            }
        }

       //UpdateCategoryBtnColors();
    }

    // 아이템 상세 정보 표시
    private void ShowItemDetails(ItemSlot slot)
    {
        ItemStatus status = slot.Status;

        selectSlot = slot;
        //clickedSlot.SelectSlot(true);

        infoPanel.SetActive(true);
        itemName.text = status.Data.Name;

        var detail = new StringBuilder();

        // 아이템 분류에 따른 상세 정보 표시
        EquipItemData equip = status.GetDataAs<EquipItemData>();
        if (equip != null)
        {
            foreach (ItemValue v in equip.Values)
            {
                detail.AppendLine($"{v.Stat} + {v.Value}");
            }
        }
        else
        {
            ConsumeItemData consume = status.GetDataAs<ConsumeItemData>();
            if (consume != null)
            {
                foreach (ItemValue v in consume.Values)
                {
                    detail.AppendLine($"{v.Stat} + {v.Value}");
                }
            }
            else
            {
                QuestItemData quest = status.GetDataAs<QuestItemData>();
                if (quest != null)
                {
                    detail.AppendLine(quest.Description);
                }
            }
        }

        itemInfo.text = detail.ToString();

        SetButtons();
    }

    // 장착/사용, 해제/버리기 버튼 세팅
    private void SetButtons()
    {
        if (selectSlot.Status.GetDataAs<QuestItemData>() != null)
        {
            ResetButton();
            cancelBtn.GetComponent<Image>().color = Color.white;

            return;
        }

        actionBtn.gameObject.SetActive(true);
        cancelBtn.gameObject.SetActive(true);

        if (selectSlot.Status.GetDataAs<ConsumeItemData>() != null)
        {
            actionBtn.GetComponentInChildren<TextMeshProUGUI>().text = "사용하기";
            cancelBtn.GetComponentInChildren<TextMeshProUGUI>().text = "버리기";
            cancelBtn.GetComponent<Image>().color = new Color(1, 1, 1, 0.7f);
        }
        else if (selectSlot.Status.GetDataAs<EquipItemData>() != null)
        {
            actionBtn.GetComponentInChildren<TextMeshProUGUI>().text = "장착하기";
            cancelBtn.GetComponentInChildren<TextMeshProUGUI>().text = "해제하기";
            cancelBtn.GetComponent<Image>().color = Color.white;
        }

    }

    // 장착/사용, 해제/버리기 버튼 비활성화
    private void ResetButton()
    {
        actionBtn.gameObject.SetActive(false);
        cancelBtn.gameObject.SetActive(false);
        actionBtn.GetComponentInChildren<TextMeshProUGUI>().text = "";
        cancelBtn.GetComponentInChildren<TextMeshProUGUI>().text = "";
    }

    private void OnEquipButton()
    {
        // equip ( 플레이어 );
        //selectSlot.Status.GetDataAs<EquipItemData>()?.Equip();
    }

    private void OnActionClicked()
    {
        if (selectSlot == null) return;

        switch (selectSlot.Data.GetCategory())
        {
            case E_CategoryType.Equip when selectSlot.Stack > 0:

                // 기존 장착 템 인벤토리로
                var eqSlot = equipSlots[selectSlot.EquipType];
                if (eqSlot.HasData())
                {
                    var nowEquip = eqSlot.Data;
                    eqSlot.Clear();
                    inventory.AddItem(selectSlot.Data);
                    player?.Unequip(nowEquip);
                }

                // 새아이템 장착
                eqSlot.SetupEquip(selectedItem, data => ShowItemDetails(data, true, eqSlot));
                inventory.RemoveItem(selectedItem.Data);
                player?.Equip(selectedItem);
                ClearSelect();
                break;

            case E_CategoryType.Consume:
                // 사용
                selectedData.OnUse();
                inventory.RemoveItem(selectedData, 1);
                player?.Use(selectedData);

                if (inventory.GetCount(selectedData) == 0)
                {
                    ClearSelect();
                }
                break;

            case E_CategoryType.Quest:
                // 건네주기
                player?.GiveQuestItem(selectedData);
                inventory.RemoveItem(selectedData, 1);
                ClearSelect();
                break;
        }

        RefreshUI();
    }

    // 해제하기/버리기 버튼 클릭 시
    private void OnCancelClicked()
    {
        if (selectedItem == null) return;

        if (selectedIsEquipSlot)
        {
            // 해제
            var slot = equipSlots[selectedItem.EquipType];
            slot.Clear();
            inventory.AddItem(selectedItem.Data);
            player?.Unequip(selectedItem);
        }
        else if (selectedItem.GetCategory() != E_CategoryType.Quest)
        {
            // 버리기
            inventory.RemoveItem(selectedItem.Data);
        }

        RefreshUI();
        ClearSelect();
    }
}


//[Header("카테고리 클릭시 컬러")] //추후 에셋에 따라 변동가능성있음
//[SerializeField] private Color CategorySelectedColor = Color.green;
//[SerializeField] private Color CategoryNormalColor = Color.white;

//[Header("슬롯 강조 컬러")]
//[SerializeField] private Color slotSelectedColor = new Color(0.2f, 0.2f, 0.2f);
//[SerializeField] private Color slotNormalColor = Color.white;


//[Header("슬롯 초과시 팝업")]
//[SerializeField] private GameObject popup;
//[SerializeField] private CanvasGroup canvasGroup;
//[SerializeField] private TextMeshProUGUI popupText;

//private void Start()
//{
//    if (inventory == null)
//    {
//        Debug.LogError("[InventoryUI] PlayerInventory를 찾을 수 없습니다.");
//        enabled = false;
//        return;
//    }

//    // UI 컴포넌트 확인
//    var missing = GetType()
//        .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
//        .Where(fields => fields.GetCustomAttributes(typeof(SerializeField), true).Any())
//        .Where(fields => fields.GetValue(this) == null)
//        .Select(fields => fields.Name)
//        .ToArray();

//    if (missing.Length > 0)
//    {
//        Debug.LogError($"[InventoryUI] Inspector에 할당 필요: {string.Join(", ", missing)}");
//        enabled = false;
//        return;
//    }

//    // 슬롯 초기화
//    SelectedSlotColor = slotSelectedColor;
//    NormalSlotColor = slotNormalColor;

//    // 버튼 이벤트
//    //categoryAll.onClick.AddListener(() => ChangeCategory(EItemCategory.All));
//    //openBtn.onClick.AddListener(Show);
//    //closeBtn.onClick.AddListener(Hide);
//    //categoryEquip.onClick.AddListener(() => ChangeCategory(EItemCategory.Equipment));
//    //categoryConsum.onClick.AddListener(() => ChangeCategory(EItemCategory.Consumable));
//    //categoryQuest.onClick.AddListener(() => ChangeCategory(EItemCategory.Quest));
//    //actionBtn.onClick.AddListener(OnActionClicked);
//    //cancelBtn.onClick.AddListener(OnCancelClicked);

//    // 장착 아이템 슬롯 초기화 (Weapon=0, Hat=1, Accessory=2, Clothes=3, Shoes=4)
//    var children = equipPanel.GetComponentsInChildren<ItemSlot>(true);
//    if (children.Length < Enum.GetValues(typeof(E_EquipType)).Length)
//    {
//        Debug.LogError("[InventoryUI] EquipPanel 자식이 5개여야합니다.");
//    }

//    for (int i = 0; i < children.Length; i++)
//    {
//        var type = (E_EquipType)i;
//        var slot = children[i];

//        equipSlots[type] = slot;
//        slot.Clear();
//        slot.OnClickEmptySlot(status => ShowItemDetails(status, true, slot));

//    }

//    // 팝업, 상세 패널 초기화
//    canvasGroup.alpha = 0;
//    popup.SetActive(false);
//    detailPanel.SetActive(false);

//    // 플레이어 찾기
//    player = GameManager.Instance.Player.GetComponent<Player>();
//    if (player == null)
//    {
//        Debug.LogWarning("[InventoryUI] Player를 찾을 수 없습니다. Equip/Use 호출이 동작하지 않습니다.");
//    }

//    // 이벤트 구독 및 UI 갱신
//    //inventory.OnInventoryChanged += RefreshUI;
//    //inventory.OnAddFail += ShowCapacityPopup;
//}

//// 인벤토리 변경 이벤트 구독 해제
//private void OnDisable()
//{
//    if (inventory != null)
//    {
//        //inventory.OnInventoryChanged -= RefreshUI;
//        //inventory.OnInventoryChanged -= Show;
//    }
//}

//// 인벤토리 UI 열기
//public void Show()
//{
//    gameObject.SetActive(true);
//    equipPanel.gameObject.SetActive(true);
//}

//// 인벤토리 UI 닫기
//public void Hide()
//{
//    gameObject.SetActive(false);
//    equipPanel.gameObject.SetActive(false);
//    detailPanel.SetActive(false);
//}

//// 클릭해제 완전 초기화
//private void ClearSelect()
//{
//    currentSlot?.UnSelectSlot();
//    currentSlot = null;

//    detailPanel.SetActive(false);
//    actionBtn.interactable = false;
//    cancelBtn.interactable = false;
//}

//// 카테고리 변경
//private void ChangeCategory(E_CategoryType type)
//{
//    if (category == type) return;
//    ClearSelect();
//    category = type;
//    RefreshUI();
//    UpdateCategoryBtnColors();
//}

//private void UpdateCategoryBtnColors()
//{
//    all.GetComponent<Image>().color = (category == E_CategoryType.All) ? CategorySelectedColor : CategoryNormalColor;
//    equip.GetComponent<Image>().color = (category == E_CategoryType.Equip) ? CategorySelectedColor : CategoryNormalColor;
//    consume.GetComponent<Image>().color = (category == E_CategoryType.Consume) ? CategorySelectedColor : CategoryNormalColor;
//    quest.GetComponent<Image>().color = (category == E_CategoryType.Quest) ? CategorySelectedColor : CategoryNormalColor;
//}



//// 장착/사용 버튼 클릭 시


//// 슬롯 개수 초과시 팝업 표시
//private void ShowCapacityPopup(string message)
//{
//    popupText.text = message;
//    StopAllCoroutines(); // 연출 효과를 위함
//    StartCoroutine(PopupCoroutine());
//}

//private IEnumerator PopupCoroutine()
//{
//    popup.SetActive(true);
//    yield return Fade(0, 1, 0.5f);
//    yield return new WaitForSeconds(1.5f);
//    yield return Fade(1, 0, 0.5f);
//    popup.SetActive(false);
//}

//private IEnumerator Fade(float from, float to, float duration)
//{
//    float time = 0f;
//    while (time < duration)
//    {
//        time += Time.deltaTime;
//        canvasGroup.alpha = Mathf.Lerp(from, to, time / duration);
//        yield return null;
//    }
//    canvasGroup.alpha = to;
//}