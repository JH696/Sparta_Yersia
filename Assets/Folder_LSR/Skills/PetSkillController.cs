using System.Linq;
using UnityEngine;

public class PetSkillController : MonoBehaviour
{
    private CharacterSkill _skillHolder;
    private PetData _petData;
    private int _evoStage;

    /// <summary>
    /// 외부에서 반드시 Init으로 초기화해 주어야 합니다.
    /// </summary>
    public void Init(CharacterSkill skillHolder, PetData petData, int evoStage)
    {
        _skillHolder = skillHolder;
        _petData = petData;
        _evoStage = Mathf.Clamp(evoStage, 1, 3);

        var templates = _petData.startingSkills
            .Cast<SkillBase>()
            .Take(_evoStage);
        _skillHolder.Init(templates);
    }

    public bool UsePetSkill(string id)
    {
        return _skillHolder.TryUseSkill(id);
    }

    public SkillStatus[] GetUsableSkills()
        => _skillHolder.AllStatuses.Where(s => s.CanUse).ToArray();
}
