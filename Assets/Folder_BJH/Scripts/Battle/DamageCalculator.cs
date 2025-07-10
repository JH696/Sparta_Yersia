using UnityEngine;

public class DamageCalculator
{
    public float DamageCalculate(BaseCharacter attacker, BaseCharacter target, SkillData skill)
    {
        // 공격자 능력치
        float atk = attacker.Attack;
        float luck = attacker.Luck;

        // 피격자 능력치
        float def = target.Defense;
        float spd = target.Speed;

        Debug.Log($"공격자 공격력: {atk}, 행운: {luck}");
        Debug.Log($"피격자 방어력: {def}, 속도: {spd}");

        // 데미지 계산
        float power = skill == null? atk : atk * skill.Damage;
        float damage = IsCritical(luck) ? power * 1.5f : power;
        float finalDamage = IsDodge(spd) ? damage * 0 : damage - (target.Defense * 0.5f);

        Debug.Log($"최종 데미지: {finalDamage}");

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

    // 회피 발생 여부
    public bool IsDodge(float speed)
    {
        float roll = Random.Range(0, 100);

        if (speed >= roll)
        {
            return true;
        }

        return false;
    }

}
