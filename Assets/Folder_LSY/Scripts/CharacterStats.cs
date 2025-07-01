using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    public float MaxHp;
    public float CurrentHp;
    public float MaxMana;
    public float CurrentMana;
    public float Attack;
    public float Defense;
    public float Luck;
    public float Speed;

    public void SetBaseStats(ICharacterStatData data)
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

    // 스탯을 배율로 증가 (레벨업, 진화)

    public void MultiplyStats(float multiplier)
    {
        MaxHp *= multiplier;
        CurrentHp = MaxHp;

        MaxMana *= multiplier;
        CurrentMana = MaxMana;

        Attack *= multiplier;
        Defense *= multiplier;
        Luck *= multiplier;
        Speed *= multiplier;
    }

    // 현재 HP/Mana가 Max를 넘지 않도록 조정

    public void SetCurrentHp(float hp)
    {
        CurrentHp = Mathf.Clamp(hp, 0, MaxHp);
    }

    public void SetCurrentMana(float mana)
    {
        CurrentMana = Mathf.Clamp(mana, 0, MaxMana);
    }
}