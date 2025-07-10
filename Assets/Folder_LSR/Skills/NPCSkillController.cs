using System.Linq;
using UnityEngine;

public class NPCSkillController : MonoBehaviour
{
    private CharacterSkill _characterSkill;
    private NPCData _npcData;
    private int _affinity;
    private int _unlockThreshold;

    public void Init(CharacterSkill characterSkill, NPCData npcData, int initialAffinity, int unlockThreshold)
    {
        _characterSkill = characterSkill;
        _npcData = npcData;
        _affinity = initialAffinity;
        _unlockThreshold = unlockThreshold;

        _characterSkill.Init(_npcData.startingSkills.Cast<SkillBase>());
    }

    public void IncreaseAffinity(int amount)
    {
        _affinity = Mathf.Clamp(_affinity + amount, 0, 100);
        int unlockCount = _affinity / 20; // 임시 20% 단위
        var list = _characterSkill.AllStatuses;
        for (int i = 0; i < unlockCount && i < list.Count; i++)
            list[i].Unlock();
    }

    public SkillStatus[] GetUsableSkills()
        => _characterSkill.AllStatuses.Where(status => status.CanUse).ToArray();
}
