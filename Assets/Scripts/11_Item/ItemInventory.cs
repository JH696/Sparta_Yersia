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
    public int GetItemIndex(BaseItem data)
    {
        if (data == null) return -1;

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

    /// <summary>
    /// 이 아이템을 1개 더넣을수있는지 확인(같은ID 스택여유 or 빈슬롯)
    /// </summary>
    public bool CanAddOne(BaseItem data)
    {
        if (data == null) return false;

        // 같은 ID 스택 중에 여유가 있으면 OK
        foreach (var it in items)
        {
            if (it.Data.ID == data.ID && it.Stack < it.Data.MaxStack)
                return true;
        }

        // 여유 슬롯이 있으면 OK
        return items.Count < maxItemCount;
    }

    /// <summary>
    /// 상점 구매 전용
    /// 성공시 true, 실패시 false(슬롯/스택 여유 모두 없을때)
    /// </summary>
    public bool TryAddOne(BaseItem data)
    {
        if (data == null) return false;

        // 스택 여유 먼저 채워 넣기
        for (int i = 0; i < items.Count; i++)
        {
            var s = items[i];
            if (s.Data.ID == data.ID && s.Stack < s.Data.MaxStack)
            {
                s.StackItem(1);
                InventoryChanged?.Invoke();
                return true;
            }
        }

        // 새 슬롯 생성 가능한지
        if (items.Count >= maxItemCount) return false;

        var status = new ItemStatus(data);
        status.OnEmpty += RemoveItem;
        items.Add(status);
        InventoryChanged?.Invoke();
        return true;
    }
}