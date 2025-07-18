using System;
using UnityEngine;

public enum E_EquipType
{
    Weapon = 0,
    Hat = 1,
    Accessory = 2,
    Clothes = 3,
    Shoes = 4,
}

[System.Serializable]
public class ItemEquipment
{
    public EquipItemData Weapon;
    public EquipItemData Hat;
    public EquipItemData Accessory;
    public EquipItemData Clothes;
    public EquipItemData Shoes;

    public event Action EquipmentChanged; // 장비 교체 이벤트

    public ItemEquipment()
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
