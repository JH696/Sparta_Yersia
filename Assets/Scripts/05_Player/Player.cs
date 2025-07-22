using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("플레이어 상태")]
    [SerializeField] private PlayerStatus status = null;

    [Header("플레이어 데이터")]
    [SerializeField] private PlayerData playerData;

    [Header("월드에서 보여질 스프라이트")]
    public SpriteRenderer worldSprite;

    // @@ 플레이어 파티에서 관리 예정 @@
    //[Header("펫 관련")]
    //[SerializeField] private List<Pet> ownedPets = new List<Pet>();    // 보유한 펫 목록
    //[SerializeField] private List<Pet> equippedPets = new List<Pet>(); // 장착한 펫 목록 (최대 2마리)
    //public List<Pet> OwnedPets => ownedPets;
    //public List<Pet> EquippedPets => equippedPets;

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
    //    data.CurrnetHP = stat.CurrentHp;
    //    data.CurrentMP = stat.CurrentMana;
    //    data.Level = Level;
    //    data.YP = YP;
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

    //            data.eliQuestProgressDatas.Add(eliData);
    //        }
    //        Debug.Log($"엘리 퀘스트 저장: {q.Key}");
    //    }

    //    foreach (var pet in ownedPets)
    //    {
    //        data.ownedPetIDs.Add(pet.Data.PetID);
    //    }

    //    foreach (var pet in equippedPets)
    //    {
    //        data.equipPetIDs.Add(pet.Data.PetID);
    //    }
    //    Debug.Log($"보유 펫 저장: {data.ownedPetIDs.Count}마리, 장착 펫 저장: {data.equipPetIDs.Count}마리");

    //    return data;
    //}

    //public void LoadData(PlayerSaveData data)
    //{
    //    SetCurrentHp(data.CurrnetHP);
    //    SetCurrentMana(data.CurrentMP);
    //    Level = data.Level;
    //    yp = data.YP;
    //    CurrentExp = data.CurrentExp;

    //    foreach (var Item in data.Inventory)
    //    {
    //        ItemData itemData = Resources.Load<ItemData>($"ItemDatas/{Item.Key}");
    //        Inventory.AddItem(itemData, Item.Value);
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
    //            quest.GetEliQProgress()[eliData.QuestID].EliCounts [countData.EnemyID] = countData.KillCount;
    //        }
    //    }

    //    foreach (var petID in data.ownedPetIDs)
    //    {
    //        Pet petPrefab = Resources.Load<Pet>($"PetData/{petID}");

    //        if (petPrefab != null)
    //        {
    //            AddPetFromPrefab(petPrefab);
    //        }
    //        else
    //        {
    //            Debug.LogWarning($"펫 프리팹을 찾을 수 없습니다: {petID}");
    //        }
    //        Debug.Log($"펫 보유됨: {petID}");
    //    }

    //    foreach (var petID in data.equipPetIDs)
    //    {
    //        Pet petPrefab = Resources.Load<Pet>($"PetData/{petID}");

    //        if (petPrefab != null)
    //        {
    //            AddPetFromPrefab(petPrefab);
    //            EquipPet(petPrefab);
    //        }
    //        else
    //        {
    //            Debug.LogWarning($"펫 프리팹을 찾을 수 없습니다: {petID}");
    //        }
    //        Debug.Log($"펫 장착됨: {petID}");
    //    }
    //}
}