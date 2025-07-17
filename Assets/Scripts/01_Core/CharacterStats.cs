using System.Collections;
using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    // 레벨 관련
    public int Level;
    public int Exp;
    public int MaxExp;
    // 기본 능력치
    public float MaxHp;
    public float CurrentHp;
    public float MaxMana;
    public float CurrentMana;
    public float Attack;
    public float Defense;
    public float Luck;
    public float Speed;
    // 이벤트
    public event System.Action LevelUP; // 예: 플레이어 스킬 포인트 획득, 펫 스프라이트 변화
    public event System.Action StatusChanged; // UI 연결

    public void SetBaseStats(StatData data)
    {
        if (data == null) return;

        MaxHp = data.MaxHp;
        CurrentHp = MaxHp;

        MaxMana = data.MaxMana;
        CurrentMana = MaxMana;

        Attack = data.Attack;
        Defense = data.Defense;
        Luck = data.Luck;
        Speed = data.Speed;
    }

    public void AddExp(int amount)
    {
        Exp += amount;

        if (Exp >= MaxExp)
        {
            //levelup
        }
    }

    public void LevelUp(float multiplier)
    {
        MaxHp *= multiplier;
        CurrentHp = MaxHp;

        MaxMana *= multiplier;
        CurrentMana = MaxMana;

        Attack *= multiplier;
        Defense *= multiplier;
        Luck *= multiplier;
        Speed *= multiplier;

        LevelUP?.Invoke();
    }

    public void SetCurrentHp(float amount)
    {
        float hp = CurrentHp + amount;
        CurrentHp = Mathf.Clamp(hp, 0, MaxHp);
    }

    public void SetCurrentMana(float amount)
    {
        float mana = CurrentMana + amount;
        CurrentMana = Mathf.Clamp(mana, 0, MaxMana);
    }

    public void IncreaseStat(EStatType statType, float amount)
    {
        switch (statType)
        {
            case EStatType.MaxHp:
                MaxHp += Mathf.Max(amount, 0); break;

            case EStatType.MaxMana:
                MaxMana += Mathf.Max(amount, 0); break;

            case EStatType.Attack:
                Attack += Mathf.Max(amount, 0); break;

            case EStatType.Defense:
                Defense += Mathf.Max(amount, 0); break;

            case EStatType.Luck:
                Luck += Mathf.Max(amount, 0); break;

            case EStatType.Speed:
                Speed += Mathf.Max(amount, 0); break;

            default:
                return;
        }
    }

    public void DecreaseStat(EStatType statType, float amount)
    {
        switch (statType)
        {
            case EStatType.MaxHp:
                MaxHp -= Mathf.Max(amount, MaxHp); break;

            case EStatType.MaxMana:
                MaxMana -= Mathf.Max(amount, MaxMana); break;

            case EStatType.Attack:
                Attack -= Mathf.Max(amount, Attack); break;

            case EStatType.Defense:
                Defense -= Mathf.Max(amount, Defense); break;

            case EStatType.Luck:
                Luck -= Mathf.Max(amount, Luck); break;

            case EStatType.Speed:
                Speed -= Mathf.Max(amount, Speed); break;

            default:
                return;
        }
    }
}