using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 스킬 해금 및 레벨 상태를 관리
/// </summary>
public class PlayerSkillController : MonoBehaviour
{
    [Header("스킬 포인트")]
    [SerializeField] private int availableSkillPoints = 0;

    [Header("플레이어 등급(초/중/상)")]
    [SerializeField] private ETier playerTier = ETier.Basic;

    [Header("해금된 스킬 ID")]
    [SerializeField] private List<string> unlockedSkillIDs = new List<string>();

    [Header("스킬 레벨")]
    [SerializeField] private Dictionary<string, int> skillLevels = new Dictionary<string, int>();

    public int AvailableSkillPoints => availableSkillPoints;
    public ETier PlayerTier => playerTier;

    private void Awake()
    {
        skillLevels = new Dictionary<string, int>();
    }

    // 스킬 레벨업시 필요한 포인트
    private bool HasSkillUnlocked(string skillID)
    {
        return unlockedSkillIDs.Contains(skillID);
    }

    // 스킬 락 해제 가능여부
    private bool CanUnlockSkill(SkillData data)
    {
        if (HasSkillUnlocked(data.SkillID)) return false;
        if (playerTier < data.TierRequirement) return false;
        if (availableSkillPoints < data.BaseUnlockCost) return false;

        return true;
    }

    // 스킬 락 해제
    private void UnlockSkill(SkillData data)
    {
        if (!CanUnlockSkill(data)) return;

        availableSkillPoints -= data.BaseUnlockCost;
        unlockedSkillIDs.Add(data.SkillID);
        skillLevels[data.SkillID] = 1; // 기본 레벨 1로 설정

        // 플레이어에게 마나 보너스 적용 - 상의 필요하나 테스트용으로 구현
        var stats = GetComponent<CharacterStats>();
        if (stats != null)
        {
            stats.AddMaxMana(data.ManaBonusOnUnlock);
        }

        Debug.Log($"[PlayerSkill]스킬 '{data.DisplayName}' 해금됨! 현재 스킬 포인트: {availableSkillPoints}");
    }

    // 스킬 레벨업 가능여부
    private bool CanLevelUpSkill(SkillData data)
    {
        if (!HasSkillUnlocked(data.SkillID)) return false;
        if (!skillLevels.ContainsKey(data.SkillID)) return false;

        int currentLevel = skillLevels[data.SkillID];
        if (currentLevel >= data.MaxLevel) return false;

        return availableSkillPoints >= GetLevelUpCost(currentLevel + 1);
    }

    // 스킬 레벨업 비용 계산
    private int GetLevelUpCost(int level)
    {
        if (level <= 10) return 1000 + (level - 1) * 500; // 예시: 레벨 1은 1000, 레벨 2는 1500, 레벨 3은 2000 등
        return 1000;
    }

    // 스킬 레벨업
    private void LevelUpSkill(SkillData data)
    {
        if (!CanLevelUpSkill(data)) return;

        int currentLevel = skillLevels[data.SkillID];
        int cost = GetLevelUpCost(currentLevel + 1);

        availableSkillPoints -= cost;

        if (currentLevel + 1 <= 10)
        {
            skillLevels[data.SkillID] = currentLevel + 1;
            Debug.Log($"[PlayerSkill] '{data.DisplayName}' 레벨업성공! {skillLevels[data.SkillID]}, point: {availableSkillPoints}");
        }
        else
        {
            bool success = RandomHighLvUpSuccess(currentLevel + 1);
            if (success)
            {
                skillLevels[data.SkillID] = currentLevel + 1;
                Debug.Log($"[PlayerSkill] '{currentLevel + 1}로 레벨업 성공");
            }
            else
            {
                Debug.Log($"[PlayerSkill] '{currentLevel + 1}로 레벨업 실패");
            }
        }
    }

    // 특정레벨 이상 레벨업 성공 확률 계산
    private bool RandomHighLvUpSuccess(int level)
    {
        // 예시: 레벨 11 이상은 50% 확률로 성공
        if (level > 10)
        {
            return Random.Range(0f, 1f) < 0.5f;
        }
        return true; // 레벨 10 이하의 경우 항상 성공
    }

    public void AddSkillPoints(int points)
    {
        availableSkillPoints += points;
    }

    public void SetPlayerTier(ETier tier)
    {
        playerTier = tier;
    }

    public int GetSkillLevel(string skillID)
    {
        return skillLevels.ContainsKey(skillID) ? skillLevels[skillID] : 0;
    }
}
