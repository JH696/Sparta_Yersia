using System.Collections.Generic;
using UnityEngine;

public class MonsterAction : MonoBehaviour
{
    [SerializeField] private B_Characters chars;

    public void MonsterAttack(CharacterStatus monster)
    {
        DamageCalculator cal = new DamageCalculator();
        
        List<CharacterStatus> targets = new List<CharacterStatus>();

        foreach (B_CharacterSlot slot in chars.CSlots)
        {
            if (slot.Character != null)
            {
                if (!slot.Character.IsDead)
                {
                    targets.Add(slot.Character);
                }
            }
        }

        int roll = Random.Range(0, targets.Count);

        if (targets.Count <= 0) return;

        targets[roll].TakeDamage(cal.DamageCalculate(monster, targets[roll], null));
    }
}
