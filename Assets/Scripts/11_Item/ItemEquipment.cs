using System;
using System.Collections.Generic;
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
    [SerializeField] private CharacterStatus owner; // 소유자

    [Header("현재 장착된 아이템")]
    [SerializeField] private EquipItemData Weapon = null;
    [SerializeField] private EquipItemData Hat = null;
    [SerializeField] private EquipItemData Accessory = null;
    [SerializeField] private EquipItemData Clothes = null;
    [SerializeField] private EquipItemData Shoes = null;

    public event Action EquipmentChanged; // 장비 교체 이벤트

    public ItemEquipment(CharacterStatus ower)
    {
        this.owner = ower;

        EquipmentChanged += UpdateBonusStats;
    }

    /// <summary>
    /// 장비 아이템을 장착하고, 현재 장착 중인 아이템은 장착 해제
    /// </summary>
    public void Equip(EquipItemData equipData)
    {
        switch (equipData.Type)
        {
            case E_EquipType.Weapon: Weapon = equipData; break;
            case E_EquipType.Hat: Hat = equipData; break;
            case E_EquipType.Accessory: Accessory = equipData; break;
            case E_EquipType.Clothes: Clothes = equipData; break;
            case E_EquipType.Shoes: Shoes = equipData; break;
            default:
                Debug.LogWarning("[CharacterEquipment] 존재하지 않는 유형의 장비입니다.");
                return;
        }

        Debug.Log($"[CharacterEquipment] {owner}이(가) {equipData.Name}을(를) 장착합니다.");
        EquipmentChanged?.Invoke();
    }

    /// <summary>
    /// 원하는 타입에 장비를 장착 해제하고 반환합니다.
    /// </summary>
    public void Unequip(E_EquipType type)
    {
        switch (type)
        {
            case E_EquipType.Weapon: Weapon = null; break;
            case E_EquipType.Hat: Hat = null; break;
            case E_EquipType.Accessory: Accessory = null; break;
            case E_EquipType.Clothes: Clothes = null; break;
            case E_EquipType.Shoes: Shoes = null; break;
            default:
                Debug.LogWarning("[CharacterEquipment] 존재하지 않는 유형의 장비입니다");
                break;
        }

        EquipmentChanged?.Invoke();
    }

    // 이미 장착된 아이템인지 확인하는 메서드
    public EquipItemData FindEquippedItem(EquipItemData equipItem)
    {
        foreach (var item in GetAllEquippedItems())
        {
            if (item != null && item.ID == equipItem.ID)
                return item;
        }
        return null;
    }

    // 원하는 타입에 아이템 아이콘 반환
    public Sprite GetItemIcon(E_EquipType type)
    {
        EquipItemData target = null;

        switch (type)
        {
            case E_EquipType.Weapon: target = Weapon; break;
            case E_EquipType.Hat: target = Hat; break;
            case E_EquipType.Accessory: target = Accessory; break;
            case E_EquipType.Clothes: target = Clothes; break;
            case E_EquipType.Shoes: target = Shoes; break;
            default:
                Debug.Log("[CharacterEquipment] 존재하지 않는 유형의 장비입니다.");
                return null;
        }

        if (target != null)
        {
            return target.Icon; // 아이콘 반환
        }

        return null;
    }

    // 이벤트로 호출되는 추가 능력치 업데이트 메서드
    private void UpdateBonusStats()
    {
        CharacterStats stats = owner.stat;

        // 기존 추가 능력치 초기화 (장비 해제일 경우 고려)
        stats.ResetBonusStat();

        // 아이템별 추가 능력치 적용
        foreach (var equip in GetAllEquippedItems())
        {
            if (equip == null) continue;

            foreach (var value in equip.Values)
            {
                stats.IncreaseBonusStat(value.Stat, value.Value);
            }
        }
    }

    // 내부용 장착된 아이템 목록 반환 메서드
    private IEnumerable<EquipItemData> GetAllEquippedItems()
    {
        yield return Weapon;
        yield return Hat;
        yield return Accessory;
        yield return Clothes;
        yield return Shoes;
    }
}
