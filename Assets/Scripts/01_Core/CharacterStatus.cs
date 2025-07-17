using Unity.VisualScripting;

public abstract class CharacterStatus
{
    public PetData data;
    public CharacterStats stat;
    public bool IsDead;

    public virtual void Init(PetData data)
    {
        this.data = data;
        InitStat(data.GetComponent<StatData>());
    }

    protected virtual void InitStat(StatData statData)
    {
        if (stat == null)
            stat = new CharacterStats();

        stat.SetBaseStats(statData);
    }

    public virtual void TakeDamage(float amount)
    {
        stat.SetCurrentHp(stat.CurrentHp - amount);

        if (IsDead)
        {
            CharacterDie();
        }
    }

    public virtual void CharacterDie()
    {
        IsDead = true;
    }

    public virtual void Revive()
    {
        stat.CurrentHp = 1;
        IsDead = false;
    }

    // 체력 회복
    public void HealHP(float amount)
    {
        if (IsDead) return;

        stat.SetCurrentHp(stat.CurrentHp + amount);
    }

    // 마나 회복
    public void HealMana(float amount)
    {
        if (IsDead) return;

        stat.SetCurrentMana(stat.CurrentMana + amount);
    }
}