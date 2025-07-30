using System.Collections.Generic;

// 캐릭터별 시작 스킬 목록을 제공하는 인터페이스
public interface ISkillUsable
{
    // 캐릭터가 처음부터 배우는 스킬 목록
    List<SkillData> StartSkills { get; }
}