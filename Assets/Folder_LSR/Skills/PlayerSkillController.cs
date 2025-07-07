using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    [Header("초기 스킬 포인트")]
    [SerializeField] private int initialSkillPoints = 0;

    private int availableSkillPoints;
    private Dictionary<string, int> skillLevels = new Dictionary<string, int>();

    private void Awake()
    {
        availableSkillPoints = initialSkillPoints;
    }

    public int AvailableSkillPoints()
    {
        return availableSkillPoints;
    }

    // 스킬이 해금 되었는가
    public bool IsUnlocked(SkillData skillData)
    {
        return skillLevels.ContainsKey(skillData.Id);
    }

    // 현재 레벨
    public int GetSkillLevel(SkillData skillData)
    {
        return IsUnlocked(skillData) ? skillLevels[skillData.Id] : 0;
    }

    // 잠금 해제 가능 여부 (테스트용) - 포인트으로 가능.
    public bool CanUnlock(SkillData skillData)
    {
        return !IsUnlocked(skillData) && availableSkillPoints > 0;
    }

    // 스킬 잠금 해제 (레벨 1로 해금 및 포인트 차감)
    public void UnlockSkill(SkillData skill)
    {
        if (IsUnlocked(skill)) return;

        availableSkillPoints--;
        skillLevels[skill.Id] = 1; // 기본 레벨 1로 해금
    }

    // 스킬 레벨업 (최대 레벨 미만일 떄만)
    public void LevelUpSkill(SkillData skill)
    {
        if (!IsUnlocked(skill)) return;
        int currentLevel = skillLevels[skill.Id];
        //if (currentLevel >= skill.MaxLevel) return;
        skillLevels[skill.Id] = currentLevel + 1;
    }

}
