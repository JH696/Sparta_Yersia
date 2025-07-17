using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum E_EquipType
{
    Weapon,
    Hat,
    Accessory,
    Clothes,
    Shoes,
}

public struct ItemValue
{
    public EStatType Stat;
    public int Value;
}

// 스킬 데이터 부모 클래스
[System.Serializable]
public abstract class BaseItem : ScriptableObject
{
    [Header("ID / 이름")]
    public string ID;
    public string Name;

    [Header("최대 중첩 수")]
    public int MaxStack = 24;

    [Header("아이콘")]
    public Sprite Icon;

    public abstract E_CategoryType GetCategory();
}

// 장비 아이템
[System.Serializable]
[CreateAssetMenu(fileName = "I_e00", menuName = "Data/장비 아이템")]
public class EquipItemData : BaseItem
{
    [Header("장비 타입")]
    public E_EquipType Type;

    [Header("장착 여부")]
    public bool IsEquipped;

    [Header("장비 성능")]
    public List<ItemValue> Values;

    [Header("가격")]
    public int Price;

    public void Equip(CharacterStats stats)
    {
        if (IsEquipped) return;

        IsEquipped = true;

        if (Values.Count <= 0) return;

        foreach (ItemValue iv in Values)
        {
            stats.IncreaseStat(iv.Stat, iv.Value);
        }
    }   

    public void Unequip(CharacterStats stats)
    {
        if (!IsEquipped) return;

        IsEquipped = false;

        foreach (ItemValue iv in Values)
        {
            stats.DecreaseStat(iv.Stat, iv.Value);
        }
    }


    public override E_CategoryType GetCategory()
    {
        return E_CategoryType.Equip;
    }
}

// 소비 아이템
[System.Serializable]
[CreateAssetMenu(fileName = "I_c00", menuName = "Data/소비 아이템")]
public class ConsumeItemData : BaseItem
{
    [Header("카테고리")]
    [SerializeField] private E_CategoryType Category = E_CategoryType.Consume;

    [Header("소모품 성능")]
    public List<ItemValue> Values;

    [Header("효과 지속 턴")]
    public int Duration;

    [Header("가격")]
    public int Price;

    public void Use()
    {
        // 플레이어 능력치 증가
    }

    public override E_CategoryType GetCategory()
    {
        return E_CategoryType.Consume;
    }
}

// 일반 아이템 (퀘스트 아이템)
[System.Serializable]
[CreateAssetMenu(fileName = "I_q00", menuName = "Data/퀘스트 아이템")]
public class QuestItemData : BaseItem
{
    [Header("설명")]
    public string Description;

    public override E_CategoryType GetCategory()
    {
        return E_CategoryType.Quest;
    }
}
