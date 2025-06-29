using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // 인벤토리 변경 이벤트
    public event Action OnInventoryChanged;

    // <키: ItemData.ID, 값: 아이템 개수>
    private readonly Dictionary<string, int> itemDic = new Dictionary<string, int>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // 아이템 추가
    public void AddItem(ItemData data, int count = 1)
    {
        if (data == null) return;

        if (itemDic.ContainsKey(data.ID))
        {
            itemDic[data.ID] += count; // 이미 있는 아이템이면 개수 증가
        }
        else
        {
            itemDic[data.ID] = count; // 새로운 아이템이면 추가
        }
        OnInventoryChanged?.Invoke(); // 인벤토리 변경 이벤트 호출
    }

    // 아이템 제거
    public bool RemoveItem(ItemData data, int count = 1)
    {
        if (data == null) return false;

        itemDic[data.ID] -= count; // 아이템 개수 감소

        if (itemDic[data.ID] <= 0)
        {
            itemDic.Remove(data.ID); // 개수가 0 이하가 되면 아이템 제거
        }

        OnInventoryChanged?.Invoke(); // 인벤토리 변경 이벤트 호출
        return true;
    }

    // 특정 아이템 개수 조회
    public int GetCount(string itemID)
    {
        if (itemDic.TryGetValue(itemID, out int count))
        {
            return count; // 아이템이 있으면 개수 반환
        }

        return 0; // 아이템이 없으면 0 반환
    }

    // 오버로딩: ItemData로 개수 조회
    public int GetCount(ItemData data)
    {
        if (data == null) return 0;
        return GetCount(data.ID); // 아이템 ID로 개수 조회
    }

    // 전체 아이템 목록 반환
    public Dictionary<string, int> GetAllItems()
    {
        return new Dictionary<string, int>(itemDic); // 현재 아이템 목록 복사하여 반환
    }
}
