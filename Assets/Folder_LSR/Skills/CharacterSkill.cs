using System.Collections.Generic;
using UnityEngine;

// 스킬 공통 상태 보관 컴포넌트
public class CharacterSkill : MonoBehaviour
{
    private List<SkillStatus> skillStatuses = new List<SkillStatus>();
    public IReadOnlyList<SkillStatus> AllStatuses => skillStatuses;

    /// <summary>
    /// 캐릭터 생성 시 호출: 시작 스킬 목록으로 상태 리스트 초기화
    /// </summary>
    public void Init(IEnumerable<SkillBase> templates)
    {
        skillStatuses.Clear();
        foreach (var tpl in templates)
            skillStatuses.Add(new SkillStatus(tpl));
    }

    /// <summary>
    /// 전투 턴이 끝날 때마다 호출: 쿨다운(턴 단위) 감소
    /// </summary>
    public void TickCooldowns(int deltaTurns = 1)
    {
        for (int i = 0; i < deltaTurns; i++)
            foreach (var s in skillStatuses)
                s.ReduceCooldown();
    }

    /// <summary>
    /// 전투 중 스킬 사용 시도
    /// </summary>
    public bool TryUseSkill(string id)
    {
        var s = skillStatuses.Find(x => x.Data.Id == id);
        if (s == null || !s.CanUse) return false;
        s.Use();
        return true;
    }

    /// <summary>
    /// 쿨다운 즉시 초기화 (스킬 즉시 재사용)
    /// </summary>
    public bool ResetCooldown(string id)
    {
        var s = skillStatuses.Find(x => x.Data.Id == id);
        if (s == null || s.Cooldown == 0) return false;
        s.ResetCooldown();
        return true;
    }
}