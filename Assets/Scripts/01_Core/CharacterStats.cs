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

    public void SetBaseStats(CharacterData data)
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


    // 스탯을 증가시키는 메소드 - 스킬에 사용 (선량 추가- 만약 이미 추가하신것있으면 지워도 됨. 임시로 추가한거임)
    public void AddMaxMana(float amount)
    {
        MaxMana += amount;
        CurrentMana = Mathf.Clamp(CurrentMana, 0, MaxMana);
    }
}