using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterStatus, ILevelable
{
    [Header("플레이어 데이터")]
    [SerializeField] private PlayerData playerData;

    [Header("플레이어 정보")]
    [SerializeField] private PlayerParty party;
    [SerializeField] private PlayerQuest quest;
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private CharacterSkill skill;

    public PlayerParty Party => party;
    public StatData PlayerData => playerData; // 읽기 전용
    public PlayerQuest Quest => quest; // 읽기 전용
    public PlayerInventory Inventory => inventory; // 읽기 전용
    public CharacterSkill Skill => skill; // 읽기 전용

    // 레벨, YP 관련 데이터 인터페이스로 접근
    private ILevelData LevelData => playerData as ILevelData;
    private IYPHolder YPData => playerData as IYPHolder;

    // 성별 (플레이어 전용)
    public EGender Gender { get; private set; } = EGender.Male;

    // 레벨, 경험치
    public int Level { get; private set; } = 1;
    public int CurrentExp { get; private set; } = 0;
    public int ExpToNextLevel => LevelData?.BaseExpToLevelUp * Level ?? 100 * Level;

    [Header("YP(화폐)")]
    private int yp = 0;
    public int YP => yp;

    [Header("펫 관련")]
    [SerializeField] private List<Pet> ownedPets = new List<Pet>();    // 보유한 펫 목록
    [SerializeField] private List<Pet> equippedPets = new List<Pet>(); // 장착한 펫 목록 (최대 2마리)
    public List<Pet> OwnedPets => ownedPets;
    public List<Pet> EquippedPets => equippedPets;

    [Header("파티 관리")]
    [SerializeField] private PlayerParty playerParty;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (playerData == null || LevelData == null) return;

        InitFromData(playerData);
    }

    private void InitFromData(PlayerData data)
    {
        InitStat(data);
        skill.Init(data.startingSkills);
        SetLevel(LevelData.StartLevel);
        SetExp(LevelData.StartExp);
        SetYP(YPData.StartYP);
        Gender = data.gender;
    }

    // 경험치 추가 메서드
    public void AddExp(int amount)
    {
        CurrentExp += amount;
        while (CurrentExp >= ExpToNextLevel)
        {
            CurrentExp -= ExpToNextLevel;
            LevelUp();
        }
    }

    public void LevelUp()
    {
        SetLevel(Level + 1);
        Debug.Log($"플레이어 레벨업 현재 레벨: {Level}");

        float multiplier = LevelData?.StatMultiplierPerLevel ?? 1.1f;
        stat.MultiplyStats(multiplier);
    }

    public void SetLevel(int level)
    {
        Level = Mathf.Max(1, level);
    }

    public void SetExp(int exp)
    {
        CurrentExp = Mathf.Max(0, exp);
    }

    public void SetYP(int amount)
    {
        yp = Mathf.Max(0, amount);
    }

    public void SetCurrentHp(float hp)
    {
        stat.CurrentHp = Mathf.Clamp(hp, 0, stat.MaxHp);
    }

    public void SetCurrentMana(float mana)
    {
        stat.CurrentMana = Mathf.Clamp(mana, 0, stat.MaxMana);
    }

    // YP(돈) 획득 메서드
    public void AddYP(int amount)
    {
        yp += Mathf.Max(0, amount);
    }

    // YP(돈) 소비 메서드
    public bool SpendYP(int amount)
    {
        if (yp >= amount)
        {
            yp -= amount;
            return true;
        }
        return false;
    }

    /// <summary>장착했을 때 호출</summary>
    public void Equip(ItemData item)
    {
        Debug.Log($"[Player] Equip: {item.ItemName}");
        if (item == null || item.Category != EItemCategory.Equipment) return;

        foreach (var itemStat in item.ItemStats)
        {
            stat.ApplyStat(itemStat.eStat, itemStat.value);
        }
    }

    /// <summary>해제했을 때 호출</summary>
    public void Unequip(ItemData item)
    {
        Debug.Log($"[Player] Unequip: {item.ItemName}");
        if (item == null || item.Category != EItemCategory.Equipment) return;

        foreach (var itemStat in item.ItemStats)
        {
            stat.ApplyStat(itemStat.eStat, -itemStat.value);
        }
    }

    /// <summary>소모품 사용했을 때 호출</summary>
    public void Use(ItemData item)
    {
        Debug.Log($"[Player] Use: {item.ItemName}");
        if (item == null || item.Category != EItemCategory.Consumable) return;

        foreach (var stat in item.ItemStats)
        {
            if (stat.eStat == EStatType.MaxHp)
                HealHP(stat.value);
            else if (stat.eStat == EStatType.MaxMana)
                HealMana(stat.value);
        }
    }

    /// <summary>퀘스트용 건네주기 호출</summary>
    public void GiveQuestItem(ItemData item)
    {
        Debug.Log($"[Player] GiveQuestItem: {item.ItemName}");
        // TODO: 퀘스트 시스템에 통지
    }


    public void AddPetFromPrefab(Pet petPrefab)
    {
        if (petPrefab == null) return;

        // 이미 보유 중인지 PetData 기준으로 검사
        //if (ownedPets.Exists(p => p.PetData == petPrefab.PetData)) return;

        Pet newPet = Instantiate(petPrefab);
        newPet.gameObject.SetActive(false); // 보유한 펫은 기본 비활성화
        ownedPets.Add(newPet);
        Debug.Log($"[Player] 펫 지급됨: {newPet.Data.PetName}");
    }

    // 보유 펫 추가
    public void AddPet(Pet pet)
    {
        if (pet == null) return;

        if (ownedPets.Exists(p => p.Data == pet.Data)) return;

        pet.gameObject.SetActive(false);
        ownedPets.Add(pet);
        Debug.Log($"[Player] 펫 지급됨: {pet.Data.PetName}");
    }

    /// <summary>
    /// 펫 장착 (씬 오브젝트 활성화)
    /// </summary>
    public void EquipPet(Pet pet)
    {
        if (pet == null || !ownedPets.Contains(pet)) return;

        if (equippedPets.Contains(pet))
        {
            Debug.Log($"[Player] 이미 장착 중: {pet.Data.PetName}");
            return;
        }

        if (equippedPets.Count >= 2)
        {
            Debug.LogWarning("펫은 최대 2마리까지만 장착할 수 있습니다.");
            return;
        }

        pet.gameObject.SetActive(true);
        equippedPets.Add(pet);
        Debug.Log($"[Player] 펫 장착됨: {pet.Data.PetName}");

        if (playerParty != null)
            playerParty.AddPet(pet.status);
    }

    /// <summary>
    /// 펫 장착 해제 (씬 오브젝트 비활성화)
    /// </summary>
    public void UnequipPet(Pet pet)
    {
        if (pet == null || !equippedPets.Contains(pet)) return;

        equippedPets.Remove(pet);
        pet.gameObject.SetActive(false);
        Debug.Log($"[Player] 펫 해제됨: {pet.Data.PetName}");

        if (playerParty != null)
            playerParty.RemovePet(pet.status);
    }

    // 펫 보유 여부 확인
    public bool HasPet(Pet pet)
    {
        return ownedPets.Contains(pet);
    }

    public PlayerSaveData makeSaveData()
    {
        PlayerSaveData data = new PlayerSaveData();
        data.CurrnetHP = stat.CurrentHp;
        data.CurrentMP = stat.CurrentMana;
        data.Level = Level;
        data.YP = YP;
        data.Inventory = inventory.GetAllItems();

        foreach (var quest in quest.GetMyQStatus())
        {
            data.questStatusDatas.Add(new QuestStatusData
            {
                QuestID = quest.Key,
                IsCompleted = quest.Value.IsCleared
            });
            Debug.Log($"퀘스트 저장: {quest.Key}, 완료 여부: {quest.Value.IsCleared}");
        }

        foreach (var q in quest.GetEliQProgress())
        {
            EliQuestProgressData eliData = new EliQuestProgressData
            {
                QuestID = q.Key
            };

            foreach (var kvp in q.Value.EliCounts)
            {
                eliData.eliCountDatas.Add(new EliCountData
                {
                    EnemyID = kvp.Key,
                    KillCount = kvp.Value
                });

                data.eliQuestProgressDatas.Add(eliData);
            }
            Debug.Log($"엘리 퀘스트 저장: {q.Key}");
        }

        foreach (var pet in ownedPets)
        {
            data.ownedPetIDs.Add(pet.Data.PetID);
        }

        foreach (var pet in equippedPets)
        {
            data.equipPetIDs.Add(pet.Data.PetID);
        }
        Debug.Log($"보유 펫 저장: {data.ownedPetIDs.Count}마리, 장착 펫 저장: {data.equipPetIDs.Count}마리");

        return data;
    }

    public void LoadData(PlayerSaveData data)
    {
        SetCurrentHp(data.CurrnetHP);
        SetCurrentMana(data.CurrentMP);
        Level = data.Level;
        yp = data.YP;
        CurrentExp = data.CurrentExp;

        foreach (var Item in data.Inventory)
        {
            ItemData itemData = Resources.Load<ItemData>($"ItemDatas/{Item.Key}");
            Inventory.AddItem(itemData, Item.Value);
        }

        foreach (var q in data.questStatusDatas)
        {
            QuestData questData = Resources.Load<QuestData>($"QuestDatas/{q.QuestID}");
            
            quest.AddMyQ(questData);

            if (q.IsCompleted)
            {
                quest.GetMyQStatus()[q.QuestID].IsCleared = true;
            }
        }

        foreach (var eliData in data.eliQuestProgressDatas)
        {
            QuestData progress = Resources.Load<QuestData>($"QuestDatas/{eliData.QuestID}");

            quest.AddEliQ(progress);

            foreach (var countData in eliData.eliCountDatas)
            {
                quest.GetEliQProgress()[eliData.QuestID].EliCounts [countData.EnemyID] = countData.KillCount;
            }
        }

        foreach (var petID in data.ownedPetIDs)
        {
            Pet petPrefab = Resources.Load<Pet>($"PetData/{petID}");
            
            if (petPrefab != null)
            {
                AddPetFromPrefab(petPrefab);
            }
            else
            {
                Debug.LogWarning($"펫 프리팹을 찾을 수 없습니다: {petID}");
            }
            Debug.Log($"펫 보유됨: {petID}");
        }

        foreach (var petID in data.equipPetIDs)
        {
            Pet petPrefab = Resources.Load<Pet>($"PetData/{petID}");

            if (petPrefab != null)
            {
                AddPetFromPrefab(petPrefab);
                EquipPet(petPrefab);
            }
            else
            {
                Debug.LogWarning($"펫 프리팹을 찾을 수 없습니다: {petID}");
            }
            Debug.Log($"펫 장착됨: {petID}");
        }
    }
}

//using System.Collections.Generic;
//using UnityEngine;

//public class Player : CharacterStatus, ILevelable
//{
//    [Header("플레이어 데이터")]
//    [SerializeField] private PlayerData playerData;

//    [Header("플레이어 정보")]
//    [SerializeField] private PlayerParty party;
//    [SerializeField] private PlayerQuest quest;
//    [SerializeField] private PlayerInventory inventory;
//    [SerializeField] private CharacterSkill skill;

//    //public override Sprite Icon => playerData.Icon; // 읽기 전용

//    public PlayerParty Party => party;
//    public StatData PlayerData => playerData; // 읽기 전용
//    public PlayerQuest Quest => quest; // 읽기 전용
//    public PlayerInventory Inventory => inventory; // 읽기 전용
//    public CharacterSkill Skill => skill; // 읽기 전용

//    // 레벨, YP 관련 데이터 인터페이스로 접근
//    private ILevelData LevelData => playerData as ILevelData;
//    private IYPHolder YPData => playerData as IYPHolder;

//    // 성별 (플레이어 전용)
//    public EGender Gender { get; private set; } = EGender.Male;

//    // 레벨, 경험치
//    public int Level { get; private set; } = 1;
//    public int CurrentExp { get; private set; } = 0;
//    public int ExpToNextLevel => LevelData?.BaseExpToLevelUp * Level ?? 100 * Level;

//    [Header("YP(화폐)")]
//    private int yp = 0;
//    public int YP => yp;

//    [Header("펫 관련")]
//    [SerializeField] private List<Pet> ownedPets = new List<Pet>();    // 보유한 펫 목록
//    [SerializeField] private List<Pet> equippedPets = new List<Pet>(); // 장착한 펫 목록 (최대 2마리)
//    public List<Pet> OwnedPets => ownedPets;
//    public List<Pet> EquippedPets => equippedPets;

//    [Header("파티 관리")]
//    [SerializeField] private PlayerParty playerParty;

//    private void Awake()
//    {
//        Init();
//    }

//    public void Init()
//    {
//        if (playerData == null || LevelData == null) return;

//        InitStat(playerData);
//        skill.Init(playerData.startingSkills);
//        Level = LevelData.StartLevel;
//        CurrentExp = LevelData.StartExp;
//        yp = YPData.StartYP;

//        Gender = (playerData as PlayerData)?.gender ?? EGender.Male;
//    }

//    // 경험치 추가 메서드
//    public void AddExp(int amount)
//    {
//        CurrentExp += amount;
//        while (CurrentExp >= ExpToNextLevel)
//        {
//            CurrentExp -= ExpToNextLevel;
//            LevelUp();
//        }
//    }

//    public void LevelUp()
//    {
//        Level++;
//        Debug.Log($"플레이어 레벨업 현재 레벨: {Level}");

//        float multiplier = LevelData?.StatMultiplierPerLevel ?? 1.1f;
//        Stat.MultiplyStats(multiplier);
//    }

//    // YP(돈) 획득 메서드
//    public void AddYP(int amount)
//    {
//        yp += Mathf.Max(0, amount);
//    }

//    // YP(돈) 소비 메서드
//    public bool SpendYP(int amount)
//    {
//        if (yp >= amount)
//        {
//            yp -= amount;
//            return true;
//        }
//        return false;
//    }

//    /// <summary>장착했을 때 호출</summary>
//    public void Equip(ItemData item)
//    {
//        Debug.Log($"[Player] Equip: {item.ItemName}");
//        if (item == null || item.Category != EItemCategory.Equipment) return;

//        foreach (var stat in item.ItemStats)
//        {
//            ApplyStat(stat.eStat, stat.value);
//        }
//    }

//    /// <summary>해제했을 때 호출</summary>
//    public void Unequip(ItemData item)
//    {
//        Debug.Log($"[Player] Unequip: {item.ItemName}");
//        if (item == null || item.Category != EItemCategory.Equipment) return;

//        foreach (var stat in item.ItemStats)
//        {
//            ApplyStat(stat.eStat, -stat.value);
//        }
//    }

//    /// <summary>소모품 사용했을 때 호출</summary>
//    public void Use(ItemData item)
//    {
//        Debug.Log($"[Player] Use: {item.ItemName}");
//        if (item == null || item.Category != EItemCategory.Consumable) return;

//        foreach (var stat in item.ItemStats)
//        {
//            if (stat.eStat == EStatType.MaxHp)
//                HealHP(stat.value);
//            else if (stat.eStat == EStatType.MaxMana)
//                HealMana(stat.value);
//        }
//    }

//    /// <summary>퀘스트용 건네주기 호출</summary>
//    public void GiveQuestItem(ItemData item)
//    {
//        Debug.Log($"[Player] GiveQuestItem: {item.ItemName}");
//        // TODO: 퀘스트 시스템에 통지
//    }

//    private void ApplyStat(EStatType type, float value)
//    {
//        switch (type)
//        {
//            case EStatType.MaxHp:
//                Stat.MaxHp += value;
//                Stat.CurrentHp = Mathf.Clamp(Stat.CurrentHp, 0, Stat.MaxHp);
//                break;
//            case EStatType.MaxMana:
//                Stat.MaxMana += value;
//                Stat.CurrentMana = Mathf.Clamp(Stat.CurrentMana, 0, Stat.MaxMana);
//                break;
//            case EStatType.Attack:
//                Stat.Attack += value;
//                break;
//            case EStatType.Defense:
//                Stat.Defense += value;
//                break;
//            case EStatType.Luck:
//                Stat.Luck += value;
//                break;
//            case EStatType.Speed:
//                Stat.Speed += value;
//                break;
//        }
//    }

//    public void AddPetFromPrefab(Pet petPrefab)
//    {
//        if (petPrefab == null) return;

//        // 이미 보유 중인지 PetData 기준으로 검사
//        //if (ownedPets.Exists(p => p.PetData == petPrefab.PetData)) return;

//        Pet newPet = Instantiate(petPrefab);
//        newPet.gameObject.SetActive(false); // 보유한 펫은 기본 비활성화
//        ownedPets.Add(newPet);
//        Debug.Log($"[Player] 펫 지급됨: {newPet.PetData.PetName}");
//    }

//    // 보유 펫 추가
//    public void AddPet(Pet pet)
//    {
//        if (pet == null) return;

//        if (ownedPets.Exists(p => p.PetData == pet.PetData)) return;

//        pet.gameObject.SetActive(false);
//        ownedPets.Add(pet);
//        Debug.Log($"[Player] 펫 지급됨: {pet.PetData.PetName}");
//    }

//    /// <summary>
//    /// 펫 장착 (씬 오브젝트 활성화)
//    /// </summary>
//    public void EquipPet(Pet pet)
//    {
//        if (pet == null || !ownedPets.Contains(pet)) return;

//        if (equippedPets.Contains(pet))
//        {
//            Debug.Log($"[Player] 이미 장착 중: {pet.PetData.PetName}");
//            return;
//        }

//        if (equippedPets.Count >= 2)
//        {
//            Debug.LogWarning("펫은 최대 2마리까지만 장착할 수 있습니다.");
//            return;
//        }

//        pet.gameObject.SetActive(true);
//        equippedPets.Add(pet);
//        Debug.Log($"[Player] 펫 장착됨: {pet.PetData.PetName}");

//        if (playerParty != null)
//            playerParty.AddPet(pet.gameObject);
//    }

//    /// <summary>
//    /// 펫 장착 해제 (씬 오브젝트 비활성화)
//    /// </summary>
//    public void UnequipPet(Pet pet)
//    {
//        if (pet == null || !equippedPets.Contains(pet)) return;

//        equippedPets.Remove(pet);
//        pet.gameObject.SetActive(false);
//        Debug.Log($"[Player] 펫 해제됨: {pet.PetData.PetName}");

//        if (playerParty != null)
//            playerParty.RemoveMember(pet.gameObject);
//    }

//    // 펫 보유 여부 확인
//    public bool HasPet(Pet pet)
//    {
//        return ownedPets.Contains(pet);
//    }

//    public PlayerSaveData makeSaveData()
//    {
//        PlayerSaveData data = new PlayerSaveData();
//        data.CurrnetHP = Stat.CurrentHp;
//        data.CurrentMP = Stat.CurrentMana;
//        data.Level = Level;
//        data.YP = YP;
//        data.Inventory = inventory.GetAllItems();

//        foreach (var quest in quest.GetMyQStatus())
//        {
//            data.questStatusDatas.Add(new QuestStatusData
//            {
//                QuestID = quest.Key,
//                IsCompleted = quest.Value.IsCleared
//            });
//            Debug.Log($"퀘스트 저장: {quest.Key}, 완료 여부: {quest.Value.IsCleared}");
//        }

//        foreach (var q in quest.GetEliQProgress())
//        {
//            EliQuestProgressData eliData = new EliQuestProgressData
//            {
//                QuestID = q.Key
//            };

//            foreach (var kvp in q.Value.EliCounts)
//            {
//                eliData.eliCountDatas.Add(new EliCountData
//                {
//                    EnemyID = kvp.Key,
//                    KillCount = kvp.Value
//                });

//                data.eliQuestProgressDatas.Add(eliData);
//            }
//            Debug.Log($"엘리 퀘스트 저장: {q.Key}");
//        }

//        foreach (var pet in ownedPets)
//        {
//            data.ownedPetIDs.Add(pet.PetData.PetID);
//        }

//        foreach (var pet in equippedPets)
//        {
//            data.equipPetIDs.Add(pet.PetData.PetID);
//        }
//        Debug.Log($"보유 펫 저장: {data.ownedPetIDs.Count}마리, 장착 펫 저장: {data.equipPetIDs.Count}마리");

//        return data;
//    }

//    public void LoadData(PlayerSaveData data)
//    {
//        SetCurrentHp(data.CurrnetHP);
//        SetCurrentMana(data.CurrentMP);
//        Level = data.Level;
//        yp = data.YP;
//        CurrentExp = data.CurrentExp;

//        foreach (var Item in data.Inventory)
//        {
//            ItemData itemData = Resources.Load<ItemData>($"ItemDatas/{Item.Key}");
//            Inventory.AddItem(itemData, Item.Value);
//        }

//        foreach (var q in data.questStatusDatas)
//        {
//            QuestData questData = Resources.Load<QuestData>($"QuestDatas/{q.QuestID}");

//            quest.AddMyQ(questData);

//            if (q.IsCompleted)
//            {
//                quest.GetMyQStatus()[q.QuestID].IsCleared = true;
//            }
//        }

//        foreach (var eliData in data.eliQuestProgressDatas)
//        {
//            QuestData progress = Resources.Load<QuestData>($"QuestDatas/{eliData.QuestID}");

//            quest.AddEliQ(progress);

//            foreach (var countData in eliData.eliCountDatas)
//            {
//                quest.GetEliQProgress()[eliData.QuestID].EliCounts[countData.EnemyID] = countData.KillCount;
//            }
//        }

//        foreach (var petID in data.ownedPetIDs)
//        {
//            Pet petPrefab = Resources.Load<Pet>($"PetData/{petID}");

//            if (petPrefab != null)
//            {
//                AddPetFromPrefab(petPrefab);
//            }
//            else
//            {
//                Debug.LogWarning($"펫 프리팹을 찾을 수 없습니다: {petID}");
//            }
//            Debug.Log($"펫 보유됨: {petID}");
//        }

//        foreach (var petID in data.equipPetIDs)
//        {
//            Pet petPrefab = Resources.Load<Pet>($"PetData/{petID}");

//            if (petPrefab != null)
//            {
//                AddPetFromPrefab(petPrefab);
//                EquipPet(petPrefab);
//            }
//            else
//            {
//                Debug.LogWarning($"펫 프리팹을 찾을 수 없습니다: {petID}");
//            }
//            Debug.Log($"펫 장착됨: {petID}");
//        }
//    }
//}