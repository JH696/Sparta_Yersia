// 소비 아이템
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "I_c00", menuName = "ItemData/ConsumeItem")]
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

    public void Consume(CharacterStatus consumer)
    {
        foreach (ItemValue v in Values)
        {
            switch (v.Stat)
            {
                case EStatType.MaxHp:
                    consumer.RecoverHealth(v.Value);
                    break;
                case EStatType.MaxMana:
                    consumer.RecoverMana(v.Value);
                    break;
                default:
                    Debug.LogWarning($"[ConsumeItemData] 지원하지 않는 능력치 입니다.");
                    return;
            }
        }
    }
}