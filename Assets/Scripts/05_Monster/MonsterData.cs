using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/MonsterData")]
public class MonsterData : StatData, ICharacterSkillSetData
{
    [Header("몬스터 ID / 이름")]
    public string MonsterID;
    public string MonsterName;

    [Header("몬스터 등급")]
    public EMonsterType MonsterType;

    [Header("초기 스킬 리스트")]
    [SerializeField] private List<SkillData> startSkills = new List<SkillData>();
    public IReadOnlyList<SkillData> StartSkills => startSkills;

    [Header("몬스터 시작 스킬 목록")]
    [Tooltip("SkillBase 구현 SO(SkillData 등)를 드래그하세요")]
    public List<SkillBase> startingSkills = new List<SkillBase>();

    // 인터페이스 구현
    public List<SkillBase> StartingSkills => startingSkills;

    [Header("처치 보상")]
    public int ypDrop = 0;
    public int expDrop = 0;
    public List<DropItemData> dropItems = new List<DropItemData>();
}