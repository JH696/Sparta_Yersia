using UnityEngine;

public class DamageCalculator
{
    /// <summary>
    /// 공격자, 피격자, 스킬 사용이라면 스킬 상태까지. 아니라면 null을 전달.
    /// 예시: DamageCalculator cal = new DamageCalculator(attacker, target, null);
    /// </summary>
    public float DamageCalculate(CharacterStats attacker, CharacterStats target, SkillStatus skill)
    {
        // 공격자 능력치
        float atk = attacker.Attack;
        float luck = attacker.Luck;

        // 피격자 능력치
        float def = target.Defense;

        // 데미지 계산
        float power = skill == null? atk : atk * skill.Power;
        float damage = IsCritical(luck) ? power * 1.5f : power;
        float finalDamage = damage - (target.Defense * 0.5f);

        return finalDamage;
    }

    // 치명타 발생 여부
    public bool IsCritical(float luck)
    {
        float roll = Random.Range(0, 100);

        if (luck >= roll)
        {
            return true;
        }

        return false;
    }
}
