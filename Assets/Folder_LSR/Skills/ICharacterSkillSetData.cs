using System.Collections.Generic;

// 캐릭터별 시작 스킬 목록을 제공하는 인터페이스
public interface ICharacterSkillSetData
{
    List<SkillBase> StartingSkills { get; }
}