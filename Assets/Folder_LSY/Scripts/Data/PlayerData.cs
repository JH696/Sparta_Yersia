using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
public class PlayerData : CharacterData, ILevelData, IYPHolder, ICharacterSkillSetData
{
    [Header("플레이어 성별")]
    public EGender gender = EGender.Male;

    [Header("레벨 및 경험치")]
    public int startLevel = 1;
    public int startExp = 0;
    public int baseExpToLevelUp = 100;
    public float statMultiplierPerLevel = 1.1f;

    [Header("YP (화폐)")]
    public int startYP = 0;

    [Header("초기 스킬 리스트")]
    [SerializeField] private List<SkillData> startSkills = new List<SkillData>();
    public IReadOnlyList<SkillData> StartSkills => startSkills;

    public int StartLevel => startLevel;
    public int StartExp => startExp;
    public int BaseExpToLevelUp => baseExpToLevelUp;
    public float StatMultiplierPerLevel => statMultiplierPerLevel;
    public int StartYP => startYP;

    [Header("플레이어 시작 스킬 목록")]
    [Tooltip("SkillBase 구현 SO(SkillData 등)를 드래그하세요")]
    public List<SkillBase> startingSkills = new List<SkillBase>();

    // 인터페이스 구현
    public List<SkillBase> StartingSkills => startingSkills;
}