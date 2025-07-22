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

    public event System.Action<ItemSlot> OnClickSlot; // 슬롯 클릭 이벤트
    public ItemStatus Status => status;

    // 슬롯 초기화
    public void SetItem(ItemStatus status, PlayerStatus player)
    {
        this.status = status;
        status.StatusChanged += UpdateSlot;
        //status.OnEmpty += ClearSlot;
        UpdateSlot();
        GetComponent<Button>().onClick.AddListener(OnClick); // 슬롯 클릭 이벤트 등록

        if (status.Data is EquipItemData equipData)
        {
            if (player.equipment.FindEquippedItem(equipData))
            {
                ActiveEquipSlot(); // 장비 아이템이 장착된 경우
            }
            else
            {
                DeactiveEquipSlot(); // 장비 아이템이 장착되지 않은 경우
            }
        }
    }

    // 슬롯 업데이트 (자동 호출)
    public void UpdateSlot()
    {
        if (status == null)
        {
            icon.enabled = false;
            icon.sprite = null;
            stack.text = string.Empty;
            Debug.LogWarning(icon);
            return; 
        }

        Debug.Log(status.Data);
        Debug.Log(icon);

        icon.enabled = true;
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
        icon.color = Color.white;
        icon.sprite = null;
        //icon.enabled = false;
        stack.text = string.Empty;
        GetComponent<Button>().onClick.RemoveListener(OnClick); // 슬롯 클릭 이벤트 제거
    }   

    public void ActiveEquipSlot()
    {
        if (status == null) return;

        icon.color = Color.red; // 아이콘 색상 활성화
        stack.text = "E";
    }

    public void DeactiveEquipSlot()
    {
        if (status == null) return;

        icon.color = Color.white;
        stack.text = status.Stack > 1 ? status.Stack.ToString() : string.Empty;
    }

    public void OnClick()
    {
        if (status == null) return;

        OnClickSlot?.Invoke(this); // 슬롯 클릭 이벤트 호출
    }
}
