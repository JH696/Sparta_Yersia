using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 플레이어 스킬 전반 (해금, 레벨업, 장착 여부 등)
public class PlayerSkillController : MonoBehaviour
{
    private CharacterSkill characterSkill;
    private PlayerData playerData;
    private int skillPoints;

    private readonly HashSet<string> equippedSkillIds = new HashSet<string>();

    public void Init(CharacterSkill skill, PlayerData data, int startingPoints)
    {
        characterSkill = skill;
        playerData = data;
        skillPoints = startingPoints;

        // 스킬 리스트 초기화
        characterSkill.Init(playerData.startingSkills.Cast<SkillBase>());

        // 기본 스킬 자동 해금
        foreach (var basicSkillStatus
            in characterSkill.AllStatuses.
            Where(status => status.Data.SkillTier == ETier.Basic))
        {
            basicSkillStatus.Unlock();
        }
    }

    // 스킬 해금
    public void UnlockSkill(string skillId)
    {
        var skillStatus = characterSkill.AllStatuses.FirstOrDefault(status => status.Data.Id == skillId);
        skillStatus?.Unlock();
    }

    // 스킬 레벨업
    public bool LevelUpSkill(string skillId)
    {
        var skillStatus = characterSkill.AllStatuses.FirstOrDefault(status => status.Data.Id == skillId);
        if (skillStatus == null) return false;
        return skillStatus != null && skillStatus.LevelUp(ref skillPoints);
    }

    // 스킬 장착 여부 확인
    public bool IsEquipped(string skillId)
    {
        return equippedSkillIds.Contains(skillId);
    }

    // 스킬 장착
    public void EquipSkill(string skillId)
    {
        if (!equippedSkillIds.Contains(skillId))
        {
            equippedSkillIds.Add(skillId);
        }
    }

    // 스킬 해제
    public void UnEquipSkill(string skillId)
    {
        equippedSkillIds.Remove(skillId);
    }

    // 전투용 장착 스킬 사용 시도
    public bool TryUseEquippedSkill(string skillId)
    {
        if (!IsEquipped(skillId)) return false;
        return characterSkill.TryUseSkill(skillId);
    }
}
