using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [Header("등록된 아이템")]
    [SerializeField] private ItemStatus status;

    [Header("아이템 아이콘")]
    [SerializeField] private Image icon;

    [Header("아이템 수량")]
    [SerializeField] private TextMeshProUGUI stack;

    //[Header("슬롯 배경")]
    //[SerializeField] private Image bgImg;

    public event System.Action<ItemSlot> OnClickSlot; // 슬롯 클릭 이벤트
    public ItemStatus Status => status;

    // 슬롯 초기화
    public void SetItem(ItemStatus status)
    {
        this.status = status;
        status.StatusChanged += UpdateSlot;
        //status.OnEmpty += ClearSlot;
        UpdateSlot();
        GetComponent<Button>().onClick.AddListener(OnClick); // 슬롯 클릭 이벤트 등록
    }

    // 슬롯 업데이트 (자동 호출)
    public void UpdateSlot()
    {
        if (status == null)
        {
            icon.sprite = null;
            stack.text = string.Empty;
            return;
        }

        icon.sprite = status.Data.Icon;
        stack.text = status.Stack > 1 ? status.Stack.ToString() : string.Empty;
    }

    // 슬롯 비우기
    public void ClearSlot()
    {
        if (status != null)
        {
            status.StatusChanged -= UpdateSlot;
            //status.OnEmpty -= ClearSlot;
        }
        status = null;
        icon.sprite = null;
        stack.text = string.Empty;
        GetComponent<Button>().onClick.RemoveListener(OnClick); // 슬롯 클릭 이벤트 제거
    }   

    public void OnClick()
    {
        if (status == null) return;

        OnClickSlot?.Invoke(this); // 슬롯 클릭 이벤트 호출
    }
}
