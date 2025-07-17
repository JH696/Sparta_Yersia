using System.Collections.Generic;
using System.Text;
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
    //public static Color SelectedSlotColor;
    //public static Color NormalSlotColor;

    [Header("캐릭터")]
    [SerializeField] private CharacterStatus player;

    [Header("인벤토리")]
    [SerializeField] private ItemInventory inventory;

    [Header("현재 카테고리")]
    [SerializeField] private E_CategoryType category = E_CategoryType.All;

    [Header("오픈 / 클로즈 버튼")]
    [SerializeField] private Button openBtn; // UI 매니저
    [SerializeField] private Button closeBtn;

    [Header("카테고리 버튼")]
    [SerializeField] private Button all;
    [SerializeField] private Button equip;
    [SerializeField] private Button consume;
    [SerializeField] private Button quest;

    [Header("사용 / 해제(버리기) 버튼")]
    [SerializeField] private Button useBtn; // 장착/사용 버튼
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

    private ItemSlot selectedSlot;

    public void Start()
    {
        all.onClick.AddListener(() => ChangeCategory(E_CategoryType.All));
        equip.onClick.AddListener(() => ChangeCategory(E_CategoryType.Equip));
        consume.onClick.AddListener(() => ChangeCategory(E_CategoryType.Consume));
        quest.onClick.AddListener(() => ChangeCategory(E_CategoryType.Quest));
        cancelBtn.onClick.AddListener(OnCancelClicked);

        inventory.InventoryChanged += RefreshUI;

        DisplaySlots();

        //장착 아이템 슬롯 초기화(Weapon= 0, Hat= 1, Accessory= 2, Clothes= 3, Shoes= 4)
        ItemSlot[] eSlots = equipPanel.GetComponentsInChildren<ItemSlot>(true);

        for (int i = 0; i < eSlots.Length; i++)
        {
            E_EquipType type = (E_EquipType)i;
            ItemSlot slot = eSlots[i];

            equipSlots[type] = slot;
            slot.SlotClear();
            slot.OnClickEmptySlot(_ => ClearButtons());
        }
    }

    // 인벤토리 변경 이벤트 구독 해제
    private void OnDisable()
    {
        if (inventory != null)
        {
            inventory.InventoryChanged -= RefreshUI;
        }
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

    // 카테고리 변경
    private void ChangeCategory(E_CategoryType type)
    {
        if (category == type) return;
        ClearSelect();
        category = type;
        RefreshUI();
    }

    // 인벤토리 UI 갱신
    public void RefreshUI()
    {
        List<ItemStatus> items = inventory.Items;
        List<ItemStatus> categoryItems = new List<ItemStatus>();

        foreach (ItemStatus item in items)
        {
            if (category == E_CategoryType.All || item.Data.GetCategory() == category)
            {
                categoryItems.Add(item);
            }
        }

        if (categoryItems.Count <= 0) return;

        for (int i = 0; i < categoryItems.Count; i++)
        {
            slots[i].SlotClear();

            if (categoryItems[i] != null)
            {
                ItemStatus itemStatus = categoryItems[i];
                slots[i].SetSlot(itemStatus, _ => ShowItemDetails(slots[i]));
            }
            else
            {
                ItemSlot emptyslot = slots[i];
                emptyslot.OnClickEmptySlot(_ => ClearButtons());
            }
        }

        //UpdateCategoryBtnColors();
    }

    // 아이템 상세 정보 표시
    private void ShowItemDetails(ItemSlot slot)
    {
        ItemStatus status = slot.Status;

        if (status == null) return;

        selectedSlot = slot;

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
                    detail.AppendLine(quest.Description ?? "설명이 없습니다.");
                }
            }
        }

        itemInfo.text = detail.ToString();
        SetButtons();
    }

    // 장착/사용, 해제/버리기 버튼 세팅
    private void SetButtons()
    {
        if (selectedSlot.Status.GetDataAs<QuestItemData>() != null)
        {
            ClearButtons();
            cancelBtn.GetComponent<Image>().color = Color.white;

            return;
        }

        useBtn.gameObject.SetActive(true);
        cancelBtn.gameObject.SetActive(true);

        useBtn.onClick.RemoveAllListeners(); // 버튼 이벤트 초기화

        if (selectedSlot.Status.GetDataAs<ConsumeItemData>() != null)
        {
            useBtn.GetComponentInChildren<TextMeshProUGUI>().text = "사용하기";
            cancelBtn.GetComponentInChildren<TextMeshProUGUI>().text = "버리기";

            useBtn.onClick.AddListener(OnEquipButton); // 버튼 이벤트 변경

            cancelBtn.GetComponent<Image>().color = new Color(1, 1, 1, 0.7f);
        }
        else if (selectedSlot.Status.GetDataAs<EquipItemData>() != null)
        {
            useBtn.GetComponentInChildren<TextMeshProUGUI>().text = "장착하기";
            cancelBtn.GetComponentInChildren<TextMeshProUGUI>().text = "해제하기";

            useBtn.onClick.AddListener(OnUseButton); // 버튼 이벤트 변경

            cancelBtn.GetComponent<Image>().color = Color.white;
        }
    }

    // 장착/사용, 해제/버리기 버튼 비활성화
    private void ClearButtons()
    {
        useBtn.gameObject.SetActive(false);
        cancelBtn.gameObject.SetActive(false);
        useBtn.GetComponentInChildren<TextMeshProUGUI>().text = "";
        cancelBtn.GetComponentInChildren<TextMeshProUGUI>().text = "";
    }

    // 장착 버튼 클릭 시
    private void OnEquipButton()
    {
        EquipItemData item = selectedSlot.Status.GetDataAs<EquipItemData>();
        if (item == null) return;

        ItemSlot equipslot = equipSlots[item.Type];

        equipslot.SetSlot(selectedSlot.Status, _ => ShowItemDetails(equipslot));
        item.Equip(player);
        selectedSlot.Status.UseItem();
        RefreshUI();
    }

    // 사용 버튼 클릭 시
    private void OnUseButton()
    {
        ConsumeItemData item = selectedSlot.Status.GetDataAs<ConsumeItemData>();

        if (item == null) return;

        item.Consume(player);
        selectedSlot.Status.UseItem();
        RefreshUI();
    }

    // 해제하기/버리기 버튼 클릭 시
    private void OnCancelClicked()
    {
        if (selectedSlot.Status == null) return;

        if (equipSlots.ContainsValue(selectedSlot))
        {
            if (selectedSlot.HasData())
            {
                inventory.AddItem(selectedSlot.Status.Data);
                selectedSlot.SlotClear();

            }
        }
        else if (selectedSlot.Status.GetDataAs<ConsumeItemData>() != null)
        {
            inventory.RemoveItem(selectedSlot.Status.Data);
        }

        RefreshUI();
        ClearSelect();
    }

    // 슬롯 선택 해제
    private void ClearSelect()
    {
        selectedSlot = null;
        infoPanel.SetActive(false);
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
//

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