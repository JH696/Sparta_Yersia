using UnityEngine;
public class DamageCalculator
{
    public float DamageCalculate(BaseCharacter stats, SkillData skill)
    {
        float Attack = stats.Attack; // 공격력
        float Damage = Attack * skill.Damage; // 공격력 * 스킬 배율
        float finalDamage = IsCritical(stats.Luck)? Damage * 1.5f : Damage; // 치명타 피해 적용

        Debug.Log($"최종 피해량: {finalDamage}");
        return finalDamage;
    }

    public bool IsCritical(float luck)
    {
        float roll = UnityEngine.Random.Range(0f, 100f); // 치명타 계산
        bool isCritical = roll <= luck; // luck과 같거나 그보다 작으면

        return isCritical;
    }
}
