using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System.Collections.Generic;
using System.Collections;

public class InventoryUI : MonoBehaviour
{
    [Header("인벤토리 Slot")]
    [SerializeField] private Transform slotContent;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private int slotCount = 20;
    
    [Header("장착아이템 Slot")]
    [SerializeField] private Transform equipSlotContent;
    [SerializeField] private GameObject equipSlotPrefab;

    [Header("slotCount초과시 팝업")]
    [SerializeField] private GameObject capacityPopup;
    [SerializeField] private TextMeshProUGUI capacityPopupText;

    private GameObject detailPanel, equipPanel;
    private TextMeshProUGUI itemNameText, itemEffectText;
    private Button openBtn, closeBtn,
                   categoryEquip, categoryConsum, categoryQuest,
                   equipBtn, discardBtn;

    private PlayerInventory inventory;
    private ItemData[] itemDB;
    private ItemData selectedData;

    private List<ItemSlot> inventorySlots = new List<ItemSlot>();
    private Dictionary<EEquipType, ItemSlot> equipSlots = new Dictionary<EEquipType, ItemSlot>();
    private EItemCategory currentCategory = EItemCategory.Equipment;

    private void Awake()
    {
        // PlayerInventory 컴포넌트
        inventory = FindObjectOfType<PlayerInventory>();
        if (inventory == null)
        {
            enabled = false;
            return;
        }

        // ItemData SO
        itemDB = Resources.LoadAll<ItemData>("ItemDatas");
        if (itemDB.Length == 0)
        {
            Debug.LogWarning("[InventoryUI] Resources/ItemDatas 폴더에 SO 없음");
        }

        // UI 요소 초기화
        var t = transform;
        openBtn = t.Find("OpenButton").GetComponent<Button>();
        closeBtn = t.Find("CloseButton").GetComponent<Button>();

        categoryEquip = t.Find("Category/EquipButton").GetComponent<Button>();
        categoryConsum = t.Find("Category/ConsumableButton").GetComponent<Button>();
        categoryQuest = t.Find("Category/QuestButton").GetComponent<Button>();

        detailPanel = t.Find("DetailPanel").gameObject;
        itemNameText = t.Find("DetailPanel/ItemNameText").GetComponent<TextMeshProUGUI>();
        itemEffectText = t.Find("DetailPanel/ItemEffectText").GetComponent<TextMeshProUGUI>();

        equipPanel = t.Find("EquipPanel").gameObject;
        equipBtn = t.Find("DetailPanel/EquipButton").GetComponent<Button>();
        discardBtn = t.Find("DetailPanel/DiscardButton").GetComponent<Button>();

        // 버튼 이벤트
        openBtn.onClick.AddListener(Show);
        closeBtn.onClick.AddListener(Hide);

        categoryEquip.onClick.AddListener(() => ChangeCategory(EItemCategory.Equipment));
        categoryConsum.onClick.AddListener(() => ChangeCategory(EItemCategory.Consumable));
        categoryQuest.onClick.AddListener(() => ChangeCategory(EItemCategory.Quest));

        equipBtn.onClick.AddListener(OnEquipClicked);
        discardBtn.onClick.AddListener(OnDiscardClicked);

        // 슬롯 생성
        for (int i = 0; i < slotCount; i++)
        {
            inventorySlots.Add(Instantiate(slotPrefab, slotContent).GetComponent<ItemSlot>());
        }

        foreach (EEquipType type in Enum.GetValues(typeof(EEquipType)))
        {
            equipSlots[type] = Instantiate(equipSlotPrefab, equipSlotContent).GetComponent<ItemSlot>();
        }

        capacityPopup.gameObject.SetActive(false); // 초기에는 팝업 숨김
        detailPanel.SetActive(false); // 상세 패널 숨김
        Hide(); // 초기에는 인벤토리 UI 숨김
    }

    // 인벤토리 변경 이벤트 구독
    private void OnEnable()
    {
        inventory.OnInventoryChanged += RefreshUI;
        RefreshUI();
    }

    // 인벤토리 변경 이벤트 구독 해제
    private void OnDisable()
    {
        inventory.OnInventoryChanged -= RefreshUI;
    }

    // 인벤토리 UI 열기
    public void Show()
    {
        gameObject.SetActive(true);
        equipPanel.SetActive(true);
    }

    // 인벤토리 UI 닫기
    public void Hide()
    {
        gameObject.SetActive(false);
        equipPanel.SetActive(false);
        detailPanel.SetActive(false);
    }

    // 카테고리 변경
    private void ChangeCategory(EItemCategory category)
    {
        currentCategory = category;
        RefreshUI();
    }

    // 슬롯 UI 갱신
    public void RefreshUI()
    {
        var items = inventory.GetAllItems();
        int index = 0;

        foreach (var key in items)
        {
            // DB에서 ItemData 찾기
            var data = Array.Find(itemDB, db => db.ID == key.Key);
            if (data == null || data.Category != currentCategory) continue;

            // 팝업 표시
            if (index >= inventorySlots.Count)
            {
                ShowCapacityPopup("슬롯이 가득 찼습니다.");
                break;
            }

            inventorySlots[index++].Setup(data, key.Value, ShowItemDetails);
        }

        // 나머지 슬롯 비활성화
        for (int i = index; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].Clear();
        }
    }

    private void ShowItemDetails(ItemData data)
    {
        selectedData = data;
        
        detailPanel.SetActive(true);
        closeBtn.gameObject.SetActive(false);

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
    }

    // 장착 버튼 클릭 시
    private void OnEquipClicked()
    {
        if (selectedData == null || selectedData.Category != EItemCategory.Equipment) return;

        equipSlots[selectedData.EquipType].Setup(selectedData, 1, ShowItemDetails);
        selectedData.OnEquip();

        if (!inventory.RemoveItem(selectedData, 1)) return;

        RefreshUI();
        detailPanel.SetActive(false);
        closeBtn.gameObject.SetActive(true);
    }

    // 버리기 버튼 클릭 시
    private void OnDiscardClicked()
    {
        if (selectedData == null) return;
        
        if (selectedData.Category == EItemCategory.Equipment)
        {
            equipSlots[selectedData.EquipType].Clear();
        }

        if (!inventory.RemoveItem(selectedData, 1)) return;
        
        RefreshUI(); // UI 갱신
        detailPanel.SetActive(false);
        closeBtn.gameObject.SetActive(true);
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
        yield return Fade(0, 1, 0.2f);
        yield return new WaitForSeconds(1.5f);
        yield return Fade(1, 0, 0.2f);
        capacityPopup.SetActive(false);
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, time / duration);
            capacityPopup.GetComponent<CanvasGroup>().alpha = alpha;
            yield return null;
        }
        capacityPopup.GetComponent<CanvasGroup>().alpha = to;
        capacityPopup.SetActive(to > 0);
    }
}
