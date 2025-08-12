using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PetSaveData
{
    public string PetID; // 펫 고유 ID
    public int Level;    // 펫 레벨
    public int EvoLevel; // 진화 단계

    public PetSaveData() { }

    public PetSaveData(PetStatus pet)
    {
        PetID = pet.PetData.PetID;
        Level = pet.stat.Level;
        EvoLevel = pet.EvoLevel;
    }
}

[CreateAssetMenu(fileName = "PetData", menuName = "Data/PetData")]
public class PetData : StatData, ISkillUsable
{
    [Header("펫 ID 및 이름")]
    public string PetID;
    public string PetName;

    [Header("프리팹")]
    public GameObject PetPrefab;

    [Header("진화 관련")]
    [Tooltip("진화 단계별 스프라이트 (0 = 기본, 1 = 1차 진화, 2 = 2차 진화)")]
    public PetSprite[] sprites = new PetSprite[3];

    [Tooltip("진화 단계별 프로필 아이콘")]
    public Sprite GetCurrentProfileIcon(int evoLevelIndex)
    {
        if (sprites == null || evoLevelIndex < 0 || evoLevelIndex >= sprites.Length)
            return null;

        return sprites[evoLevelIndex].ProfileIcon ?? sprites[0].ProfileIcon;
    }

    [Header("진화 레벨 요구치")]
    public int[] evoLevel = new int[3] { 1, 5, 10 };

    [Header("초기 스킬 리스트")]
    [SerializeField] private List<SkillData> startSkills = new();
    public List<SkillData> StartSkills => startSkills;

    [Header("진화 단계별 배우는 스킬")]
    [SerializeField] private List<EvolveSkillEntry> evolveSkills = new();

    public List<SkillData> GetSkillsForEvoLevel(int evoLevel)
    {
        List<SkillData> result = new();
        foreach (var entry in evolveSkills)
        {
            if (entry.EvoLevel == evoLevel)
            {
                result.Add(entry.Skill);
            }
        }
        return result;
    }

    [Header("배틀씬")]
    public BattleVisuals BattleVisuals;
}

[Serializable]
public class PetSprite
{
    public Sprite WorldSprite;
    public Sprite ProfileIcon;
}

[Serializable]
public class EvolveSkillEntry
{
    [Tooltip("몇 단계에서 배우는지 (0=기본, 1=1차 진화, 2=2차 진화)")]
    public int EvoLevel;

    [Tooltip("배울 스킬")]
    public SkillData Skill;
}