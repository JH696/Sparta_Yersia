using System.Linq;
using UnityEngine;

public class NPCSkillController : MonoBehaviour
{
    //private CharacterSkill characterSkill;
    //private NPCData npcData;
    //private int affinity;
    //private int unlockSkill;

    //public void Init(CharacterSkill skill, NPCData data, int initialAffinity, int unlock)
    //{
    //    characterSkill = skill;
    //    npcData = data;
    //    affinity = initialAffinity;
    //    unlockSkill = unlock;

    //    characterSkill.Init(npcData.startingSkills.Cast<SkillBase>());
    //}

    //public void IncreaseAffinity(int amount)
    //{
    //    affinity = Mathf.Clamp(affinity + amount, 0, 100);
    //    int unlockCount = affinity / 20; // 임시 20% 단위
    //    var list = characterSkill.AllStatuses;
    //    for (int i = 0; i < unlockCount && i < list.Count; i++)
    //    {
    //        list[i].Unlock();
    //    }
    //}

    //public SkillStatus[] GetUsableSkills()
    //    => characterSkill.AllStatuses.Where(status => status.CanUse).ToArray();
}
