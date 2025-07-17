using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("플레이어 상태")]
    [SerializeField] private PlayerStatus status;

    [Header("플레이어 데이터")]
    [SerializeField] private PlayerData playerData;

    [Header("월드에서 보여질 스프라이트")]
    public SpriteRenderer worldSprite;

    // 레벨, YP 관련 데이터 인터페이스로 접근
    //private ILevelData LevelData => playerData as ILevelData;
    //private IYPHolder YPData => playerData as IYPHolder;

    // 성별 (플레이어 전용)
    //public EGender Gender { get; private set; } = EGender.Male;

    // 레벨, 경험치
    //public int Level { get; private set; } = 1;
    //public int CurrentExp { get; private set; } = 0;
    //public int ExpToNextLevel => LevelData?.BaseExpToLevelUp * Level ?? 100 * Level;

    //[Header("YP(화폐)")]
    //private int yp = 0;
    //public int YP => yp;

    // @@ 플레이어 파티에서 관리 예정 @@
    //[Header("펫 관련")]
    //[SerializeField] private List<Pet> ownedPets = new List<Pet>();    // 보유한 펫 목록
    //[SerializeField] private List<Pet> equippedPets = new List<Pet>(); // 장착한 펫 목록 (최대 2마리)
    //public List<Pet> OwnedPets => ownedPets;
    //public List<Pet> EquippedPets => equippedPets;

    public PlayerStatus Status => status; // 읽기 전용

    private void Start()
    {
        status = new PlayerStatus(playerData,"Player"); // 플레이어 데이터와 이름 설정

        ChangeSprite();
    }

    private void ChangeSprite()
    {
        if (status == null) return;
        worldSprite.sprite = status.PlayerData.WSprite;
    }

    //public void SetLevel(int level)
    //{
    //    Level = Mathf.Max(1, level);
    //}

    //public void SetExp(int exp)
    //{
    //    CurrentExp = Mathf.Max(0, exp);
    //}

    //public void SetYP(int amount)
    //{
    //    yp = Mathf.Max(0, amount);
    //}

    //public void SetCurrentHp(float hp)
    //{
    //    stat.CurrentHp = Mathf.Clamp(hp, 0, stat.MaxHp);
    //}

    //public void SetCurrentMana(float mana)
    //{
    //    stat.CurrentMana = Mathf.Clamp(mana, 0, stat.MaxMana);
    //}

    //// YP(돈) 획득 메서드
    //public void AddYP(int amount)
    //{
    //    yp += Mathf.Max(0, amount);
    //}

    //// YP(돈) 소비 메서드
    //public bool SpendYP(int amount)
    //{
    //    if (yp >= amount)
    //    {
    //        yp -= amount;
    //        return true;
    //    }
    //    return false;
    //}


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