using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    [SerializeField] protected CharacterStats Stat = new CharacterStats();

    public float MaxHp => Stat.MaxHp;
    public float CurrentHp => Stat.CurrentHp;
    public float MaxMp => Stat.MaxMp;
    public float CurrentMp => Stat.CurrentMp;
    public float Attack => Stat.Attack;
    public float Defense => Stat.Defense;
    public float Luck => Stat.Luck;
    public float Speed => Stat.Speed;

    public virtual void TakeDamage(float amount)
    {
        float finalDamage = Mathf.Max(1f, amount - Defense);
        Stat.SetCurrentHp(Stat.CurrentHp - finalDamage);
    }

    public virtual void Heal(float amount)
    {
        Stat.SetCurrentHp(Stat.CurrentHp + amount);
    }
}
