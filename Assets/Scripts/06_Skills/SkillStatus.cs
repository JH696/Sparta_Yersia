//using System;

//// 런타임에서 스킬 해금, 사용, 쿨다운, 레벨 관리 담당
//public class SkillStatus
//{
//    public SkillBase Data { get; }
//    public ESkillState State { get; private set; }
//    public int Level { get; private set; }
//    public int Cooldown { get; private set; }

//    public bool IsUnlocked => State != ESkillState.Locked;
//    public bool CanUse => IsUnlocked && Cooldown == 0;

//    public event Action<SkillStatus> OnStateChanged;
//    public event Action<SkillStatus> OnLevelChanged;

//    public SkillStatus(SkillBase data)
//    {
//        Data = data;
//        State = ESkillState.Locked;
//        Cooldown = 0;
//        Level = 0;
//    }

//    /// <summary>스킬 획득 시 호출 (레벨 1로 시작)</summary>
//    public void Unlock()
//    {
//        if (State != ESkillState.Locked) return;
//        State = ESkillState.Available;
//        Level = 1;
//        OnStateChanged?.Invoke(this);
//    }

//    /// <summary>스킬 사용 시 호출 (쿨다운 턴 설정)</summary>
//    public void Use()
//    {
//        //if (!CanUse) return;
//        Cooldown = Data.Cooldown;
//        State = ESkillState.OnCooldown;
//        OnStateChanged?.Invoke(this);
//    }

//    /// <summary>전투 턴마다 호출: 쿨다운 턴 감소</summary>
//    public void ReduceCooldown()
//    {
//        if (Cooldown == 0) return;
//        Cooldown--;
//        if (Cooldown == 0)
//        {
//            State = ESkillState.ReadyCooldown;
//            OnStateChanged?.Invoke(this);
//        }
//    }

//    /// <summary>쿨다운 즉시 초기화</summary>
//    public void ResetCooldown()
//    {
//        if (Cooldown == 0) return;
//        Cooldown = 0;
//        State = ESkillState.ReadyCooldown;
//        OnStateChanged?.Invoke(this);
//    }

//    /// <summary>플레이어 전용: 스킬 레벨업</summary>
//    public bool LevelUp(ref int points)
//    {
//        int cost = NextLevelCost();
//        if (!IsUnlocked || points < cost) return false;
//        points -= cost;
//        Level++;
//        OnLevelChanged?.Invoke(this);
//        return true;
//    }

//    public int NextLevelCost() => Level * Level + 1;
//}
