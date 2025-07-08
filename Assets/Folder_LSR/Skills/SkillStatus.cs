using System;
using UnityEngine;

public class SkillStatus
{
    public ISkillInfo Data { get; }
    public ESkillState State { get; private set; }
    public bool IsOnCoolTime { get; private set; }
    public int Level { get; private set; }

    public bool IsUnlocked => State != ESkillState.Locked;
    public bool CanUse => IsUnlocked && !IsOnCoolTime;
    public bool CanLevelUp(int availablePoint) => IsUnlocked && availablePoint >= NextLevelCost();

    public event Action<SkillStatus> OnStateChanged;
    public event Action<SkillStatus> OnLevelUp;

    public SkillStatus(SkillData data)
    {
        Data = data;
        State = ESkillState.Locked;
        IsOnCoolTime = false;
        Level = 0;
    }

    /// <summary>
    /// 퀘스트완료 혹은 그 외 방법으로 해금 (잠금 해제) 
    /// </summary>
    public void Unlock()
    {
        if (State != ESkillState.Locked) return;

        State = ESkillState.Available;
        Level = 1;
        OnStateChanged?.Invoke(this);
    }

    /// <summary>
    /// 스킬 사용 처리 (쿨타임 시작)
    /// </summary>
    public void Use()
    {
        if (!CanUse) return;

        IsOnCoolTime = true;
        State = ESkillState.OnCoolTIme;
        OnStateChanged?.Invoke(this);
    }

    /// <summary>
    /// 쿨타임 해제 (재사용 가능 상태로 전환)
    /// </summary>
    public void ResetCoolTime()
    {
        if (!IsOnCoolTime) return;

        IsOnCoolTime = false;
        State = ESkillState.ReadyCoolTime;
        OnStateChanged?.Invoke(this);
    }

    // 레벨업 시도
    public bool LevelUp(ref int availablePoint)
    {
        int cost = NextLevelCost();
        if (!IsUnlocked || availablePoint < cost) return false;

        availablePoint -= cost;
        Level++;
        OnLevelUp?.Invoke(this);
        return true;
    }

    // 다음 레벨업에 필요한 스킬 포인트 비용: 임시 계산식임 - 언제든지 변경가능
    public int NextLevelCost()
    {
        return Level * Level + 1;
    }
}
