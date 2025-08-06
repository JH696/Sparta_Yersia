using System.Collections.Generic;
using UnityEngine;

// 캐릭터 스킬 상태 저장, 관리 클래스
[System.Serializable]
public class SkillInventory
{
    [Header("장착한 스킬")]
    [SerializeField] private List<SkillStatus> equipSkills = new List<SkillStatus>();

    [Header("습득한 스킬")]
    [SerializeField] private List<SkillStatus> allSkills = new List<SkillStatus>();

    // 읽기 전용
    public List<SkillStatus> EquipSkills => equipSkills;
    public List<SkillStatus> AllSkills => allSkills;

    public event System.Action OnChanged;

    public SkillInventory(ISkillUsable startSkills)
    {
        foreach (SkillData skill in startSkills.StartSkills)
        {
            AddSkill(skill, startSkills is Pet);
        }
    }

    // 스킬 인벤토리 스킬 추가
    public bool AddSkill(SkillData data, bool isPet = false)
    {
        if (HasSkill(data)) return false;

        Debug.Log($"{data.Name} 스킬을 습득했습니다!");

        SkillStatus status = new SkillStatus(data);
        allSkills.Add(status);

        // 펫일 경우 자동 장착
        if (isPet && equipSkills.Count < 5 && !equipSkills.Contains(status))
        {
            equipSkills.Add(status);
            Debug.Log($"{data.Name} 스킬을 자동 장착했습니다!");
        }

        OnChanged?.Invoke(); // 스킬 인벤토리 변경 알림
        return true;
    }

    // 스킬 인벤토리 속 스킬 제거
    public void RemoveSkill(SkillData data)
    {
        if (!HasSkill(data)) return;

        allSkills.Remove(allSkills[GetSkillIndex(data)]);

        Debug.Log($"{data.Name} 스킬을 제거했습니다!");

        OnChanged?.Invoke(); // 스킬 인벤토리 변경 알림
    }

    // 스킬 인벤토리 속 스킬 장착
    public void EquipSkill(SkillStatus status)
    {
        if (AllSkills.Contains(status) && equipSkills.Count < 5)
        {
            if (!equipSkills.Contains(status))
            {
                equipSkills.Add(status);
                Debug.Log($"{status.Data.Name} 스킬을 장착했습니다!");
                OnChanged?.Invoke(); // 스킬 인벤토리 변경 알림
            }
            else
            {
                Debug.Log($"{status.Data.Name} 스킬은 이미 장착되어 있습니다.");
            }
        }
        else
        {
            Debug.Log("보유하지 않았거나, 장착 가능한 최대 스킬 수를 넘겼습니다.");
        }
    }

    // 스킬 인벤토리 속 스킬 해제
    public void UnequipSkill(SkillStatus status)
    {
        if (equipSkills.Contains(status))
        {
            equipSkills.Remove(status);
            OnChanged?.Invoke(); // 스킬 인벤토리 변경 알림
            Debug.Log($"{status.Data.Name} 스킬을 해제 했습니다.");
        }
        else
        {
            Debug.Log("존재하지 않는 스킬 입니다.");
        }
    }

    // 스킬 인벤토리 속 스킬 레벨업
    public void LevelUpSkill(SkillData data)
    {
        if (HasSkill(data))
        {
            allSkills[GetSkillIndex(data)].LevelUP();

            Debug.Log($"{data.Name} 스킬 레벨업! 현재 레벨: {allSkills[GetSkillIndex(data)].Level}");
        }
        else
        {
            Debug.Log("존재하지 않는 스킬 입니다.");
        }

        OnChanged?.Invoke(); // 스킬 인벤토리 변경 알림
    }

    public void ReduceCooldown(int amount)
    {
        foreach (SkillStatus skill in allSkills)
        {
            skill.ReduceCooldown(amount);
        }
    }

    // 스킬 인벤토리에서 스킬 찾기
    private int GetSkillIndex(SkillData data)
    {
        for (int i = 0; i < allSkills.Count; i++)
        {
            if (allSkills[i].Data.ID == data.ID)
            {
                return i;
            }
        }

        return -1;
    }

    // 스킬 인벤토리에 스킬 존재 여부 (예외 처리용)
    public bool HasSkill(SkillData data)
    {
        return GetSkillIndex(data) != -1;
    }

    public SkillStatus GetSkillStatus(SkillData data)
    {
        for (int i = 0; i < allSkills.Count; i++)
        {
            if (allSkills[i].Data.ID == data.ID)
            {
                return allSkills[i];
            }
        }

        return null;
    }
}