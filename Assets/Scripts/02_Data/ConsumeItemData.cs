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

                // 임시: 나머지는 추가 능력치로 적용
                case EStatType.Attack:
                case EStatType.Defense:
                case EStatType.Luck:
                case EStatType.Speed:
                    consumer.stat.IncreaseBonusStat(v.Stat, v.Value);
                    break;

                default:
                    Debug.LogWarning("[ConsumeItemData] 지원하지 않는 능력치입니다.");
                    continue; // return이 아니라 계속 처리

                // 상점 추가로 인한 수정, 확인 후 문제시 위 삭제 후 아래 주석 해제
                //default:
                //    Debug.LogWarning($"[ConsumeItemData] 지원하지 않는 능력치 입니다.");
                //    return;
            }
        }
    }
}