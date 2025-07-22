using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("플레이어 상태")]
    [SerializeField] private PlayerStatus status = null;

    [Header("플레이어 데이터")]
    [SerializeField] private PlayerData playerData;

    [Header("월드에서 보여질 스프라이트")]
    public SpriteRenderer worldSprite;

    public PlayerStatus Status => status; // 읽기 전용
    public PlayerData PlayerData => playerData; // 읽기 전용

    private void Start()
    {
        if (BattleManager.player != null)
        {
            status = BattleManager.player; // 전투 매니저에서 플레이어 상태를 가져옴
        }
        else
        {
            status = new PlayerStatus(playerData, "Player"); // 플레이어 데이터와 이름 설정
            Debug.Log("[Player] PlayerStatus가 초기화되었습니다.");
        }

        ChangeSprite();
    }

    private void ChangeSprite()
    {
        if (status == null) return;
        worldSprite.sprite = playerData.WSprite;
    }




    //public PlayerSaveData makeSaveData()
    //{
    //    PlayerSaveData data = new PlayerSaveData();

    //    data.CurrnetHP = status.stat.CurrentHp;
    //    data.CurrentMP = status.stat.CurrentMana;
    //    data.Level = status.Level;
    //    data.CurrentExp = status.Exp;
    //    data.YP = status.wallet.YP;
    //    data.Inventory = inventory.GetAllItems();

    //    foreach (var quest in quest.GetMyQStatus())
    //    {
    //        data.questStatusDatas.Add(new QuestStatusData
    //        {
    //            QuestID = quest.Key,
    //            IsCompleted = quest.Value.IsCleared
    //        });
    //        Debug.Log($"퀘스트 저장: {quest.Key}, 완료 여부: {quest.Value.IsCleared}");
    //    }

    //    foreach (var q in quest.GetEliQProgress())
    //    {
    //        EliQuestProgressData eliData = new EliQuestProgressData
    //        {
    //            QuestID = q.Key
    //        };

    //        foreach (var kvp in q.Value.EliCounts)
    //        {
    //            eliData.eliCountDatas.Add(new EliCountData
    //            {
    //                EnemyID = kvp.Key,
    //                KillCount = kvp.Value
    //            });
    //        }

    //        data.eliQuestProgressDatas.Add(eliData);
    //        Debug.Log($"엘리 퀘스트 저장: {q.Key}");
    //    }

    //    foreach (var pet in status.party.curPets)
    //    {
    //        data.ownedPetIDs.Add(pet.PetData.PetID);
    //    }

    //    foreach (var pet in status.party.partyPets)
    //    {
    //        data.equipPetIDs.Add(pet.PetData.PetID);
    //    }

    //    Debug.Log($"보유 펫 저장: {data.ownedPetIDs.Count}마리, 장착 펫 저장: {data.equipPetIDs.Count}마리");

    //    return data;
    //}

    //public void LoadData(PlayerSaveData data)
    //{
    //    SetCurrentHp(data.CurrnetHP);
    //    SetCurrentMana(data.CurrentMP);
    //    status.SetLevel(data.Level);
    //    status.SetExp(data.CurrentExp);
    //    status.wallet.SetYP(data.YP);

    //    foreach (var item in data.Inventory)
    //    {
    //        BaseItem itemData = Resources.Load<BaseItem>($"ItemDatas/{item.Key}");
    //        inventory.AddItem(itemData, item.Value);
    //    }

    //    foreach (var q in data.questStatusDatas)
    //    {
    //        QuestData questData = Resources.Load<QuestData>($"QuestDatas/{q.QuestID}");
    //        quest.AddMyQ(questData);

    //        if (q.IsCompleted)
    //        {
    //            quest.GetMyQStatus()[q.QuestID].IsCleared = true;
    //        }
    //    }

    //    foreach (var eliData in data.eliQuestProgressDatas)
    //    {
    //        QuestData progress = Resources.Load<QuestData>($"QuestDatas/{eliData.QuestID}");
    //        quest.AddEliQ(progress);

    //        foreach (var countData in eliData.eliCountDatas)
    //        {
    //            quest.GetEliQProgress()[eliData.QuestID].EliCounts[countData.EnemyID] = countData.KillCount;
    //        }
    //    }

    //    // 펫 보유 리스트 복원: PetData 로드 후 PetStatus 생성하여 PlayerParty에 추가
    //    foreach (var petID in data.ownedPetIDs)
    //    {
    //        PetData petData = Resources.Load<PetData>($"PetData/{petID}");
    //        if (petData != null)
    //        {
    //            var petStatus = new PetStatus(petData);
    //            status.party.AddPet(petStatus);
    //        }
    //        else
    //        {
    //            Debug.LogWarning($"펫 데이터를 찾을 수 없습니다: {petID}");
    //        }
    //    }

    //    // 펫 장착 리스트 복원: 보유한 PetStatus 중 해당 펫을 찾아 PlayerParty에 장착
    //    foreach (var petID in data.equipPetIDs)
    //    {
    //        var petStatus = status.party.curPets.Find(p => p.PetData.PetID == petID);
    //        if (petStatus != null)
    //        {
    //            status.party.EquipPet(petStatus);
    //        }
    //        else
    //        {
    //            Debug.LogWarning($"장착할 보유 펫을 찾을 수 없습니다: {petID}");
    //        }
    //    }
    //}
}