using System;
using System.Collections.Generic;
using UnityEngine;

// 아이템 데이터 + 아이템 상태
public class ItemStatus
{
    public BaseItem Data { get; private set; }
    public int Stack { get; private set; }
    public bool IsFull => Stack == Data.MaxStack;

    public event Action StatusChanged; // 아이템 상태 변경 이벤트
    public event Action OnEmpty; // 아이템이 비었을 때 호출
    public ItemStatus(BaseItem data)
    {
        Data = data;
        Stack = 1;
    }

    public void UseItem()
    {
        Stack--;

        if (Stack == 0)
        {
            OnEmpty?.Invoke();
        }

        StatusChanged?.Invoke();
    }


    public void StackItem()
    {
        if (IsFull) return;

        Stack++;
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

public class CharacterEquipment
{
    public EquipItemData Weapon;
    public EquipItemData Hat;
    public EquipItemData Accessory;
    public EquipItemData Clothes;
    public EquipItemData Shoes;

    public event Action EquipmentChanged; // 장비 교체 이벤트

    public CharacterEquipment()
    {
        Weapon = null;
        Hat = null;
        Accessory = null;
        Clothes = null;
        Shoes = null;
    }


    /// <summary>
    /// 장비 아이템을 장착하고 기존 장착한 아이템을 반환합니다.
    /// </summary>
    public EquipItemData Equip(EquipItemData item)
    {
        // 기존 장비 해제
        EquipItemData previous = null;

        switch (item.Type)
        {
            case E_EquipType.Weapon:
                previous = Weapon;
                Weapon = item;
                break;
            case E_EquipType.Hat:
                previous = Hat;
                Hat = item;
                break;
            case E_EquipType.Accessory:
                previous = Accessory;
                Accessory = item;
                break;
            case E_EquipType.Clothes:
                previous = Clothes;
                Clothes = item;
                break;
            case E_EquipType.Shoes:
                previous = Shoes;
                Shoes = item;
                break;
            default:
                Debug.LogWarning("[CharacterEquipment] 존재하지 않는 유형의 장비입니다: " + item.Type);
                return null;
        }

        if (previous != null)
        {
            previous.IsEquipped = false;
        }

        item.IsEquipped = true;
        EquipmentChanged?.Invoke();
        return previous;
    }

    /// <summary>
    /// 원하는 타입에 장비를 장착 해제하고 반환합니다.
    /// </summary>
    public EquipItemData Unequip(E_EquipType type)
    {
        EquipItemData target;

        switch (type)
        {
            case E_EquipType.Weapon:
                target = Weapon;
                break;
            case E_EquipType.Hat:
                target = Hat;
                break;
            case E_EquipType.Accessory:
                target = Accessory;
                break;
            case E_EquipType.Clothes:
                target = Clothes;
                break;
            case E_EquipType.Shoes:
                target = Shoes;
                break;
            default:
                Debug.LogWarning("[CharacterEquipment] 존재하지 않는 유형의 장비입니다: " + type);
                return null;
        }

        target.IsEquipped = false;
        target = null;

        return target;
    }
}

public class ItemInventory
{
    [SerializeField] private List<ItemStatus> items = new List<ItemStatus>();

    [SerializeField] private List<EquipItemData> equipedItems = new List<EquipItemData>();

    public List<ItemStatus> Items => items;
    public List<EquipItemData> EquipedItems => equipedItems;

    public event Action InventoryChanged;

    // 아이템 인벤토리 아이템 추가
    public void AddItem(BaseItem data)
    {
        ItemStatus status = new ItemStatus(data);

        status.OnEmpty += () => RemoveItem(status.Data);

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