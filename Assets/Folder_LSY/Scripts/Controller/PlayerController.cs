using UnityEngine;

public class PlayerController : BaseCharacter, ILevelable
{
    [Header("플레이어 데이터")]
    [SerializeField] private PlayerData playerData;
    public PlayerData PlayerData => playerData; // 읽기 전용

    [Header("이동 관련")]
    [SerializeField] private float moveSpeed = 5f;

    private Vector3 targetPos;
    private bool isMoving = false;

    [Header("상호작용")]
    [SerializeField, Tooltip("상호작용 가능한 최대 거리")] private float interactRange = 2f;
    [SerializeField, Tooltip("상호작용 대상이 될 NPC의 레이어 마스크")] private LayerMask npcLayerMask;

    // 플레이어의 프로필 아이콘을 PlayerData에서 가져옴
    public Sprite ProfileIcon => playerData == null ? null : playerData.GetDefaultProfileIcon();

    // 레벨, 경험치
    public int Level { get; private set; } = 1;
    public int CurrentExp { get; private set; } = 0;
    public int ExpToNextLevel => playerData == null ? 100 * Level : playerData.BaseExpToLevelUp * Level;

    [Header("YP(화폐)")]
    private int yp = 0;
    public int YP => yp;

    private void Awake()
    {
        if (playerData == null) return;

        InitStat(playerData);
        Level = playerData.StartLevel;
        CurrentExp = playerData.StartExp;
        yp = playerData.StartYP;
    }

    private void Update()
    {
        HandleInput();
        HandleMovement();
        HandleInteractionInput();

        // 테스트용 키
        if (Input.GetKeyDown(KeyCode.H)) HealHP(10f);
        if (Input.GetKeyDown(KeyCode.J)) TakeDamage(20f);
        if (Input.GetKeyDown(KeyCode.E)) AddExp(30);
        if (Input.GetKeyDown(KeyCode.Z)) AddYP(100);
        if (Input.GetKeyDown(KeyCode.X)) SpendYP(50);
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log($"플레이어 스탯 확인: HP {CurrentHp}/{MaxHp}, MP {CurrentMana}/{MaxMana}, Attack {Attack}, Defense {Defense}, Luck {Luck}, Speed {Speed}");
        }
    }

    private void HandleInput()
    {
        // 대화 중이라면 이동 입력 무시
        if (DialogueManager.Instance.IsDialogueActive) return;

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            targetPos = Camera.main.ScreenToWorldPoint(mousePos);
            targetPos.z = 0f;
            isMoving = true;
        }
    }

    private void HandleMovement()
    {
        // 대화 중이라면 이동 중지
        if (DialogueManager.Instance.IsDialogueActive)
        {
            isMoving = false;
            return;
        }

        if (!isMoving) return;

        Vector3 direction = (targetPos - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPos);

        if (distance > 0.1f)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        else
        {
            isMoving = false;
        }
    }

    // 플레이어가 상호작용 키(F)를 눌렀을 때 주변 NPC를 감지하고 상호작용을 수행합니다
    private void HandleInteractionInput()
    {
        if (!Input.GetKeyDown(KeyCode.F)) return;

        Collider2D npcCollider = Physics2D.OverlapCircle(transform.position, interactRange, npcLayerMask);
        if (npcCollider == null) return;

        NPC npc = npcCollider.GetComponent<NPC>();
        if (npc == null) return;

        npc.Interact();
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
        Level++;
        Debug.Log($"플레이어 레벨업 현재 레벨: {Level}");

        float multiplier = playerData == null ? 1.1f : playerData.StatMultiplierPerLevel;
        Stat.MultiplyStats(multiplier);
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
        Debug.Log($"[PlayerCharacter] Equip: {item.ItemName}");
        // TODO: 실제 스탯에 반영 (item.GetStatValue 등)
        if (item == null || item.Category != EItemCategory.Equipment) return;

        foreach (var stat in item.ItemStats)
        {
            if (stat.eStat == EStatType.MaxHp)
            {
                Stat.MaxHp += stat.value;
                Stat.CurrentHp = Mathf.Clamp(Stat.CurrentHp, 0, Stat.MaxHp);
            }
            else if (stat.eStat == EStatType.MaxMana)
            {
                Stat.MaxMana += stat.value;
                Stat.CurrentMana = Mathf.Clamp(Stat.CurrentMana, 0, Stat.MaxMana);
            }
            else if (stat.eStat == EStatType.Attack)
            {
                Stat.Attack += stat.value;
            }
            else if (stat.eStat == EStatType.Defense)
            {
                Stat.Defense += stat.value;
            }
            else if (stat.eStat == EStatType.Luck)
            {
                Stat.Luck += stat.value;
            }
            else if (stat.eStat == EStatType.Speed)
            {
                Stat.Speed += stat.value;
            }
        }
    }

    /// <summary>해제했을 때 호출</summary>
    public void Unequip(ItemData item)
    {
        Debug.Log($"[PlayerCharacter] Unequip: {item.ItemName}");
        // TODO: 실제 스탯에서 제거
        if (item == null || item.Category != EItemCategory.Equipment) return;

        foreach (var stat in item.ItemStats)
        {
            if (stat.eStat == EStatType.MaxHp)
            {
                Stat.MaxHp -= stat.value;
                Stat.CurrentHp = Mathf.Clamp(Stat.CurrentHp, 0, Stat.MaxHp);
            }
            else if (stat.eStat == EStatType.MaxMana)
            {
                Stat.MaxMana -= stat.value;
                Stat.CurrentMana = Mathf.Clamp(Stat.CurrentMana, 0, Stat.MaxMana);
            }
            else if (stat.eStat == EStatType.Attack)
            {
                Stat.Attack -= stat.value;
            }
            else if (stat.eStat == EStatType.Defense)
            {
                Stat.Defense -= stat.value;
            }
            else if (stat.eStat == EStatType.Luck)
            {
                Stat.Luck -= stat.value;
            }
            else if (stat.eStat == EStatType.Speed)
            {
                Stat.Speed -= stat.value;
            }
        }
    }

    /// <summary>소모품 사용했을 때 호출</summary>
    public void Use(ItemData item)
    {
        Debug.Log($"[PlayerCharacter] Use: {item.ItemName}");
        // TODO: 소모품 효과 발동
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
        Debug.Log($"[PlayerCharacter] GiveQuestItem: {item.ItemName}");
        // TODO: 퀘스트 시스템에 통지
    }
}