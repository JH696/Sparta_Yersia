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
    public float Power => data.Power + ((level - 1) * 0.1f); // 레벨마다 10%씩 피해량 증가
    public int Level => level;
    public int Cooldown => curCooldown;
    public bool CanUse => curCooldown == 0;

    public SkillStatus(SkillData data)
    {
        this.data = data;
        level = 1;
        curCooldown = 0;
    }

    // 시전
    public void Spell(CharacterStatus caster)
    {
        if (!CanUse) return;

        curCooldown = data.Cooldown;
    }

    // 레벨업
    public void LevelUP()
    {
        if (level >= maxLevel) return;

        level++;
    }

    // 쿨다운 감소
    public void ReduceCooldown(int amount)
    {
        curCooldown = Mathf.Max(0, curCooldown - amount);
    }
}
