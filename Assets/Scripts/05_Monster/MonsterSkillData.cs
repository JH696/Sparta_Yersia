using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MSF_a01", menuName = "Data/MonsterSkillData")]
public class MonsterSkillData : SkillBase
{
    [Header("ID / 이름")]
    [SerializeField] private string id;
    [SerializeField] private string skillName;

    [Header("속성 / 등급")]
    [SerializeField] private ESkillType type;
    [SerializeField] private ETier tier;

    [Header("전투용 스탯")]
    [SerializeField] private int damage;
    [SerializeField] private int coefficient;
    [SerializeField] private int range;
    [SerializeField] private int cooldown;

    public override string Id => id;
    public override string SkillName => skillName;
    public override ESkillType SkillType => type;
    public override ETier SkillTier => tier;
    public override float Damage => damage;
    public override int Coefficient => coefficient;
    public override int Range => range;
    public override int Cooldown => cooldown;
    public override IReadOnlyList<SkillBase> UnlockNext
        => Array.Empty<SkillBase>();
}
