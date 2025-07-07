using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    protected CharacterStats Stat = new CharacterStats();

    public float MaxHp => Stat.MaxHp;
    public float CurrentHp => Stat.CurrentHp;
    public float MaxMana => Stat.MaxMana;
    public float CurrentMana => Stat.CurrentMana;
    public float Attack => Stat.Attack;
    public float Defense => Stat.Defense;
    public float Luck => Stat.Luck;
    public float Speed => Stat.Speed;

    public bool IsDead => CurrentHp <= 0f;


    // 외부 데이터로부터 스탯을 초기화
    public virtual void InitStat(ICharacterStatData statData)
    {
        if (statData == null) return;
        Stat.SetBaseStats(statData);
    }

    public virtual float GainDamage(float power)
    {
        float roll = UnityEngine.Random.Range(0f, 100f); // 치명타 계산
        bool isCritical = roll <= Luck; // luck과 같거나 그보다 작으면


        float attack = Attack; // 공격력
        float damage = Attack * (1 + power); // 공격력 * 스킬 배율
        float finalDamage = Mathf.Max(1, isCritical ? damage * 1.5f : damage); // 치명타 피해 적용

        Debug.Log($"최종 피해량: {finalDamage}");
        return finalDamage;
    }


    // 데미지 입음
    public virtual void TakeDamage(float amount)
    {
        float finalDamage = Mathf.Max(1f, amount - Defense);
        Stat.SetCurrentHp(CurrentHp - finalDamage);

        Debug.Log($"받은 피해: {amount} 남은 체력: {CurrentHp}/{MaxHp}");
    }

    // HP 회복
    public virtual void HealHP(float amount)
    {
        Stat.SetCurrentHp(CurrentHp + amount);
    }

    // Mana 회복
    public virtual void HealMana(float amount)
    {
        Stat.SetCurrentMana(CurrentMana + amount);
    }

    // HP 변경
    public virtual void SetCurrentHp(float value)
    {
        Stat.CurrentHp = Mathf.Clamp(value, 0f, MaxHp);
        if (IsDead)
        {
            IsDie();
        }
    }

    // MP 변경
    public virtual void SetCurrentMana(float value)
    {
        Stat.CurrentMana = Mathf.Clamp(value, 0f, MaxMana);
    }

    // 사망 처리 (필요 시 자식 클래스에서 오버라이드)
    protected virtual void IsDie()
    {
        Debug.Log($"{gameObject.name} 사망 처리");
    }
}