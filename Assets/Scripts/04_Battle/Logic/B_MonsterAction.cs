//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class B_MonsterAction : MonoBehaviour
//{
//    [Header("슬롯 매니저")]
//    [SerializeField] private B_SlotManager slotManager;

//    public void MonsterAttack(B_Slot slot)
//    {
//        CharacterStatus curStatus = slot.Character;

//        List<BattleEffecter> effecters = slotManager.GetNonEmptySlots()
//            .Where(slot => slot.GetSlotType() == E_B_SlotType.Ally)
//            .Select(slot => slot.GetComponentInChildren<BattleEffecter>())
//            .ToList();

//        if (effecters.Count == 0)
//        {
//            slotManager.ClearCurrentSlot();
//            return;
//        }

//        List<SkillStatus> skills = curStatus.skills.EquipSkills;
//        List<SkillStatus> castableSkills = skills
//            .Where(skill => skill.CanCast(curStatus))
//            .ToList();

//        List<BattleEffecter> randomTargets = new List<BattleEffecter>();

//        if (castableSkills.Count <= 0 || Random.value <= 0.75f)
//        {
//            BattleEffecter target = effecters[Random.Range(0, effecters.Count)];

//            target.SetBaseEffect();
//            Debug.Log($"몬스터 {curStatus}이(가) {target.Character}에게 {dmg}의 피해를 입혔습니다.");
//        }
//        else
//        {
//            SkillStatus randomSkill = castableSkills[Random.Range(0, castableSkills.Count)];

//            int range = Mathf.Min(randomSkill.Data.Range, allSlots.Count);

//            List<int> indices = Enumerable.Range(0, allSlots.Count).ToList();
//            for (int i = 0; i < range; i++)
//            {
//                int rand = Random.Range(i, indices.Count);
//                (indices[i], indices[rand]) = (indices[rand], indices[i]);
//                randomTargets.Add(allSlots[indices[i]]);
//            }

//            randomSkill.Cast(curStatus);

//            foreach (B_Slot t in randomTargets)
//            {
//                float dmg = cal.DamageCalculate(curStatus.stat, t.Character.stat, randomSkill);
//                t.Character.TakeDamage(dmg);
//                Debug.Log($"몬스터 {curStatus}이(가) {t.Character}에게 {dmg}의 피해를 입혔습니다.");
//            }
//        }

//        slotManager.ClearCurrentSlot();
//    }
//}
