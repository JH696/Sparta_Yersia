using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    [Header("플레이어 스킬 포인트")]
    [SerializeField] private int availableSkillPoints = 3000;

    [Header("플레이어 등급")]
    [SerializeField] private ETier playerTier = ETier.Basic;

    [Header("해금된 스킬 ID")]
    [SerializeField] private List<string> unlockedSkillIDs = new List<string>();

    private Dictionary<string, int> skillLevels = new Dictionary<string, int>();

    public int AvailableSkillPoints => availableSkillPoints;
    public ETier PlayerTier => playerTier;

    private void Awake()
    {
        skillLevels = new Dictionary<string, int>();
    }

    public bool HasSkillUnlocked(string skillID) => unlockedSkillIDs.Contains(skillID);

    public bool CanUnlockSkill(SkillData data)
    {
        if (HasSkillUnlocked(data.SkillID)) return false;
        if (playerTier < data.TierRequirement) return false;
        if (availableSkillPoints < data.BaseUnlockCost) return false;
        return true;
    }

    public void UnlockSkill(SkillData data)
    {
        if (!CanUnlockSkill(data)) return;

        availableSkillPoints -= data.BaseUnlockCost;
        unlockedSkillIDs.Add(data.SkillID);
        skillLevels[data.SkillID] = 1;

        // 마나 증가
        var stats = GetComponent<CharacterStats>();
        stats?.AddMaxMana(data.ManaBonusOnUnlock);

        Debug.Log($"[PlayerSkill] {data.DisplayName} 해금 완료. 남은 포인트: {availableSkillPoints}");
    }

    public bool CanLevelUpSkill(SkillData data)
    {
        if (!HasSkillUnlocked(data.SkillID)) return false;
        if (!skillLevels.ContainsKey(data.SkillID)) return false;

        int currentLevel = skillLevels[data.SkillID];
        if (currentLevel >= data.MaxLevel) return false;

        return availableSkillPoints >= GetLevelUpCost(currentLevel + 1);
    }

    public void LevelUpSkill(SkillData data)
    {
        if (!CanLevelUpSkill(data)) return;

        int currentLevel = skillLevels[data.SkillID];
        int cost = GetLevelUpCost(currentLevel + 1);
        availableSkillPoints -= cost;

        if (currentLevel + 1 <= 10)
        {
            skillLevels[data.SkillID] = currentLevel + 1;
        }
        else
        {
            if (RandomHighLvUpSuccess(currentLevel + 1))
            {
                skillLevels[data.SkillID] = currentLevel + 1;
            }
        }
    }

    private int GetLevelUpCost(int level)
    {
        if (level <= 10) return 1000 + (level - 1) * 500;
        return 1000;
    }

    private bool RandomHighLvUpSuccess(int level)
    {
        float successRate = 0.9f - 0.05f * (level - 11);
        successRate = Mathf.Clamp(successRate, 0.1f, 0.9f);
        return Random.value <= successRate;
    }

    public int GetSkillLevel(string skillID)
    {
        return skillLevels.ContainsKey(skillID) ? skillLevels[skillID] : 0;
    }

    public void AddSkillPoints(int points)
    {
        availableSkillPoints += points;
    }

    public void SetPlayerTier(ETier tier)
    {
        playerTier = tier;
    }
}
