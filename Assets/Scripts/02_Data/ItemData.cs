using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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
    [Header("장비 분류")]
    public E_EquipType Type;

    [Header("장비 성능")]
    public List<ItemValue> Values;

    [Header("가격")]
    public int Price;

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
    [Header("소모품 성능")]
    public List<ItemValue> Values;

    [Header("효과 지속 턴")]
    public int Duration;

    [Header("가격")]
    public int Price;

    public override E_CategoryType GetCategory()
    {
        return E_CategoryType.Consume;
    }

    public void Consume(CharacterStatus status)
    { 
        foreach (ItemValue v in Values)
        {
            switch (v.Stat)
            {
                case EStatType.MaxHp:
                    status.RecoverHealth(v.Value);
                    break;
                case EStatType.MaxMana:
                    status.RecoverMana(v.Value);
                    break;
                default:
                    Debug.LogWarning($"[ConsumeItemData] 지원하지 않는 능력치 입니다.");
                    return;
            }
        }
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
