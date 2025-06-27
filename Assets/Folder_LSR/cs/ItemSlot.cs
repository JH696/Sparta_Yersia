using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("아이템 아이콘")]
    [SerializeField] private Image Icon;
    [Header("아이템 수량")]
    [SerializeField] private TextMeshProUGUI CountText;

    private ItemData itemData;
    private Action<ItemData> onClickAction;

    private void Awake()
    {
        if (Icon == null)
        {
            Icon = GetComponentInChildren<Image>();
        }
        if (CountText == null)
        {
            CountText = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    // 아이템 슬롯 초기화
    public void Setup(ItemData data, int count, Action<ItemData> onClick)
    {
        if (data == null)
        {
            Clear();
            return;
        }

        itemData = data;
        onClickAction = onClick;

        Icon.sprite = data.Icon;
        Icon.enabled = true; // 아이콘이 없으면 비활성화

        CountText.text = count > 1 ? count.ToString() : string.Empty; // 아이템 개수 표시
        CountText.gameObject.SetActive(count > 1);
    }

    // 슬롯 아이템 세팅 확인
    public bool HasData()
    {
        return itemData != null;
    }

    // 장착 아이템 슬롯 초기화
    public void SetupEquip(ItemData data, Action<ItemData> onClick)
    {
        Setup(data, 1, onClick); // 장착 아이템은 개수가 1개로 고정
        CountText.gameObject.SetActive(false);
    }

    // IPointerClickHandler 인터페이스 구현(클릭 이벤트 처리)
    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemData == null || onClickAction == null) return;

        onClickAction(itemData);

        Debug.Log($"아이템 클릭: {itemData.ItemName}");
    }

    // 아이템 슬롯 비우기
    public void Clear()
    {
        itemData = null;
        onClickAction = null;

        if (Icon != null)
        {
            Icon.sprite = null;
            Icon.enabled = false;
        }

        if (CountText != null)
        {
            CountText.text = string.Empty;
            CountText.gameObject.SetActive(false);
        }
    }
}
