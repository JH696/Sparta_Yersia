using System;
using System.Collections.Generic;
using UnityEngine;


// 스킬 데이터 + 스킬 상태
[Serializable]
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

// 캐릭터 스킬 상태 저장, 관리 클래스
[Serializable]
public class SkillInventory
{
    [SerializeField] private List<SkillStatus> skills = new List<SkillStatus>();
    public List<SkillStatus> Skills => skills;

    public SkillInventory()
    {
        skills = null;
    }

    // 스킬 인벤토리 스킬 추가
    public void AddSkill(SkillData data)
    {
        if (HasSkill(data)) return;

        SkillStatus status = new SkillStatus(data);
        skills.Add(status);
    }

    // 스킬 인벤토리 속 스킬 제거
    public void RemoveSkill(SkillData data)
    {
        if (!HasSkill(data)) return;

        skills.Remove(skills[GetSkillIndex(data)]);

    }

    // 스킬 인벤토리 속 스킬 레벨업
    public void LevelUpSkill(SkillData data)
    {
        if (HasSkill(data))
        {
            skills[GetSkillIndex(data)].LevelUP();
        }
        else
        {
            Debug.Log("존재하지 않는 스킬 입니다.");
        }
    }

    // 스킬 인벤토리에서 스킬 찾기
    private int GetSkillIndex(SkillData data)
    {
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].Data.ID == data.ID)
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