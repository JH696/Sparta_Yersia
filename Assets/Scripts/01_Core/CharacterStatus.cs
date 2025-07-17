public abstract class CharacterStatus
{
    // 캐릭터 스탯
    public CharacterStats stat;
    // 사망 여부
    public bool IsDead;

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