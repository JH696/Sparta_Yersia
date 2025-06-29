using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static Color SelectedSlotColor;
    public static Color NormalSlotColor;

    [Header("슬롯 강조 컬러")]
    [SerializeField] private Color slotSelectedColor = Color.white;
    [SerializeField] private Color slotNormalColor = new Color(0.2f, 0.2f, 0.2f);

    [Header("버튼들")]
    [SerializeField] private Button openBtn;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button categoryEquip;
    [SerializeField] private Button categoryConsum;
    [SerializeField] private Button categoryQuest;
    [SerializeField] private Button actionBtn; // 장착/사용 버튼
    [SerializeField] private Button cancelBtn; // 해제/버리기 버튼

    [Header("카테고리 클릭시 컬러")] //추후 에셋에 따라 변동가능성있음
    [SerializeField] private Color selectedColor = Color.green;
    [SerializeField] private Color normalColor = Color.white;

    [Header("상세 패널")]
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemEffectText;

    [Header("인벤토리 Slot")]
    [SerializeField] private Transform slotContent;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private int slotCount = 20;

    [Header("장착아이템 Slot")]
    [SerializeField] private Transform equipPanel;

    [Header("슬롯 초과시 팝업")]
    [SerializeField] private GameObject capacityPopup;
    [SerializeField] private CanvasGroup capacityCanvasGroup;
    [SerializeField] private TextMeshProUGUI capacityPopupText;

    private PlayerInventory inventory;
    private ItemData[] itemDB;
    private ItemData selectedData;
    private bool selectedIsEquipSlot;

    private List<ItemSlot> slots = new();
    private Dictionary<EEquipType, ItemSlot> equipSlots = new();
    private EItemCategory currentCategory = EItemCategory.Equipment;

    private ItemSlot currentSlot;
    private Test_PlayerCharacter player;

    private void Awake()
    {
        // PlayerInventory 컴포넌트
        inventory = FindObjectOfType<PlayerInventory>();
        if (inventory == null)
        {
            Debug.LogError("[InventoryUI] PlayerInventory를 찾을 수 없습니다.");
            enabled = false;
            return;
        }

        // ItemData SO
        itemDB = Resources.LoadAll<ItemData>("ItemDatas");
        if (itemDB.Length == 0)
        {
            Debug.LogWarning("[InventoryUI] Resources/ItemDatas 폴더에 SO파일이 없습니다.");
        }

        // UI 컴포넌트 확인
        var missingFields = GetType()
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(f => f.GetCustomAttributes(typeof(SerializeField), true).Any())
            .Where(f => f.GetValue(this) == null)
            .Select(f => f.Name)
            .ToArray();

        if (missingFields.Length > 0)
        {
            Debug.LogError($"[InventoryUI] Inspector에 할당 필요: {string.Join(", ", missingFields)}");
            enabled = false;
            return;
        }

        // 버튼 이벤트
        openBtn.onClick.AddListener(Show);
        closeBtn.onClick.AddListener(Hide);
        categoryEquip.onClick.AddListener(() => ChangeCategory(EItemCategory.Equipment));
        categoryConsum.onClick.AddListener(() => ChangeCategory(EItemCategory.Consumable));
        categoryQuest.onClick.AddListener(() => ChangeCategory(EItemCategory.Quest));
        actionBtn.onClick.AddListener(OnActionClicked);
        cancelBtn.onClick.AddListener(OnCancelClicked);

        // 슬롯 생성
        for (int i = 0; i < slotCount; i++)
        {
            var slot = Instantiate(slotPrefab, slotContent).GetComponent<ItemSlot>();
            slot.Clear();
            slots.Add(slot);
        }

        // 장착 아이템 슬롯 초기화 (Weapon=0, Hat=1, Accessory=2, Clothes=3, Shoes=4)
        var children = equipPanel.GetComponentsInChildren<ItemSlot>(true);
        if (children.Length < Enum.GetValues(typeof(EEquipType)).Length)
        {
            Debug.LogError("[InventoryUI] EquipPanel 자식이 5개여야합니다.");
        }

        for (int i = 0; i < children.Length; i++)
        {
            var type = (EEquipType)i;
            var slot = children[i];

            equipSlots[type] = slot;
            slot.Clear();
            slot.OnClickEmptySlot(data => ShowItemDetails(data, true, slot));

        }

        // 팝업, 상세 패널 초기화
        capacityCanvasGroup.alpha = 0;
        capacityPopup.SetActive(false);
        detailPanel.SetActive(false);

        // 플레이어 찾기
        player = FindObjectOfType<Test_PlayerCharacter>();
        if (player == null)
        {
            Debug.LogWarning("[InventoryUI] Player를 찾을 수 없습니다. Equip/Use 호출이 동작하지 않습니다.");
        }

        inventory.OnInventoryChanged += RefreshAndShow;
        inventory.OnAddFail += ShowCapacityPopup;
        RefreshAndShow();

        // 컬러 초기화
        SelectedSlotColor = slotSelectedColor;
        NormalSlotColor = slotNormalColor;
        UpdateCategoryBtnColors();
    }

    // 인벤토리 변경 이벤트 구독 해제
    private void OnDestroy()
    {
        //if (inventory == null) return;

        if (inventory != null)
        {
            inventory.OnInventoryChanged -= RefreshAndShow;
        }
    }

    // 인벤토리 변경 시 UI 갱신 후 열기
    private void RefreshAndShow()
    {
        RefreshUI();
        Show();
    }

    // 인벤토리 UI 열기
    public void Show()
    {
        gameObject.SetActive(true);
        equipPanel.gameObject.SetActive(true);
    }

    // 인벤토리 UI 닫기
    public void Hide()
    {
        gameObject.SetActive(false);
        equipPanel.gameObject.SetActive(false);
        detailPanel.SetActive(false);
    }

    // 카테고리 변경
    private void ChangeCategory(EItemCategory category)
    {
        if (currentCategory == category) return;
        ClearSelect();
        currentCategory = category;
        RefreshUI();
        UpdateCategoryBtnColors();
    }

    private void UpdateCategoryBtnColors()
    {
        categoryEquip.GetComponent<Image>().color = (currentCategory == EItemCategory.Equipment) ? selectedColor : normalColor;
        categoryConsum.GetComponent<Image>().color = (currentCategory == EItemCategory.Consumable) ? selectedColor : normalColor;
        categoryQuest.GetComponent<Image>().color = (currentCategory == EItemCategory.Quest) ? selectedColor : normalColor;
    }

    // 클릭해제 완전 초기화
    private void ClearSelect()
    {
        currentSlot?.UnSelectSlot();
        currentSlot = null;

        detailPanel.SetActive(false);
        actionBtn.interactable = false;
        cancelBtn.interactable = false;
    }

    // 슬롯 UI 갱신
    public void RefreshUI()
    {
        var itemsDict = inventory.GetAllItems();
        int index = 0;

        foreach (var pair in itemsDict)
        {
            // DB에서 ItemData 찾기
            var data = Array.Find(itemDB, so => so.ID == pair.Key);
            if (data == null || data.Category != currentCategory) continue;

            // 팝업 표시
            if (index >= slots.Count)
            {
                ShowCapacityPopup("슬롯이 가득 찼습니다.");
                break;
            }

            var slot = slots[index];
            slot.Clear();
            slot.Setup(data, pair.Value, _ => ShowItemDetails(data, false, slot));

            index++;
        }

        // 나머지 슬롯 비활성 및 클릭 시 상세패널 닫기
        for (int i = index; i < slots.Count; i++)
        {
            var emptyslot = slots[i];
            emptyslot.Clear();
            emptyslot.OnClickEmptySlot(_ =>
            {
                currentSlot?.UnSelectSlot();
                currentSlot = null;
                detailPanel.SetActive(false);
            });
        }

        UpdateCategoryBtnColors();
    }

    // 아이템 상세 정보 표시
    private void ShowItemDetails(ItemData data, bool isEquipSlot, ItemSlot clickedSlot)
    {
        currentSlot?.UnSelectSlot();
        currentSlot = clickedSlot;
        clickedSlot.SelectSlot(true);

        selectedData = data;
        selectedIsEquipSlot = isEquipSlot;
        detailPanel.SetActive(true);

        itemNameText.text = data.ItemName;
        var stringBuilder = new StringBuilder();
        foreach (var stat in data.ItemStats)
        {
            if (stat.value != 0)
            {
                stringBuilder.AppendLine($"{stat.eStat}: {stat.value}");
            }
        }
        itemEffectText.text = stringBuilder.ToString();

        if (data.Category == EItemCategory.Quest)
        {
            // 퀘스트 아이템
            actionBtn.gameObject.SetActive(true);
            actionBtn.GetComponentInChildren<TextMeshProUGUI>().text = "건네주기";
            actionBtn.interactable = true;

            // 해제/버리기 버튼
            cancelBtn.gameObject.SetActive(true);
            cancelBtn.GetComponentInChildren<TextMeshProUGUI>().text = "버리기";
            cancelBtn.interactable = false;
            var img = cancelBtn.GetComponent<Image>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0.7f);
        }
        else if (isEquipSlot)
        {
            // 장착판넬
            actionBtn.gameObject.SetActive(false);

            cancelBtn.gameObject.SetActive(true);
            cancelBtn.GetComponentInChildren<TextMeshProUGUI>().text = "해제하기";
            cancelBtn.interactable = true;
            cancelBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        else
        {
            // 인벤토리판넬
            actionBtn.gameObject.SetActive(true);
            actionBtn.GetComponentInChildren<TextMeshProUGUI>().text =
                data.Category == EItemCategory.Equipment ? "장착하기" : "사용하기";
            actionBtn.interactable = true;

            cancelBtn.gameObject.SetActive(true);
            cancelBtn.GetComponentInChildren<TextMeshProUGUI>().text = "버리기";
            cancelBtn.interactable = true;
            cancelBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }

    // 장착/사용 버튼 클릭 시
    private void OnActionClicked()
    {
        if (selectedData == null) return;

        switch (selectedData.Category)
        {
            case EItemCategory.Equipment when inventory.GetCount(selectedData) > 0:
                // 기존 장착 템 인벤토리로
                var eqSlot = equipSlots[selectedData.EquipType];
                if (eqSlot.HasData())
                {
                    var now = eqSlot.Data;
                    eqSlot.Clear();
                    inventory.AddItem(now, 1);
                    player?.Unequip(now);
                }

                // 새아이템 장착
                eqSlot.SetupEquip(selectedData, data => ShowItemDetails(data, true, eqSlot));
                inventory.RemoveItem(selectedData, 1);
                player?.Equip(selectedData);

                // 선택 해제
                ClearSelect();
                break;

            case EItemCategory.Consumable:
                // 사용
                selectedData.OnUse();
                inventory.RemoveItem(selectedData, 1);
                player?.Use(selectedData);

                if (inventory.GetCount(selectedData.ID) == 0)
                {
                    ClearSelect();
                }
                break;

            case EItemCategory.Quest:
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
        if (selectedData == null) return;

        if (selectedIsEquipSlot)
        {
            // 해제
            var slot = equipSlots[selectedData.EquipType];
            slot.Clear();
            inventory.AddItem(selectedData, 1);
            player?.Unequip(selectedData);
        }
        else if (selectedData.Category != EItemCategory.Quest)
        {
            // 버리기
            inventory.RemoveItem(selectedData, 1);
        }

        RefreshUI();
        ClearSelect();
    }

    // 슬롯 개수 초과시 팝업 표시
    private void ShowCapacityPopup(string message)
    {
        capacityPopupText.text = message;
        StopAllCoroutines(); // 연출 효과를 위함
        StartCoroutine(PopupCoroutine());
    }

    private IEnumerator PopupCoroutine()
    {
        capacityPopup.SetActive(true);
        yield return Fade(0, 1, 0.5f);
        yield return new WaitForSeconds(1.5f);
        yield return Fade(1, 0, 0.5f);
        capacityPopup.SetActive(false);
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            capacityCanvasGroup.alpha = Mathf.Lerp(from, to, time / duration);
            yield return null;
        }
        capacityCanvasGroup.alpha = to;
    }
}
