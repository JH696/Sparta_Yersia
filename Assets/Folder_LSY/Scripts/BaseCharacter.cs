using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    [SerializeField] protected CharacterStats Stat = new CharacterStats();

    public float MaxHp => Stat.MaxHp;
    public float CurrentHp => Stat.CurrentHp;
    public float MaxMana => Stat.MaxMana;
    public float CurrentMana => Stat.CurrentMana;
    public float Attack => Stat.Attack;
    public float Defense => Stat.Defense;
    public float Luck => Stat.Luck;
    public float Speed => Stat.Speed;

    public virtual void InitStat(CharacterStatData statData)
    {
        if (statData == null) return;
        Stat.InitFromData(statData);
    }

    public virtual void TakeDamage(float amount)
    {
        float finalDamage = Mathf.Max(1f, amount - Defense);
        Stat.SetCurrentHp(CurrentHp - finalDamage);
    }

    public virtual void Heal(float amount)
    {
        Stat.SetCurrentHp(CurrentHp + amount);
    }

    public virtual Sprite ProfileIcon => null;
}
