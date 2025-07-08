using System;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 스킬 전반 ( 해금, 사용, 쿨타임, 다음 스킬 해금 등)
public class PlayerSkillController : MonoBehaviour
{
    [Header("스킬 데이터 목록 SO")]

    [Header("플레이어 스킬 포인트")]
    [SerializeField] private int skillPoints = 100000; // 임시로 포인트 부여

    // 키: SkillData.Id, 값: SkillStatus 인스턴스
    private Dictionary<string, SkillStatus> skillStatuses = new Dictionary<string, SkillStatus>();

    //스킬 상태 변경 알림 이벤트(UI)
    public event Action<SkillStatus> OnSkillStateChanged;
    public event Action<SkillStatus> OnSkillLevelUp;

    private void Awake()
    {
        // SO → SkillStatus 인스턴스 생성 및 등록
    }

    private void Start()
    {
        // 초급 스킬 자동 해금 - 임시 로직
        foreach (var status in skillStatuses.Values)
        {
            if (status.Data.SkillTier == ETier.Basic)
            {
                status.Unlock();
            }
        }
    }

    /// <summary>스킬 사용 시도</summary>
    public void TryUseSkill(string skillID)
    {
        if (!skillStatuses.TryGetValue(skillID, out var status)) return;
    }

    private void ApplySkillEffect(SkillData data)
    {
        float atk = GetPlayerAttackPower();
        float dmg = data.Damage + atk * data.Coefficient;
        Debug.Log($"[{data.Id}] 사용, 데미지: {dmg}");
    }

    private void NotifySkillChanged(SkillStatus status)
    {

    }

    private float GetPlayerAttackPower() => 50f; // TODO: 실제 스탯 연결
}
