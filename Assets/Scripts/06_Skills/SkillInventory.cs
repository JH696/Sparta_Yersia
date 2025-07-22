using System.Collections.Generic;
using UnityEngine;

// 캐릭터 스킬 상태 저장, 관리 클래스
[System.Serializable]
public class SkillInventory
{
    [Header("습득한 스킬들")]
    [SerializeField] private List<SkillStatus> learnSkills = new List<SkillStatus>();

    [Header("습득 가능한 스킬들")]
    [SerializeField] private List<SkillData> learnableSkills = new List<SkillData>();

    // 읽기 전용
    public List<SkillStatus> LearnSkills => learnSkills;
    public List<SkillData> LearnableSkills => learnableSkills;

    public SkillInventory(ISkillLearnableCharacter startSkills)
    {
        foreach (SkillData skill in startSkills.StartSkills)
        {
            AddSkill(skill);
        }   

        foreach (SkillData skill in startSkills.LearnableSkills)
        {
            learnableSkills.Add(skill);
        }
    }

    // 스킬 인벤토리 스킬 추가
    public void AddSkill(SkillData data)
    {
        if (HasSkill(data)) return;

        SkillStatus status = new SkillStatus(data);
        LearnSkills.Add(status);
    }

    // 스킬 인벤토리 속 스킬 제거
    public void RemoveSkill(SkillData data)
    {
        if (!HasSkill(data)) return;

        LearnSkills.Remove(LearnSkills[GetSkillIndex(data)]);

    }

    // 스킬 인벤토리 속 스킬 레벨업
    public void LevelUpSkill(SkillData data)
    {
        if (HasSkill(data))
        {
            LearnSkills[GetSkillIndex(data)].LevelUP();
        }
        else
        {
            Debug.Log("존재하지 않는 스킬 입니다.");
        }
    }

    public void ReduceCooldown(int amount)
    {
        foreach (SkillStatus skill in LearnSkills)
        {
            skill.ReduceCooldown(amount);
            return;
        }
    }

    // 스킬 인벤토리에서 스킬 찾기
    private int GetSkillIndex(SkillData data)
    {
        for (int i = 0; i < LearnSkills.Count; i++)
        {
            if (LearnSkills[i].Data.ID == data.ID)
            {
                return i;
            }
        }

        return -1;
    }

    // 스킬 인벤토리에 스킬 존재 여부 (예외 처리용)
    private bool HasSkill(SkillData data)
    {
        return GetSkillIndex(data) != -1;
    }
}