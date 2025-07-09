using System.Collections.Generic;
using UnityEngine;

// 정적 데이터만
[CreateAssetMenu(fileName = "SF_a01", menuName = "Data/SkillData")]
public class SkillData : ScriptableObject, ISkillBase
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

    [Header("다음 해금 스킬 목록(SO 참조)")]
    [SerializeField] private List<SkillData> unlockNext = new List<SkillData>();

    [Header("아이콘")]
    [SerializeField] private Sprite icon;
    [Header("마나 소모량")]
    [SerializeField] private int manaCost = 0;

    // ISkillBase 
    public string Id => skillID;
    public string SkillName => skillName;
    public ESkillType SkillType => type;
    public ETier SkillTier => tier;
    public float Damage => damage;
    public int Coefficient => coefficient;
    public int Range => range;
    public int CoolTime => coolTime;
    public IReadOnlyList<ISkillBase> UnlockNext => unlockNext.ConvertAll(nextSkill => (ISkillBase)nextSkill);

    // 추가 속성
    public Sprite Icon => icon;
    public int ManaCost => manaCost;
}