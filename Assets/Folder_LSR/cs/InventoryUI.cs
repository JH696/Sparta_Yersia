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
            Debug.LogWarning("[InventoryUI] Resources/ItemDatas 폴더에 SO파일이 없습니다.");
        }

        // UI 컴포넌트 확인
        var missingFields = this.GetType()
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(field => field.GetCustomAttribute(typeof(SerializeField)) != null)
            .Select(field => (field.Name, field.GetValue(this)))
            .Where(tuple => tuple.Item2 == null)
            .ToList();

        if (missingFields.Count > 0)
        {
            var names = string.Join(", ", missingFields.Select(field => field.Name));
            Debug.LogError($"[InventoryUI] Inspector에 {names} 컴포넌트를 할당해주세요.");
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

        // 카테고리 색상
        ChangeCategory(currentCategory);

        // 슬롯 생성
        for (int i = 0; i < slotCount; i++)
        {
            var slot = Instantiate(slotPrefab, slotContent).GetComponent<ItemSlot>();
            slot.Clear();
            inventorySlots.Add(slot);
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
            equipSlots[type] = children[i];
            children[i].Clear();
        }

        // 팝업, 상세 패널 초기화
        capacityCanvasGroup.alpha = 0;
        capacityPopup.SetActive(false);
        detailPanel.SetActive(false);

        inventory.OnInventoryChanged += RefreshAndShow;
        RefreshAndShow();
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
        currentCategory = category;
        RefreshUI();

        // 버튼 색상 변경
        categoryEquip.GetComponent<Image>().color = (category == EItemCategory.Equipment) ? selectedColor : normalColor;
        categoryConsum.GetComponent<Image>().color = (category == EItemCategory.Consumable) ? selectedColor : normalColor;
        categoryQuest.GetComponent<Image>().color = (category == EItemCategory.Quest) ? selectedColor : normalColor;
    }

    // 슬롯 UI 갱신
    public void RefreshUI()
    {
        var itemsDict = inventory.GetAllItems();
        int index = 0;

        foreach (var key in itemsDict)
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

            var slot = inventorySlots[index];
            slot.Clear();
            slot.Setup(data, key.Value, _ => ShowItemDetails(data, false));

            index++;
        }

        // 나머지 슬롯 비활성화
        for (int i = index; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].Clear();
        }
    }

    private void ShowItemDetails(ItemData data, bool isEquipSlot)
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

        // 버튼 세팅
        actionBtn.gameObject.SetActive(!isEquipSlot);
        cancelBtn.gameObject.SetActive(true);
        if (!isEquipSlot)
        {
            actionBtn.GetComponentInChildren<TextMeshProUGUI>().text =
                (data.Category == EItemCategory.Equipment) ? "장착하기" : "사용하기";

            cancelBtn.GetComponentInChildren<TextMeshProUGUI>().text = "버리기";
        }
        else
        {
            actionBtn.gameObject.SetActive(false);
            cancelBtn.GetComponentInChildren<TextMeshProUGUI>().text = "해제하기";
        }   
    }

    // 장착 버튼 클릭 시
    private void OnActionClicked()
    {
        if (selectedData == null) return;

        if (selectedData.Category == EItemCategory.Equipment && inventory.GetCount(selectedData) > 0)
        {
            // 장착
            var slot = equipSlots[selectedData.EquipType];
            slot.Clear();
            slot.SetupEquip(selectedData, _ => ShowItemDetails(selectedData, true));
            inventory.RemoveItem(selectedData, 1); // 아이템 개수 감소
        }
        else if (selectedData.Category == EItemCategory.Consumable)
        {
            // 소모품 사용
            selectedData.OnUse();
            inventory.RemoveItem(selectedData, 1);
        }
        else if (selectedData.Category == EItemCategory.Quest)
        {
            // 퀘스트 아이템 구체적으로 정하지 않음 - 확인 필요
            Debug.LogWarning("퀘스트 아이템은 장착할 수 없습니다.");
            return;
        }

        RefreshUI();
        detailPanel.SetActive(false);
        closeBtn.gameObject.SetActive(true);
    }

    // 버리기 버튼 클릭 시
    private void OnCancelClicked()
    {
        if (selectedData == null) return;

        bool isCurEquip = equipSlots[selectedData.EquipType].HasData();
        if (isCurEquip && inventory.GetCount(selectedData) == 0)
        {
            // 장착 해제
            var slot = equipSlots[selectedData.EquipType];
            slot.Clear();
            inventory.AddItem(selectedData, 1);
        }
        else
        {
            // 버리기
            inventory.RemoveItem(selectedData, 1);
        }

        RefreshUI();
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
