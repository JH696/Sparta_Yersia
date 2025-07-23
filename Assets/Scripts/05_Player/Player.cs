using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("플레이어 상태")]
    [SerializeField] private PlayerStatus status;

    [Header("플레이어 데이터")]
    [SerializeField] private PlayerData playerData;

    [Header("월드에서 보여질 스프라이트")]
    public SpriteRenderer worldSprite;

    public PlayerStatus Status => status; // 읽기 전용
    public PlayerParty Party => status?.party; // 펫 등 파티 관련 접근은 PlayerParty를 통해


    private void Start()
    {
        // 플레이어 데이터와 이름 설정
        status = new PlayerStatus(playerData, "Player");

        // petParent를 플레이어 자신의 Transform으로 지정
        status.party.Initialize(this.transform);

        ChangeSprite();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            status.stat.AddExp(50); // 50 경험치 획득
        }

        // 테스트용 펫 지급 키 : T
        if (Input.GetKeyDown(KeyCode.T))
        {
            GiveTestPet();
        }
    }

    private void ChangeSprite()
    {
        if (status == null) return;
        worldSprite.sprite = status.PlayerData.WSprite;
    }

    public void SetCurrentHp(float hp)
    {
        status.stat.CurrentHp = Mathf.Clamp(hp, 0, status.stat.MaxHp);
    }

    public void SetCurrentMana(float mana)
    {
        status.stat.CurrentMana = Mathf.Clamp(mana, 0, status.stat.MaxMana);
    }

    public void SetPlayerName(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            status.PlayerName = name;
        }
    }

    private void GiveTestPet()
    {
        PetData petData = Resources.Load<PetData>("PetData/P_p01");

        if (petData == null)
        {
            Debug.LogWarning("테스트 펫 데이터를 찾을 수 없습니다: PetData/P_p01");
            return;
        }

        PetStatus pet = new PetStatus(petData);

        Party.AddPet(pet);

        Debug.Log($"테스트 펫 '{petData.PetName}' 지급 완료");
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