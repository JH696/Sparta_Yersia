using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PetData", menuName = "Data/PetData")]
public class PetData : ScriptableObject, ICharacterStatData
{
    [Header("펫 ID 및 이름")]
    public string PetID;
    public string PetName;

    [Header("펫 스탯")]
    [Tooltip("펫의 기본 스탯 데이터")]
    public CharacterStatData StatData;

    [Header("진화 관련")]
    [Tooltip("진화 단계별 스프라이트 (ex. 0 = 기본, 1 = 1차 진화, 2 = 2차 진화)")]
    public PetSprite[] sprites = new PetSprite[3];

    [Tooltip("각 진화 단계에 도달하기 위한 레벨")]
    public EvoLevel[] evoLevels = new EvoLevel[2];

    [Header("레벨 및 경험치")]
    [Tooltip("시작 레벨")]
    public int StartLevel = 1;

    [Tooltip("시작 경험치")]
    public int StartExp = 0;

    [Tooltip("레벨업 기준 경험치 (기본값 * 현재 레벨)")]
    public int BaseExpToLevelUp = 50;

    [Tooltip("레벨업 시 스탯 배율 증가")]
    public float StatMultiplierPerLevel = 1.1f;

    [Header("진화 시 적용할 스탯 배율")]
    public float StatMultiplier = 1.5f;

    [Header("현재 진화 단계")]
    [Tooltip("현재 펫의 진화 단계 (0 = 기본, 1 = 1차 진화, 2 = 2차 진화)")]
    [Range(0, 2)]
    public int CurrentEvoStage = 0;

    // 현재 진화 단계에 맞는 아이콘 반환
    public Sprite GetCurrentProfileIcon()
    {
        if (sprites == null || sprites.Length <= CurrentEvoStage) return null;
        return sprites[CurrentEvoStage].Icon;
    }

    // 현재 진화 단계에 맞는 월드 스프라이트 반환
    public Sprite GetCurrentWorldSprite()
    {
        if (sprites == null || sprites.Length <= CurrentEvoStage) return null;
        return sprites[CurrentEvoStage].WorldSprite;
    }

    // ICharacterStatData 구현

    public float MaxHp => StatData == null ? 0f : StatData.MaxHp;
    public float MaxMana => StatData == null ? 0f : StatData.MaxMana;
    public float Attack => StatData == null ? 0f : StatData.Attack;
    public float Defense => StatData == null ? 0f : StatData.Defense;
    public float Luck => StatData == null ? 0f : StatData.Luck;
    public float Speed => StatData == null ? 0f : StatData.Speed;
}

[Serializable]
public class PetSprite
{
    public Sprite WorldSprite;
    public Sprite Icon;
}

[Serializable]
public class EvoLevel
{
    public int Level;
}