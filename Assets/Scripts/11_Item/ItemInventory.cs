using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemInventory
{
    [SerializeField] private List<ItemStatus> items;
    [SerializeField] private int maxItemCount;
    private bool isFull => items.Count >= maxItemCount;
    public List<ItemStatus> Items => items;
    public int MaxItemCount => maxItemCount;

    public event Action InventoryChanged;

    // 아이템 인벤토리 아이템 추가
    public ItemInventory()
    {
        items = new List<ItemStatus>();
        maxItemCount = 20; // 기본 최대 아이템 수 설정
    }   

    public void AddItem(BaseItem data)
    {
        if (isFull) { Debug.Log("풀 인벤토리"); return; }

        ItemStatus status = new ItemStatus(this, data);

        if (HasItem(data))
        {
            if (items[GetItemIndex(data)].IsFull)
            {
                items.Add(status);
            }
            else
            {
                items[GetItemIndex(data)].StackItem(1);
            }
        }
        else
        {
            Debug.Log($"아이템 추가: {data.ID}");
            items.Add(status);
        }

        InventoryChanged?.Invoke();
    }

    // 아이템 인벤토리 속 아이템 제거
    public void RemoveItem(BaseItem data)
    {
        if (!HasItem(data)) return;

        items.RemoveAt(GetItemIndex(data));
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