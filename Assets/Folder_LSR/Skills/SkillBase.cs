using System.Collections.Generic;
using UnityEngine;

// 스킬의 정적데이터를 정의하는 추상 클래스
public abstract class SkillBase : ScriptableObject
{
    public abstract string Id { get; }
    public abstract string SkillName { get; }
    public abstract ESkillType SkillType { get; }
    public abstract ETier SkillTier { get; }
    public abstract int Damage { get; }
    public abstract int Coefficient { get; }
    public abstract int Range { get; }
    public abstract int Cooldown { get; }
    public abstract IReadOnlyList<SkillBase> UnlockNext { get; }

    // 플레이어,펫,NPC만 사용하는 속성 (몬스터는 null 반환)
    public virtual Sprite Icon => null;
    public virtual int ManaCost => 0;
}