using UnityEngine;

[System.Serializable]
public abstract class CharacterStatus
{    
    // 사망 여부
    public bool IsDead => stat.CurrentHp <= 0;

    // 캐릭터 스탯
    public CharacterStats stat;
    // 인벤토리, 장비
    public ItemInventory inventory;
    public ItemEquipment equipment;
    // 스킬
    public SkillInventory skills;

    public event System.Action TakeDamaged;
    public event System.Action OnCharacterDead;

    public virtual void TakeDamage(float amount)
    {
        stat.SetCurrentHp(-amount);

        TakeDamaged?.Invoke();

        if (IsDead)
        {
            CharacterDie();
        }
    }

    public virtual void CharacterDie()
    {
        OnCharacterDead?.Invoke();
        Debug.Log($"{this} 사망."); 
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

    public virtual Sprite GetWSprite()
    {
        return null; // 월드 스프라이트 반환, 자식 클래스에서 구현
    }

    public abstract BattleVisuals GetBattleVisuals();
}