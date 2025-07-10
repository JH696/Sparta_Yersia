using System.Linq;
using UnityEngine;

// 플레이어 스킬 전반 ( 해금, 사용, 쿨타임, 다음 스킬 해금 등)
public class PlayerSkillController : MonoBehaviour
{
    private CharacterSkill _skillHolder;
    private PlayerData _playerData;
    private int _skillPoints;

    /// <summary>
    /// 외부에서 반드시 Init으로 초기화해주기. 아래 주석부분 복사 붙여넣기후 주석 해제하여 사용하면 됨.
    //void SetupParty()
    //{
    //    // 플레이어 세팅
    //    var playerGO = Instantiate(playerPrefab);
    //    var charSkill = playerGO.GetComponent<CharacterSkill>();
    //    var playerCtrl = playerGO.AddComponent<PlayerSkillController>();
    //    playerCtrl.Init(charSkill, playerDataAsset, startingSkillPoints);

    //    // NPC 세팅
    //    var npcGO = Instantiate(npcPrefab);
    //    var npcSkill = npcGO.GetComponent<CharacterSkill>();
    //    var npcCtrl = npcGO.AddComponent<NPCSkillController>();
    //    npcCtrl.Init(npcSkill, npcDataAsset, initialAffinity, unlockThreshold);

    //    // 펫 세팅
    //    var petGO = Instantiate(petPrefab);
    //    var petSkill = petGO.GetComponent<CharacterSkill>();
    //    var petCtrl = petGO.AddComponent<PetSkillController>();
    //    petCtrl.Init(petSkill, petDataAsset, petEvoStage);
    //}
    /// </summary>
    public void Init(CharacterSkill skillHolder, PlayerData playerData, int startingPoints)
    {
        _skillHolder = skillHolder;
        _playerData = playerData;

        _skillPoints = startingPoints;

        // 스킬 리스트 초기화
        _skillHolder.Init(_playerData.startingSkills.Cast<SkillBase>());

        // 기본 등급 스킬 자동 해금
        foreach (var s in _skillHolder.AllStatuses.Where(x => x.Data.SkillTier == ETier.Basic))
            s.Unlock();
    }

    public void UnlockSkill(string id)
    {
        var s = _skillHolder.AllStatuses.FirstOrDefault(x => x.Data.Id == id);
        s?.Unlock();
    }

    public bool LevelUpSkill(string id)
    {
        var s = _skillHolder.AllStatuses.FirstOrDefault(x => x.Data.Id == id);
        return s != null && s.LevelUp(ref _skillPoints);
    }

    public bool TryUseEquippedSkill(string id)
    {
        return _skillHolder.TryUseSkill(id);
    }
}
