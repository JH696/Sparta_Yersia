using UnityEngine;

[System.Serializable]
public class PetStatus : CharacterStatus
{
    [Header("펫 데이터")]
    public PetData PetData;

    [Header("펫의 진화 단계")]
    public int EvoLevel;

    [Header("실제 펫 인스턴스")]
    public Pet PetInstance;

    /// <summary>
    /// 생성자 (초기 상태와 스탯 지정)
    /// </summary>
    public PetStatus(PetData data)
    {
        PetData = data;
        stat = new CharacterStats(data);
        EvoLevel = 0;

        stat.LevelUP += EvoLevelUp;
        skills = new SkillInventory(data);

        LearnInitialSkills();
    }

    private void LearnInitialSkills()
    {
        if (PetData.StartSkills == null) return;

        foreach (var skillData in PetData.StartSkills)
        {
            if (skills.AddSkill(skillData))
            {
                Debug.Log($"{PetData.PetName}이(가) 초기 스킬 {skillData.Name}을(를) 배웠습니다.");
            }
        }
    }

    public void AddExp(int amount)
    {
        if (stat == null)
        {
            Debug.LogWarning("PetStatus의 stat이 null입니다.");
            return;
        }

        stat.AddExp(amount);
    }

    /// <summary>
    /// 레벨업 시 진화 단계 상승 처리
    /// </summary>
    public void EvoLevelUp()
    {
        // 최대 진화 단계 도달 시 종료
        if (EvoLevel >= PetData.evoLevel.Length - 1) return;

        int nextEvoLevel = EvoLevel + 1; // 다음 진화 단계
        int requiredLevel = PetData.evoLevel[nextEvoLevel];

        // 현재 레벨이 다음 진화 조건을 만족하면 진화
        if (stat.Level >= requiredLevel)
        {
            EvoLevel = nextEvoLevel;
            Debug.Log($"펫이 진화했습니다! 현재 진화 단계: {EvoLevel}");

            var evoSkills = PetData.GetSkillsForEvoLevel(EvoLevel);
            foreach (var skillData in evoSkills)
            {
                if (skills.AddSkill(skillData, true))
                {
                    Debug.Log($"{PetData.PetName}이(가) 진화 {EvoLevel}단계에서 {skillData.Name} 스킬을 배웠습니다.");
                }
            }
        }
    }

    /// <summary>
    /// 현재 진화 단계에 맞는 스프라이트 반환
    /// </summary>
    /// <returns>PetSprite 또는 null</returns>
    public PetSprite GetPetSprite()
    {
        if (EvoLevel < 0 || EvoLevel >= PetData.sprites.Length) return null;

        return PetData.sprites[EvoLevel];
    }

    public Sprite GetCurrentProfileIcon()
    {
        if (PetData == null || PetData.sprites == null) return null;
        if (EvoLevel < 0 || EvoLevel >= PetData.sprites.Length) return null;

        return PetData.sprites[EvoLevel]?.ProfileIcon;
    }

    public void LoadFromSaveData(PetSaveData saveData)
    {
        if (saveData.PetID != PetData.PetID)
        {
            Debug.LogWarning("PetSaveData와 PetData ID 불일치");
            return;
        }

        stat.SetLevel(saveData.Level);
        EvoLevel = saveData.EvoLevel;
    }

    public PetSaveData MakeSaveData()
    {
        return new PetSaveData(this);
    }

    public override BattleVisuals GetBattleVisuals()
    {
        return PetData.BattleVisuals;
    }
}