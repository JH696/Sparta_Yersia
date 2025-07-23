using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("플레이어 상태")]
    private PlayerStatus status;

    [Header("플레이어 데이터")]
    [SerializeField] private PlayerData playerData;

    [Header("월드에서 보여질 스프라이트")]
    public SpriteRenderer worldSprite;

    public PlayerStatus Status => status; // 읽기 전용
    public PlayerData PlayerData => playerData; // 읽기 전용

    private void Awake()
    {
        if (GameManager.player == null)
        {
            status = new PlayerStatus(playerData, "Player");
            Debug.Log("[Player] PlayerStatus가 새로 초기화되었습니다.");

            GameManager.player = status; // 게임 매니저에 플레이어 상태 설정
        }
        else
        {
            status = GameManager.player; // 기존 플레이어 상태를 가져옴
            Debug.Log("[Player] 기존 PlayerStatus를 사용합니다.");
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