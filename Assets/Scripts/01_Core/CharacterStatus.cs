public abstract class CharacterStatus
{    
    // 사망 여부
    public bool IsDead;
    // 캐릭터 스탯
    public CharacterStats stat;

    public virtual void TakeDamage(float amount)
    {
        stat.SetCurrentHp(-amount);

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
    public void RecoverHealth(float amount)
    {
        if (IsDead) return;

        stat.SetCurrentHp(amount);
    }

    // 마나 회복
    public void RecoverMana(float amount)
    {
        if (IsDead) return;

        stat.SetCurrentMana(amount);
    }
}