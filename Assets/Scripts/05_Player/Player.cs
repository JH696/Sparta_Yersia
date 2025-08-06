using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("플레이어 상태")]
    [SerializeField] private PlayerStatus status;

    [Header("플레이어 데이터")]
    [SerializeField] private PlayerData playerData;

    [Header("월드에서 보여질 스프라이트")]
    [SerializeField] private SpriteRenderer worldSprite;

    [Header("애니메이터")]
    [SerializeField] private Animator animator;

    [Header("테스트")]
    [SerializeField] private PetData testPetData;
    [SerializeField] private SkillData testSkillData;

    public PlayerStatus Status => status;
    public PlayerParty Party => status?.party;
    public PlayerData PlayerData
    {
        get => playerData;
        private set => playerData = value;
    }

    private void Awake()
    {
        if (worldSprite == null)
            worldSprite = GetComponentInChildren<SpriteRenderer>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
        if (playerData == null)
            Debug.LogError("PlayerData가 할당되지 않았습니다!");

        if (GameManager.player == null)
        {
            status = new PlayerStatus(playerData, "Player");
            Debug.Log("[Player] PlayerStatus가 새로 초기화되었습니다.");

            GameManager.player = status;
        }
        else
        {
            status = GameManager.player;
            Debug.Log("[Player] 기존 PlayerStatus를 사용합니다.");
        }

        status.party.Initialize(transform); // playerTransform 전달
    }

    private void Start()
    {
        ChangeSprite();

        status.skills.AddSkill(testSkillData);
        status.skills.EquipSkill(status.skills.GetSkillStatus(testSkillData));
    }

    private void Update()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        if (worldSprite == null)
            worldSprite = GetComponent<SpriteRenderer>();

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (testPetData == null)
            {
                Debug.LogWarning("testPetData가 비어 있습니다. 인스펙터에 PetData를 할당해주세요.");
                return;
            }

            // 테스트용 펫 생성 및 추가
            PetStatus newPet = new PetStatus(testPetData);
            if (status != null)
            {
                status.party.AddPet(newPet);
                Debug.Log($"[Player] 테스트용 펫 추가됨: {testPetData.PetName}");
            }
        }
    }

    public void ChangeSprite()
    {
        if (animator == null) return;

        bool isExpert = status.Rank == E_Rank.Expert;

        // 월드 스프라이트
        worldSprite.sprite = isExpert
            ? playerData.darkWorldSprite
            : playerData.brownWorldSprite;

        // 애니메이터 컨트롤러
        var ctrl = isExpert
            ? playerData.darkController
            : playerData.brownController;
        if (ctrl != null)
            animator.runtimeAnimatorController = ctrl;
        else
            Debug.LogWarning($"{(isExpert ? "dark" : "brown")}Controller가 비어 있습니다.");

        // 프로필 아이콘
        UIManager.Instance.SetProfileIcon(
            isExpert
                ? playerData.darkProfileIcon
                : playerData.brownProfileIcon
        );
    }

    /// <summary>
    /// 플레이어 이름 설정
    /// </summary>
    public void SetPlayerName(string name)
    {
        Status?.SetPlayerName(name);
    }

    public void SetPlayerData(PlayerData newData)
    {
        PlayerData = newData;
        status.SetPlayerData(newData);
        ChangeSprite();
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