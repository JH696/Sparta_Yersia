using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PetSavaData
{

}

[CreateAssetMenu(fileName = "PetData", menuName = "Data/PetData")]
public class PetData : StatData, ISkillLearnableCharacter
{
    [Header("펫 ID 및 이름")]
    public string PetID;
    public string PetName;

    [Header("진화 관련")]
    [Tooltip("진화 단계별 스프라이트 (ex. 0 = 기본, 1 = 1차 진화, 2 = 2차 진화)")]
    public PetSprite[] sprites = new PetSprite[3];

    [Header("펫 시작 스킬 목록")]
    [Tooltip("SkillBase 구현 SO를 드래그하세요")]
    [SerializeField] private List<SkillData> startSkills = new List<SkillData>();

    //읽기 전용
    public List<SkillData> StartSkills => startSkills;
}

[Serializable]
public class PetSprite
{
    public Sprite WorldSprite;
    public Sprite Icon;
}

//[CreateAssetMenu(fileName = "PetData", menuName = "Data/PetData")]
//public class PetData : StatData, ILevelData, ICharacterSkillSetData
//{
//    [Header("펫 ID 및 이름")]
//    public string PetID;
//    public string PetName;

//    [Header("진화 관련")]
//    [Tooltip("진화 단계별 스프라이트 (ex. 0 = 기본, 1 = 1차 진화, 2 = 2차 진화)")]
//    public PetSprite[] sprites = new PetSprite[3];

//    [Tooltip("각 진화 단계에 도달하기 위한 레벨")]
//    public EvoLevel[] evoLevels = new EvoLevel[2];

//    [Header("레벨 및 경험치")]
//    public int startLevel = 1;
//    public int startExp = 0;
//    public int baseExpToLevelUp = 50;
//    public float statMultiplierPerLevel = 1.1f;

//    [Header("진화 시 적용할 스탯 배율")]
//    public float StatMultiplier = 1.5f;

//    [Header("현재 진화 단계")]
//    [Range(0, 2)]
//    public int CurrentEvoStage = 0;

//    [Header("펫 시작 스킬 목록")]
//    [Tooltip("SkillBase 구현 SO를 드래그하세요")]
//    public List<SkillBase> startingSkills = new List<SkillBase>();

//    public Sprite GetCurrentProfileIcon() =>
//        sprites != null && sprites.Length > CurrentEvoStage ? sprites[CurrentEvoStage].Icon : null;

//    public Sprite GetCurrentWorldSprite() =>
//        sprites != null && sprites.Length > CurrentEvoStage ? sprites[CurrentEvoStage].WorldSprite : null;

//    public int StartLevel => startLevel;
//    public int StartExp => startExp;
//    public int BaseExpToLevelUp => baseExpToLevelUp;
//    public float StatMultiplierPerLevel => statMultiplierPerLevel;

//    public List<SkillBase> StartingSkills => startingSkills;
//}

//[Serializable]
//public class PetSprite
//{
//    public Sprite WorldSprite;
//    public Sprite Icon;
//}