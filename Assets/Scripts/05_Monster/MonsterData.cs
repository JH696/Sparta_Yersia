using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/MonsterData")]
public class MonsterData : StatData, ISkillUsable, IBattable
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

    [Header("처치 보상")]
    public int ypDrop = 0;
    public int expDrop = 0;
    public List<DropItem> dropItems = new List<DropItem>();

    [Header("배틀씬")]
    public BattleVisuals BattleVisuals;

    public List<SkillData> StartSkills => startSkills;
}

[System.Serializable]
public struct DropItem
{
    public BaseItem itemData;
    public float dropRate;
}

[System.Serializable]
public struct BattleVisuals
{
    public Sprite Stand;
    public Animation Idle;
    public Animation Attack;
    public Animation Hit;
}