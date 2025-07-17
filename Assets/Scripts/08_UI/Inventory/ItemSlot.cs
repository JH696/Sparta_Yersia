using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("등록된 아이템 상태")]
    [SerializeField] private ItemStatus status;

    [Header("아이템 아이콘")]
    [SerializeField] private Image icon;

    [Header("아이템 수량")]
    [SerializeField] private TextMeshProUGUI stack;

    [Header("슬롯 배경")]
    [SerializeField] private Image bgImg;

    public ItemStatus Status => status;

    private Action<ItemStatus> onClickAction;
    private Color defaultBgColor;

    private void Start()
    {
        if (icon == null)
        {
            foreach (var img in GetComponentsInChildren<Image>(true))
            {
                if (img.gameObject != gameObject)
                {
                    icon = img;
                    break;
                }
            }
        }
        if (stack == null)
        {
            stack = GetComponentInChildren<TextMeshProUGUI>(true);
        }

        // 배경 이미지가 없으면 기본 색상 저장
        if (bgImg == null)
        {
            bgImg = GetComponent<Image>();
        }
        else
        {
            defaultBgColor = bgImg.color;
        }
    }

    // 슬롯 아이템 세팅 확인
    public bool HasData()
    {
        return status != null;
    }

    // 인벤토리/장착판넬 공통 슬롯 초기화
    public void SetSlot(ItemStatus status, int count, Action<ItemStatus> onClick)
    {
        if (status == null) return;

        this.status = status;
        onClickAction = onClick;

        icon.sprite = status.Data.Icon;
        icon.enabled = true;
        stack.text = count.ToString();
    }

    // IPointerClickHandler 인터페이스 구현(클릭 이벤트 처리)
    public void OnPointerClick(PointerEventData eventData)
    {
        if (status == null || onClickAction == null) return;

        onClickAction(status);
    }

    // 빈슬롯 클릭 시
    public void OnClickEmptySlot(Action<ItemStatus> onclick)
    {
        onClickAction = onclick;
    }

    // 아이템 슬롯 비우기
    public void SlotClear()
    {
        status = null;
        onClickAction = null;

        icon.sprite = null;
        icon.enabled = false;

        stack.text = string.Empty;

        //SelectSlot(false);
    }

    // 클릭 강조
    //public void SelectSlot(bool on)
    //{
    //    if (bgImg != null)
    //    {
    //        bgImg.color = on ? InventoryUI.SelectedSlotColor : InventoryUI.NormalSlotColor;
    //    }
    //}

    // 클릭 강조 해제
    //public void UnSelectSlot()
    //{
    //    if (bgImg != null)
    //    {
    //        bgImg.color = defaultBgColor; // 기본 배경색으로 초기화
    //    }
    //}
}
