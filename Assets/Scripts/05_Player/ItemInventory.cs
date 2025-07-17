using System;
using System.Collections.Generic;
using UnityEngine;

// 아이템 데이터 + 아이템 상태
public class ItemStatus
{
    public BaseItem Data { get; private set; }
    public int Stack { get; private set; }
    public bool IsFull => Stack == Data.MaxStack;

    public event Action StatusChanged;

    public ItemStatus(BaseItem data)
    {
        Data = data;
        Stack = 1;
    }

    public void UseItem()
    {
        Stack--;
        StatusChanged?.Invoke();
    }

    public void StackItem()
    {
        if (IsFull) return;

        Stack++;
        StatusChanged?.Invoke();
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

public class ItemInventory
{
    [SerializeField] private List<ItemStatus> items = new List<ItemStatus>();
    public List<ItemStatus> Items => items;

    public event Action InventoryChanged;


    // 아이템 인벤토리 아이템 추가
    public void AddItem(BaseItem data)
    {
        ItemStatus status = new ItemStatus(data);

        if (HasItem(data))
        {
            if (items[GetItemIndex(data)].IsFull)
            {
                items.Add(status);
            }
            else
            {
                items[GetItemIndex(data)].StackItem();
            }
        }
        else
        {
            items.Add(status);
        }

        InventoryChanged?.Invoke();
    }

    // 아이템 인벤토리 속 아이템 제거
    public void RemoveItem(BaseItem data)
    {
        if (!HasItem(data)) return;

        items.Remove(items[GetItemIndex(data)]);
        

        InventoryChanged?.Invoke();
    }

    // 아이템 인벤토리 속 아이템 스택 조회
    public int GetStack(BaseItem data)
    {
        if (!HasItem(data)) return 0;

        int count = items[GetItemIndex(data)].Stack;

        return count;
    }

    // 아이템 인벤토리에서 아이템 찾기
    private int GetItemIndex(BaseItem data)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].Data.ID == data.ID)
            {
                return i;
            }
        }

        return -1;
    }

    // 아이템 인벤토리에 아이템 존재 여부
    private bool HasItem(BaseItem data)
    {
        return GetItemIndex(data) != -1;
    }
}