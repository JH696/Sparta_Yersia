using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/MonsterData")]
public class MonsterData : StatData, ISkillLearnableCharacter
{
    [Header("몬스터 ID / 이름")]
    public string MonsterID;
    public string MonsterName;

    [Header("서식지")]
    public E_StageType StageType;

    [Header("몬스터 등급")]
    public EMonsterType MonsterType;

    [Header("초기 스킬 리스트")]
    [SerializeField] private List<SkillData> startSkills = new List<SkillData>();

    [Header("학습 가능 스킬 리스트")]
    [SerializeField] private List<SkillData> learnableSkills = new List<SkillData>();

    [Header("처치 보상")]
    public int ypDrop = 0;
    public int expDrop = 0;
    public List<DropItem> dropItems = new List<DropItem>();

    [Header("몬스터 스프라이트")]
    public Sprite WSprite; // 월드


    public List<SkillData> StartSkills => startSkills;
    public List<SkillData> LearnableSkills => learnableSkills;
}

[System.Serializable]
public struct DropItem
{
    public BaseItem itemData;
    public float dropRate;
}