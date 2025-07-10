public enum ESkillState
{
    Locked, // 스킬 습득 전 (잠금이라 칭하겠음)
    Available, // 사용 가능 상태 (잠금해제)
    OnCooldown, // 쿨타임 중 (배틀)
    ReadyCooldown   // 쿨타임 끝나서 다시 사용 가능한 상태
}
