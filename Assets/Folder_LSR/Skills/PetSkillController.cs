using System.Linq;
using UnityEngine;

public class PetSkillController : MonoBehaviour
{
    private CharacterSkill _characterSkill;
    private PetData _petData;
    private int _evoStage;

    /// <summary>
    /// 외부에서 반드시 Init으로 초기화해 주어야 합니다.
    /// </summary>
    public void Init(CharacterSkill characterSkill, PetData petData, int evoStage)
    {
        _characterSkill = characterSkill;
        _petData = petData;
        _evoStage = Mathf.Clamp(evoStage, 1, 3);

        var templates = _petData.startingSkills
            .Cast<SkillBase>()
            .Take(_evoStage);
        _characterSkill.Init(templates);
    }

    public bool UsePetSkill(string skillId)
    {
        return _characterSkill.TryUseSkill(skillId);
    }

    public SkillStatus[] GetUsableSkills()
        => _characterSkill.AllStatuses.Where(status => status.CanUse).ToArray();
}
