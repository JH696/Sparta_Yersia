using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class InventoryUI : MonoBehaviour
{
    [Header("인벤토리 UI 열기/닫기")]
    [SerializeField] private Button openBtn;
    [SerializeField] private Button closeBtn;

    [Header("Slot 부모, 프리팹 할당")]
    [SerializeField] private Transform SlotContent;
    [SerializeField] private GameObject SlotPrefab;

    [Header("아이템 정보")]
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemEffectText;
    [SerializeField] private Button equipBtn;
    [SerializeField] private Button discardBtn;

    private PlayerInventory inventory;
    private ItemData[] itemDB; // Resources 폴더에서 가져오는 방식으로 변경
    private ItemData selectedData;

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

        // 버튼 이벤트 바인딩
        if (openBtn == null || closeBtn == null)
        {
            Debug.LogError("[InventoryUI] Open/Close 버튼이 할당되지 않았습니다.");
            return;
        }
        openBtn.onClick.AddListener(Show);
        closeBtn.onClick.AddListener(Hide);

        // 상세 패널 초기화
        if (detailPanel == null || itemNameText == null || itemEffectText == null || equipBtn == null || discardBtn == null)
        {
            Debug.LogError("[InventoryUI] 상세 패널 UI 요소가 누락되었습니다.");
            return;
        }
        detailPanel.SetActive(false);
        equipBtn.onClick.AddListener(OnEquipClicked);
        discardBtn.onClick.AddListener(OnDiscardClicked);
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

        foreach (Transform child in SlotContent)
        {
            Destroy(child.gameObject); // UI 비활성화 시 기존 슬롯 제거
        }

        detailPanel.SetActive(false); // 상세 패널 숨김
    }

    // 인벤토리 UI 열기
    public void Show()
    {
        gameObject.SetActive(true);
    }

    // 인벤토리 UI 닫기
    public void Hide()
    {
        gameObject.SetActive(false);
        detailPanel.SetActive(false); // 상세 패널 숨김
    }

    // 슬롯 UI 갱신
    public void RefreshUI()
    {
        // 기존 슬롯 제거
        foreach (Transform child in SlotContent)
        {
            Destroy(child.gameObject);
        }

        // 인벤토리 아이템 데이터 가져오기
        var invenItems = inventory.GetAllItems();
        foreach (var keyValue in invenItems)
        {
            var id = keyValue.Key;
            var count = keyValue.Value;

            // DB에서 ItemData 찾기
            var data = Array.Find(itemDB, db => db.ID == id);
            if (data == null) continue;

            // 아이템 슬롯 생성 및 Setup(설정)
            var slotObj = Instantiate(SlotPrefab, SlotContent);
            var slot = slotObj.GetComponent<ItemSlot>();
            slot.Setup(data, count, OnSlotClicked);
        }
    }

    // 클릭시 아이템 상세정보(효과) 표시
    private void OnSlotClicked(ItemData data)
    {
        ShowItemDetails(data);
    }
    private void ShowItemDetails(ItemData data)
    {
        if (data == null) return;
        selectedData = data;

        // 패널 활성화
        detailPanel.SetActive(true);

        // 텍스트 설정
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
        if (selectedData == null) return;

        selectedData.OnEquip();
        inventory.RemoveItem(selectedData, 1); // 장착 후 아이템 제거
        RefreshUI(); // UI 갱신
    }

    // 버리기 버튼 클릭 시
    private void OnDiscardClicked()
    {
        if (selectedData == null) return;
        inventory.RemoveItem(selectedData, 1); // 아이템 제거
        RefreshUI(); // UI 갱신
    }
}
