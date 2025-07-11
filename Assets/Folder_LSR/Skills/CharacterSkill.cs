using System.Collections.Generic;
using UnityEngine;

// 스킬 공통 상태 보관 컴포넌트
public class CharacterSkill : MonoBehaviour
{
    private List<SkillStatus> skillStatuses = new List<SkillStatus>();
    public List<SkillStatus> AllStatuses => skillStatuses; // IReadOnlyList<SkillStatus> 에서 변경

    /// <summary>
    /// 캐릭터 생성 시 호출: 시작 스킬 목록으로 상태 리스트 초기화
    /// </summary>
    public void Init(IEnumerable<SkillBase> templates)
    {
        skillStatuses.Clear();
        foreach (var skillTemplate in templates)
        {
            skillStatuses.Add(new SkillStatus(skillTemplate));
        }
    }

    /// <summary>
    /// 전투 턴이 끝날 때마다 호출: 쿨다운(턴 단위) 감소
    /// 이게 이미 ID 구분없이 모든 스킬 쿨다운을 깎아주는 메소드입니다.
    /// 예시: TickCooldowns(2) -> 모든 스킬의 쿨다운을 2턴씩 감소시킴
    /// 기본적으로는 1턴씩 감소시키지만, 필요에 따라 여러 턴을 한 번에 감소시킬 수 있습니다.
    /// </summary>
    public void TickCooldowns(int turns = 1)
    {
        for (int i = 0; i < turns; i++)
        {
            foreach (var skillStatus in skillStatuses)
            {
                skillStatus.ReduceCooldown();
            }
        }
    }

    /// <summary>
    /// 위 메서드가 존재하나, 모든 스킬 쿨다운을 1턴씩 감소시키는 편의 메서드입니다.
    /// </summary>
    public void TickAllCooldowns() => TickCooldowns(1);

    /// <summary>
    /// 전투 중 스킬 사용 시도
    /// </summary>
    public bool TryUseSkill(string skillId)
    {
        var skillStatus = skillStatuses.Find(status => status.Data.Id == skillId);
        if (skillStatus == null || !skillStatus.CanUse) return false;
        skillStatus.Use();
        return true;
    }

    /// <summary>
    /// 쿨다운 즉시 초기화 (스킬 즉시 재사용)
    /// </summary>
    public bool ResetCooldown(string skillId)
    {
        var skillStatus = skillStatuses.Find(status => status.Data.Id == skillId);
        if (skillStatus == null || skillStatus.Cooldown == 0) return false;
        skillStatus.ResetCooldown();
        return true;
    }

}