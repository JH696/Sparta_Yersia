using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.CullingGroup;

[System.Serializable]
public class ItemInventory
{
    [Header("저장된 아이템들")]
    [SerializeField] private List<ItemStatus> items = null;

    [Header("인벤토리 최대 용량")]
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

        ItemStatus status = new ItemStatus(data);

        status.OnEmpty += RemoveItem;

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
            items.Add(status);
        }

        InventoryChanged?.Invoke();
    }

    public void RefeshItem(ItemStatus status)
    {
        if (status.Stack <= 0)
        {
            RemoveItem(status.Data);
            return;
        }
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

        int stack = items[GetItemIndex(data)].Stack;

        return stack;
    }

    // 아이템 인벤토리 속 아이템 갯수 조회
    public int GetItemCount(BaseItem data)
    {
        if (!HasItem(data)) return 0;

        int count = 0;

        foreach (ItemStatus item in items)
        {
            if (item.Data.ID == data.ID)
            {
                count += item.Stack;
            }
        }

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

    public void OnDestroy()
    {
        InventoryChanged = null;
    }
}