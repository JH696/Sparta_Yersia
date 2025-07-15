using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // 인벤토리 변경 이벤트
    public event Action OnInventoryChanged;

    // 아이템 추가 실패 이벤트 - 스택초과
    public event Action<string> OnAddFail;

    // <키: ItemData.ID, 값: 아이템 개수>
    private readonly Dictionary<string, int> itemDic = new Dictionary<string, int>();

    // 아이템 추가
    public void AddItem(ItemData data, int count = 1)
    {
        if (data == null) return;

        itemDic.TryGetValue(data.ID, out int currentCount);

        if (currentCount + count > data.MaxStack)
        {
            OnAddFail?.Invoke($"{data.ItemName}은(는) 최대 {data.MaxStack}개까지 보유할 수 있습니다.");
            return;
        }

        itemDic[data.ID] = currentCount + count; // 아이템 개수 증가
        OnInventoryChanged?.Invoke(); // 인벤토리 변경 이벤트 호출
    }

    // 아이템 제거
    public bool RemoveItem(ItemData data, int count = 1)
    {
        if (data == null) return false;
        if (!itemDic.ContainsKey(data.ID)) return false; // 아이템이 없으면 제거 실패

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

    // 전체 아이템 목록 반환 (ID, 갯수만 필요할 때)
    public Dictionary<string, int> GetAllItems()
    {
        return new Dictionary<string, int>(itemDic); // 현재 아이템 목록 복사하여 반환
    }

    /// <summary>
    /// 보유 중인 ItemData 객체 리스트 반환
    /// 사용 예시: GetAllItemData(itemDB)
    /// </summary>
    public List<ItemData> GetAllItemData(ItemData[] data)
    {
        var result = new List<ItemData>();
        foreach (var item in data)
        {
            if (itemDic.ContainsKey(item.ID))
            {
                result.Add(item);
            }
        }
        return result; // 현재 아이템 목록에 있는 ItemData 객체 리스트 반환
    }

    /// <summary>
    /// 보유 중인 특정 카테고리만 SO 리스트로 반환
    /// 사용 예시: GetItemsByCategory(EItemCategory.Consumable, itemDB)
    /// </summary>
    public List<ItemData> GetItemsByCategory(EItemCategory category, List<ItemData> data)
    {
        var result = new List<ItemData>();
        foreach (var item in data)
        {
            if (item.Category == category && itemDic.ContainsKey(item.ID))
                result.Add(item);
        }
        return result;
    }

    /// <summary>
    /// 전체 아이템 리스트에서 아이디로 아이템 찾기
    /// 사용 예시: SearchItemToID(I_c101)
    /// </summary>
    public ItemData SearchItemToID(string ID)
    {
        ItemData item = Resources.Load<ItemData>($"ItemDatas/{ID}");

        return item;
    }
}
