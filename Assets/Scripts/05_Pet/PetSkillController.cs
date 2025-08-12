using System.Linq;
using UnityEngine;

public class PetSkillController : MonoBehaviour
{
    //private CharacterSkill characterSkill;
    //private PetData petData;
    //private int evoStage;

    //public void Init(CharacterSkill skill, PetData data, int stage)
    //{
    //    characterSkill = skill;
    //    petData = data;
    //    evoStage = Mathf.Clamp(evoStage, 1, 3);

    //    var templates = petData.startingSkills
    //        .Cast<SkillBase>()
    //        .Take(evoStage);
    //    characterSkill.Init(templates);
    //}

    //public bool UsePetSkill(string skillId)
    //{
    //    return characterSkill.TryUseSkill(skillId);
    //}

    //public SkillStatus[] GetUsableSkills()
    //    => characterSkill.AllStatuses.Where(status => status.CanUse).ToArray();
}
