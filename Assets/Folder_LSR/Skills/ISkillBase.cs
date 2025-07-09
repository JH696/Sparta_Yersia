using System.Collections.Generic;

public interface ISkillBase
{
    string Id { get; }
    string SkillName { get; }
    ESkillType SkillType { get; }
    ETier SkillTier { get; }
    float Damage { get; }
    int Coefficient { get; }
    int Range { get; }
    int CoolTime { get; }
    IReadOnlyList<ISkillBase> UnlockNext { get; }
}