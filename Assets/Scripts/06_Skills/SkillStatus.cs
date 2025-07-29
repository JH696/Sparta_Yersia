using UnityEngine;

[System.Serializable]
public class SkillStatus
{
    [Header("스킬 데이터")]
    [SerializeField] private SkillData data;

    [Header("스킬 레벨")]
    [SerializeField] private int level;

    [Header("스킬 쿨타임")]
    [SerializeField] private int curCooldown;

    // 스킬 최대 레벨
    private int maxLevel = 5;

    // 읽기 전용
    public SkillData Data => data;
    public float Power => data.Power * 1 + (level - 1 * 0.1f); // 레벨마다 10%씩 피해량 증가
    public int Level => level;
    public int Cooldown => curCooldown;
    public bool IsCool => curCooldown > 0;

    public SkillStatus(SkillData data)
    {
        this.data = data;
        level = 1;
        curCooldown = 0;
    }

    // 시전 가능 여부
    public bool CanCast(CharacterStatus caster)
    {
        if (IsCool || caster.stat.CurrentMana < data.Cost) return false;
        return true;
    }

    // 시전
    public void Cast(CharacterStatus caster)
    {
        if (!CanCast(caster)) return;

        caster.stat.SetCurrentMana(-data.Cost);
        curCooldown = data.Cooldown;
    }

    // 레벨업
    public bool LevelUP()
    {
        if (level >= maxLevel) return false; 

        level++;
        return true;
    }

    public bool LevelDown()
    {
        if (level <= 1) return false;

        level--;
        return true;
    }

    // 쿨다운 감소
    public void ReduceCooldown(int amount)
    {
        curCooldown = Mathf.Max(0, curCooldown - amount);
    }
}
