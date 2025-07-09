using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MSF_a01", menuName = "Data/MonsterSkillData")]
public class MonsterSkillData : ScriptableObject, ISkillBase
{
    [Header("ID / 이름")]
    [SerializeField] private string skillID;
    [SerializeField] private string skillName;

    [Header("속성 / 등급")]
    [SerializeField] private ESkillType type;
    [SerializeField] private ETier tier;

    [Header("전투용 스탯")]
    [Tooltip("기본 데미지")] [SerializeField] private float damage;
    [Tooltip("공격력 계수")] [SerializeField] private int coefficient;
    [Tooltip("공격 범위")] [SerializeField] private int range;
    [Tooltip("쿨타임(초)")] [SerializeField] private int coolTime;

    // ISkillBase 
    public string Id => skillID;
    public string SkillName => skillName;
    public ESkillType SkillType => type;
    public ETier SkillTier => tier;
    public float Damage => damage;
    public int Coefficient => coefficient;
    public int Range => range;
    public int CoolTime => coolTime;

    // 몬스터 스킬은 해금X : 빈 리스트 반환
    public IReadOnlyList<ISkillBase> UnlockNext => Array.Empty<ISkillBase>();
}
