using System;

public class SkillStatus
{
    public ISkillInfo Data { get; }
    public ESkillState State { get; private set; }
    public int Level { get; private set; }
    public int CoolTime { get; private set; }

    public bool IsUnlocked => State != ESkillState.Locked;
    public bool CanUse => IsUnlocked && CoolTime == 0;
    public bool CanLevelUp(int points) => IsUnlocked && points >= NextLevelCost();

    public event Action<SkillStatus> OnStateChanged;
    public event Action<SkillStatus> OnLevelUp;

    public SkillStatus(ISkillInfo info)
    {
        Data = info;
        State = ESkillState.Locked;
        CoolTime = 0;
        Level = 0;
    }

    /// <summary> 퀘스트완료 혹은 그 외 방법으로 해금 (잠금 해제)  </summary>
    public void Unlock()
    {
        if (State != ESkillState.Locked) return;

        State = ESkillState.Available;
        Level = 1;
        OnStateChanged?.Invoke(this);
    }

    /// <summary> 스킬 사용 처리 (쿨타임 시작) </summary>
    public void Use()
    {
        if (!CanUse) return;

        CoolTime = Data.CoolTime;
        State = ESkillState.OnCoolTIme;
        OnStateChanged?.Invoke(this);
    }

    /// <summary> 쿨타임 감소 처리 </summary>
    public void ReduceCoolTime()
    {
        if (CoolTime == 0) return; // 쿨타임이 0이면 감소하지 않음
        CoolTime--; // 1이상일 때만 감소

        if (CoolTime == 0) // 감소 후 0이 되면 다시 사용 가능 상태로 전환
        {
            State = ESkillState.ReadyCoolTime;
            OnStateChanged?.Invoke(this);
        }
    }

    /// <summary> 쿨타임 해제 (재사용 가능 상태로 전환) </summary>
    public void ResetCoolTime()
    {
        if (CoolTime == 0) return;

        CoolTime = 0;
        State = ESkillState.ReadyCoolTime;
        OnStateChanged?.Invoke(this);
    }

    // 레벨업 시도
    public bool LevelUp(ref int points)
    {
        int cost = NextLevelCost();
        if (!IsUnlocked || points < cost) return false;

        points -= cost;
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
