using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/MonsterData")]
public class MonsterData : StatData, ISkillLearnableCharacter
{
    [Header("몬스터 ID 및 이름")]
    public string MonsterID;
    public string MonsterName;

    [Header("몬스터 등급")]
    public EMonsterType MonsterType;

    [Header("몬스터 스프라이트")]
    public Sprite MonsterSprite;

    [Header("처치 보상")]
    public int ExpReward;
    public DropTableSO dropTable;

    [Header("시작 스킬 목록")]
    [SerializeField] private List<SkillData> startSkills = new List<SkillData>();

    [Header("학습 가능 스킬 리스트")]
    [SerializeField] private List<SkillData> learnableSkills = new List<SkillData>();


    //읽기 전용
    public List<SkillData> StartSkills => startSkills;
    public List<SkillData> LearnableSkills => learnableSkills;
}