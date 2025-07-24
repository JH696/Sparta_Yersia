using System.Collections.Generic;

// 캐릭터별 시작 스킬 목록을 제공하는 인터페이스
public interface ISkillLearnableCharacter
{
    // 캐릭터가 처음부터 배우는 스킬 목록
    List<SkillData> StartSkills { get; }

    // 캐릭터가 이후에 배울 수 있는 스킬 목록
    List<SkillData> LearnableSkills { get; }
}