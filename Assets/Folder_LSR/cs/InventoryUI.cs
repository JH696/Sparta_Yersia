using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System.Collections.Generic;
using System.Collections;

public class InventoryUI : MonoBehaviour
{
    [Header("버튼들")]
    [SerializeField] private Button openBtn;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button categoryEquip;
    [SerializeField] private Button categoryConsum;
    [SerializeField] private Button categoryQuest;
    [SerializeField] private Button equipBtn;
    [SerializeField] private Button discardBtn;

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

    private List<ItemSlot> inventorySlots = new List<ItemSlot>();
    private Dictionary<EEquipType, ItemSlot> equipSlots = new Dictionary<EEquipType, ItemSlot>();
    private EItemCategory currentCategory = EItemCategory.Equipment;

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
            Debug.LogWarning("[InventoryUI] Resources/ItemDatas 폴더에 SO가 없습니다.");
        }

        // 인스펙터에서 할당해야 할 UI 컴포넌트 확인! - 괴랄해요... 아이디어 좀...
        if (openBtn == null || closeBtn == null
            || categoryEquip == null || categoryConsum == null || categoryQuest == null
            || equipBtn == null || discardBtn == null
            || detailPanel == null || itemNameText == null || itemEffectText == null
            || slotContent == null || slotPrefab == null || equipPanel == null
            || capacityPopup == null || capacityCanvasGroup == null || capacityPopupText == null)
        {
            Debug.LogError("[InventoryUI] Inspector에 모든 UI 컴포넌트를 할당해주세요.");
            enabled = false;
            return;
        }

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

        // 장착 아이템 슬롯 초기화 (기존 동적-> 정적으로 변경)
        var children = equipPanel.GetComponentsInChildren<ItemSlot>(true);
        if (children.Length < Enum.GetValues(typeof(EEquipType)).Length)
        {
            Debug.LogError("[InventoryUI] EquipPanel 자식이 5개여야합니다.");
        }
        for (int i = 0; i < children.Length; i++)
        {
            var type = (EEquipType)i;
            equipSlots[type] = children[i];
        }

        // 팝업, 상세 패널 초기화
        capacityCanvasGroup.alpha = 0;
        capacityPopup.gameObject.SetActive(false);
        detailPanel.SetActive(false);

        inventory.OnInventoryChanged += OnInventoryChanged;
        OnInventoryChanged();

        RefreshUI();
        Hide(); // 초기에는 UI 숨김
    }

    private void OnInventoryChanged()
    {

    }

    // 인벤토리 변경 이벤트 구독
    private void OnEnable()
    {
        inventory.OnInventoryChanged += () => { RefreshUI(); Show(); };
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

        equipSlots[selectedData.EquipType].SetupEquip(selectedData, ShowItemDetails);
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
