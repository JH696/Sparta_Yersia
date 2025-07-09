using System.Collections.Generic;

public interface ISkillInfo
{
    string Id { get; }
    string SkillName { get; }
    ESkillType SkillType { get; }
    ETier SkillTier { get; }
    float Damage { get; }
    float Coefficient { get; }
    float Range { get; }
    float CoolTime { get; }
    IReadOnlyList<ISkillInfo> UnlockNext { get; }
}