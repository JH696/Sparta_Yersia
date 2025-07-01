using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum ESkillType
{
    Fire,
    Ice,
    Nature,
    Physical,
    All
}

public enum ESkillTier
{
    Beginner = 1, // 1=초급
    Intermediate = 2, // 2=중급
    Advanced = 3 // 3=상급
}

[CreateAssetMenu(menuName = "Data/SkillData")]
public class SkillData : ScriptableObject
{
    public string SkillName;
    public ESkillType SkillType;
    public ESkillTier Tier;
    public int UnlockCost;    // 해금에 필요한 포인트
    public int MaxLevel;
    public Sprite Icon;
    [TextArea] public string Description;
}

[CreateAssetMenu(menuName = "Data/SkillLibrary")]
public class SkillLibrary : ScriptableObject
{
    public SkillData[] AllSkills;

    public IEnumerable<SkillData> GetByCategory(ESkillType type)
      => AllSkills.Where(skills => skills.SkillType == type);

    public IEnumerable<SkillData> GetByTier(ESkillTier tier)
      => AllSkills.Where(s => s.Tier == tier);
}
