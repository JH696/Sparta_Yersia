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
    [Header("슬롯 배경")]
    [SerializeField] private Image bgImg;

    private ItemData itemData;
    private Action<ItemData> onClickAction;
    private Color defaultBgColor;

    private void Awake()
    {
        if (Icon == null)
        {
            foreach (var img in GetComponentsInChildren<Image>(true))
            {
                if (img.gameObject != gameObject)
                {
                    Icon = img;
                    break;
                }
            }
        }
        if (CountText == null)
        {
            CountText = GetComponentInChildren<TextMeshProUGUI>(true);
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
        return itemData != null;
    }

    // 인벤토리/장착판넬 공통 슬롯 초기화
    public void Setup(ItemData data, int count, Action<ItemData> onClick)
    {
        if (data == null)
        {
            Clear();
            return;
        }

        itemData = data;
        onClickAction = onClick;

        //if (Icon != null)
        //{
            Icon.sprite = data.Icon;
            Icon.enabled = true; // 아이콘이 없으면 비활성화
        //}

        // 수량 텍스트가 있을 때만 처리해야함. 없으면 무시
        if (CountText != null)
        {
            CountText.text = count > 1 ? count.ToString() : string.Empty; // 아이템 개수 표시
            CountText.gameObject.SetActive(count > 1);
        }
    }

    // 장착판넬 전용 초기화: 수량 X
    public void SetupEquip(ItemData data, Action<ItemData> onClick)
    {
        Setup(data, 1, onClick); // 장착 아이템은 개수가 1개로 고정
        if (CountText != null)
        {
            CountText.gameObject.SetActive(false);
        }
    }

    // IPointerClickHandler 인터페이스 구현(클릭 이벤트 처리)
    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemData == null || onClickAction == null) return;

        onClickAction(itemData);
    }

    // 빈슬롯 클릭 시
    public void OnClickEmptySlot(Action<ItemData> onclick)
    {
        onClickAction = onclick;
    }

    // 아이템 슬롯 비우기
    public void Clear()
    {
        itemData = null;
        onClickAction = null;

        //if (Icon != null)
        //{
            Icon.sprite = null;
            Icon.enabled = false;
        //}

        if (CountText != null)
        {
            CountText.text = string.Empty;
            CountText.gameObject.SetActive(false);
        }

        // 클릭 강조 해제
        if (bgImg != null)
        {
            bgImg.color = defaultBgColor; // 기본 배경색으로 초기화
        }
    }

    // 클릭 강조
    public void SelectSlot()
    {
        if (bgImg != null)
        {
            bgImg.color = new Color(0, 0, 0, 0.6f);
        }
    }

    // 클릭 강조 해제
    public void UnSelectSlot()
    {
        if (bgImg != null)
        {
            bgImg.color = defaultBgColor; // 기본 배경색으로 초기화
        }
    }
}
