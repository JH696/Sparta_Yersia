using System.Collections.Generic;

public interface ISkillInfo
{
    string Id { get; }
    string SkillName { get; }
    ESkillType SkillType { get; }
    ETier SkillTier { get; }
    int Damage { get; }
    int Coefficient { get; }
    int Range { get; }
    int CoolTime { get; }
    IReadOnlyList<ISkillInfo> UnlockNext { get; }
}