using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("아이템 아이콘")]
    public Image Icon;
    [Header("아이템 수량")]
    public TextMeshProUGUI CountText;
    
    private ItemData itemData;
    private Action<ItemData> onClickAction;

    // 아이템 슬롯 초기화
    public void Setup (ItemData data, int count, Action<ItemData> onClick)
    {
        if (data == null) return;

        itemData = data;
        onClickAction = onClick;

        Icon.sprite = data.Icon;
        Icon.enabled = data.Icon != null; // 아이콘이 없으면 비활성화

        CountText.text = count.ToString(); // 아이템 개수 표시
    }

    // IPointerClickHandler 인터페이스 구현(클릭 이벤트 처리)
    public void OnPointerClick(PointerEventData eventData)
    {
        onClickAction?.Invoke(itemData); // 클릭 시 아이템 데이터 전달
        Debug.Log($"아이템 클릭: {itemData.ItemName}");
    }

    // 아이템 슬롯 비우기
    public void Clear()
    {
        itemData = null;
        Icon.sprite = null;
        Icon.enabled = false;
        CountText.text = string.Empty;
    }
}
