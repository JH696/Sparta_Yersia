using System;
using UnityEngine;

[System.Serializable]
public class ItemStatus
{
    private ItemInventory inventory; // 아이템이 위치한 인벤토리

    [Header("아이템 데이터")]
    public BaseItem Data;

    [Header("아이템 갯수")]
    public int Stack;

    public bool IsFull => Stack == Data.MaxStack;

    public event Action StatusChanged; // 아이템 상태 변경 이벤트

    public ItemStatus(ItemInventory inventory, BaseItem data)
    {
        this.inventory = inventory;
        Data = data;
        Debug.Log($"[ItemStatus] 아이템 생성: {Data.Name}, ID: {Data.ID}, 스택: {Stack}");
        Stack = 1;
    }

    // 아이템 버리기 메서드
    public void LoseItem(int count)
    {
        Stack -= count;
        
        if (Stack <= 0)
        {
            inventory.RemoveItem(Data);
        }

        StatusChanged?.Invoke();
    }

    // 아이템 중첩 메서드
    public void StackItem(int count)
    {
        if (IsFull) return;

        Stack += count;
    }

    /// <summary>
    /// BaseItem 상속 클래스(EquipItemData, ConsumeItemData...) 반환
    /// 사용법: GetChild<EquipItemData>(), GetChild<ConsumeItemData>() 등
    /// </summary>
    public T GetDataAs<T>() where T : BaseItem
    {
        if (Data is T result)
        {
            return result;
        }
        return null;
    }
}

