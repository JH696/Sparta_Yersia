using UnityEngine;

[System.Serializable]
public class SkillStatus
{
    public SkillData Data { get; private set; }
    public int Level { get; private set; }
    public int CurCooldown { get; private set; }
    public bool CanUse => CurCooldown == 0;    // 사용 가능 여부

    public SkillStatus(SkillData data)
    {
        this.Data = data;
        Level = 1;
        CurCooldown = 0;
    }

    // 시전
    public void Spell(CharacterStats caster, CharacterStats target)
    {
        // 데미지 계산 
        // 마나 감소
        CurCooldown = Data.Cooldown;
    }

    // 레벨업
    public void LevelUP()
    {
        Debug.Log($"{Data.Name} 레벨업");
        Level++;
    }

    // 쿨다운 감소
    public void ReduceCooldown(int amount)
    {
        CurCooldown -= amount;
    }
}
