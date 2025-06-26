using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    [Header("캐릭터 기본 스탯 데이터")]
    [SerializeField] private CharacterStatData statData;

    [SerializeField] protected CharacterStats Stat = new CharacterStats();

    public float MaxHp => Stat.MaxHp;
    public float CurrentHp => Stat.CurrentHp;
    public float MaxMana => Stat.MaxMana;
    public float CurrentMana => Stat.CurrentMana;
    public float Attack => Stat.Attack;
    public float Defense => Stat.Defense;
    public float Luck => Stat.Luck;
    public float Speed => Stat.Speed;

    protected virtual void Awake()
    {
        if (statData == null) return;
        Stat.InitFromData(statData);
    }

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
