using System;
using System.Collections.Generic;
using UnityEngine;

// 스킬 레벨업 시 부여할 전투 스탯 보너스 정의
[Serializable]
public struct SkillStatBonus
{
    [Tooltip("보너스를 적용할 스탯 종류")]
    public EStatType StatType;
    [Tooltip("레벨 1당 보너스 Max스탯")]
    public float BonusPerLevel;
}

[CreateAssetMenu(fileName = "SF_a01", menuName = "Data/SkillData")]
public class SkillData : ScriptableObject
{
    public string SkillId => name;

    [Header("표시할 이름")]
    [SerializeField] private string skillName;
    public string SkillName => skillName;

    [Header("속성 타입(화염/얼음/자연/물리)")]
    [SerializeField] private ESkillType type;
    public ESkillType Type => type;

    [Header("스킬 단계(초급/중급/상급)")]
    [SerializeField] private ETier tier;
    public ETier Tier => tier;

    [Header("노드용 아이콘")]
    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;

    [Header("도달 가능한 최대 레벨")]
    [SerializeField] private int maxLevel = 30;
    public int MaxLevel => maxLevel;

    [Header("전투용 스탯")]
    [SerializeField, Tooltip("쿨타임(초)")] private float coolTime;
    public float CoolTime => coolTime;

    [SerializeField, Tooltip("공격 몬스터 범위(수)")] private float range;
    public float Range => range;

    [SerializeField, Tooltip("데미지")] private float damage;
    public float Damage => damage;

    [SerializeField, Tooltip("사용 시 소비할 마나량")] private float manaCost;
    public float ManaCost => manaCost;

    [Header("레벨업 보너스")]
    [SerializeField, Tooltip("레벨업 시 부여할 추가 스탯 보너스 목록")]
    private List<SkillStatBonus> statBonuses = new List<SkillStatBonus>();
    public IReadOnlyList<SkillStatBonus> StatBonuses => statBonuses;

    [Header("선행 조건")]
    [SerializeField, Tooltip("이 스킬을 해금하기 전에 반드시 해금해야 할 스킬들")]
    private List<SkillData> prerequisites = new List<SkillData>();
    public IReadOnlyList<SkillData> Prerequisites => prerequisites;

    [Header("퀘스트 선행 조건")]
    [SerializeField, Tooltip("이 스킬을 해금하기 위해 완료해야 할 퀘스트들")]
    private List<QuestData> questPrerequisites = new List<QuestData>();
    public IReadOnlyList<QuestData> QuestPrerequisites => questPrerequisites;
}