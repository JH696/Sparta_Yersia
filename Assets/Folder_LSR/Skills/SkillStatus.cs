using System;
using UnityEngine;

public class SkillStatus : MonoBehaviour
{
    public SkillData Data { get; }
    public ESkillState State { get; private set; }
    public float RemainCooltime { get; private set; }

    public bool IsUnlocked => State != ESkillState.Locked;
    public bool CanUse => State == ESkillState.Available || State == ESkillState.ReadyCoolTime;

    public event Action<SkillStatus> OnStateChanged;

    public SkillStatus(SkillData data)
    {
        Data = data;
        State = ESkillState.Locked;
        RemainCooltime = 0f;
    }

    // 해금 (잠금 해제)
    public void Unlock()
    {
        if (State == ESkillState.Locked)
        {
            State = ESkillState.Available;
            OnStateChanged?.Invoke(this);
        }
        else
        {
            Debug.LogWarning($"Skill {Data.SkillName} is already unlocked.");
        }
    }

    // 스킬 사용 -> 쿨타임 시작
    public void Use()
    {
        if (!CanUse) return;

        State = ESkillState.OnCoolTIme;
        RemainCooltime = Data.CoolTime;
        OnStateChanged?.Invoke(this);
    }

    // 쿨타임 업데이트 -> 준비 완료 상태전환
    public void UpdateCoolTime(float deltaTime)
    {
        if (State == ESkillState.OnCoolTIme)
        {
            RemainCooltime -= deltaTime;
            if (RemainCooltime <= 0f)
            {
                RemainCooltime = 0f;
                State = ESkillState.ReadyCoolTime;
                OnStateChanged?.Invoke(this);
            }
        }
    }
}
