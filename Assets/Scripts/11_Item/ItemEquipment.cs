using System;
using UnityEngine;

public enum E_EquipType
{
    None = -1,
    Hat = 0,
    Weapon = 1,
    Clothes = 2,
    Accessory = 3,
    Shoes = 4,
}

[System.Serializable]
public class ItemEquipment
{
    [SerializeField] private CharacterStatus Owner; // 소유자

    [SerializeField] private ItemStatus Weapon;
    [SerializeField] private ItemStatus Hat;
    [SerializeField] private ItemStatus Accessory;
    [SerializeField] private ItemStatus Clothes;
    [SerializeField] private ItemStatus Shoes;

    public event Action EquipmentChanged; // 장비 교체 이벤트

    public ItemEquipment(CharacterStatus ower)
    {
        Owner = ower;

        Weapon = null;
        Hat = null;
        Accessory = null;
        Clothes = null;
        Shoes = null;
    }

    /// <summary>
    /// 장비 아이템을 장착하고, 현재 장착 중인 아이템은 장착 해제
    /// </summary>
    public void Equip(ItemStatus status)
    {
        EquipItemData equipData = status.Data as EquipItemData;

        switch (equipData.Type)
        {
            case E_EquipType.Weapon:
                if (Weapon != null)
                {
                    Weapon.IsEquiped = false; // 기존 장비 아이템 상태 변경
                    Weapon = null;
                }
                Weapon = status;
                break;
            case E_EquipType.Hat:
                if (Hat != null)
                {
                    Hat.IsEquiped = false; // 기존 장비 아이템 상태 변경
                    Hat = null;
                }
                Hat = status;
                break;
            case E_EquipType.Accessory:
                if (Accessory != null)
                {
                    Accessory.IsEquiped = false; // 기존 장비 아이템 상태 변경
                    Accessory = null;
                }
                Accessory = status;
                break;
            case E_EquipType.Clothes:
                if (Clothes != null)
                {
                    Clothes.IsEquiped = false; // 기존 장비 아이템 상태 변경
                    Clothes = null;
                }
                Clothes = status;
                break;
            case E_EquipType.Shoes:
                if (Shoes != null)
                {
                    Shoes.IsEquiped = false; // 기존 장비 아이템 상태 변경
                    Shoes = null;
                }
                Shoes = status;
                break;
            default:
                Debug.LogWarning("[CharacterEquipment] 존재하지 않는 유형의 장비입니다.");
                return;
        }

        status.IsEquiped = true; // 장비 아이템 상태 변경
        Debug.Log($"[CharacterEquipment] {Owner}이(가) {equipData.Name}을(를) 장착합니다.");
        EquipmentChanged?.Invoke();
    }

    /// <summary>
    /// 원하는 타입에 장비를 장착 해제하고 반환합니다.
    /// </summary>
    public void Unequip(E_EquipType type)
    {
        switch (type)
        {
            case E_EquipType.Weapon:
                Weapon.IsEquiped = false;
                Weapon = null;
                break;
            case E_EquipType.Hat:
                Hat.IsEquiped = false;
                Hat = null;
                break;
            case E_EquipType.Accessory:
                Accessory.IsEquiped = false;
                Accessory = null;
                break;
            case E_EquipType.Clothes:
                Clothes.IsEquiped = false;
                Clothes = null;
                break;
            case E_EquipType.Shoes:
                Shoes.IsEquiped = false;
                Shoes = null;
                break;
            default:
                Debug.LogWarning("[CharacterEquipment] 존재하지 않는 유형의 장비입니다");
                break;
        }

        EquipmentChanged?.Invoke();
    }

    // 원하는 타입에 아이템 아이콘 반환
    public Sprite GetItemIcon(E_EquipType type)
    {
        ItemStatus target = null;

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
                Debug.Log("[CharacterEquipment] 존재하지 않는 유형의 장비입니다.");
                return null;
        }

        if (target != null && target.Data != null && target.Data.Icon != null)
        {
            return target.Data.Icon; // 아이콘 반환
        }

        return null;
    }
}
