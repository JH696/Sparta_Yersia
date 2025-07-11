using System.Collections.Generic;
using UnityEngine;

// 정적 데이터만
[CreateAssetMenu(fileName = "SF_a01", menuName = "Data/SkillData")]
public class SkillData : SkillBase
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

    [Header("다음 해금 스킬 목록")]
    [SerializeField] private List<SkillData> unlockNext = new List<SkillData>();

    [Header("아이콘 & 마나 소모량")]
    [SerializeField] private Sprite icon;
    [SerializeField] private int manaCost;

    public override string Id => id;
    public override string SkillName => skillName;
    public override ESkillType SkillType => type;
    public override ETier SkillTier => tier;
    public override int Damage => damage;
    public override int Coefficient => coefficient;
    public override int Range => range;
    public override int Cooldown => cooldown;
    public override IReadOnlyList<SkillBase> UnlockNext
        => unlockNext.ConvertAll(status => (SkillBase)status);

    public override Sprite Icon => icon;
    public override int ManaCost => manaCost;
}