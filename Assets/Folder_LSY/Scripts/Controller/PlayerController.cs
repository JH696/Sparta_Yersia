using UnityEngine;

public class PlayerController : BaseCharacter, ILevelable
{
    [Header("이동 관련")]
    [SerializeField] private float moveSpeed = 5f;

    private Vector3 targetPos;
    private bool isMoving = false;

    private bool isInDialogue = false;

    [Header("상호작용")]
    [SerializeField, Tooltip("상호작용 가능한 최대 거리")] private float interactRange = 2f;
    [SerializeField, Tooltip("상호작용 대상이 될 NPC의 레이어 마스크")] private LayerMask npcLayerMask;

    //레벨, 경험치
    public int Level { get; private set; } = 1;
    public int CurrentExp { get; private set; } = 0;
    public int ExpToNextLevel => 100 * Level;

    [Header("YP(화폐)")]
    [SerializeField] private int yp = 0;
    public int YP => yp;

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

    private void Start()
    {
        Debug.Log($"플레이어 스탯 확인: HP {CurrentHp}/{MaxHp}, MP {CurrentMana} / {MaxMana}, Attack {Attack}, Defense {Defense}, Luck {Luck}, Speed {Speed}");
    }

    private void Update()
    {
        HandleInput();
        HandleMovement();

        // 테스트용: 키 입력 시 데미지 입거나 회복
        if (Input.GetKeyDown(KeyCode.H))  // H 누르면 힐 10
        {
            Heal(10f);
            Debug.Log($"힐 받음: 현재 체력 {CurrentHp}/{MaxHp}");
        }
        if (Input.GetKeyDown(KeyCode.J))  // J 누르면 데미지 20
        {
            TakeDamage(20f);
            Debug.Log($"데미지 입음: 현재 체력 {CurrentHp}/{MaxHp}");
        }

        // 테스트용: E 키 누르면 경험치 30 추가
        if (Input.GetKeyDown(KeyCode.E))
        {
            AddExp(30);
            Debug.Log($"플레이어 경험치: {CurrentExp} / {ExpToNextLevel}, 레벨: {Level}");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log($"플레이어 스탯 확인: HP {CurrentHp}/{MaxHp}, MP {CurrentMana}/{MaxMana}, Attack {Attack}, Defense {Defense}, Luck {Luck}, Speed {Speed}");
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            AddYP(100);
        }

        HandleInteractionInput();

        if (Input.GetKeyDown(KeyCode.X))
        {
            SpendYP(50);
        }
    }

    private void HandleInput()
    {
        // 대화 중이라면 이동 입력 무시
        if (isInDialogue) return;

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
        if (isInDialogue)
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

        Collider2D npcColider = Physics2D.OverlapCircle(transform.position, interactRange, npcLayerMask);
        if (npcColider == null) return;

        npcColider.GetComponent<NPCController>().Interact();
    }

    public void StartDialogue()
    {
        isInDialogue = true;
        isMoving = false;
    }

    // 외부에서 대화 종료 시 호출하여 이동 제한 해제
    public void EndDialogue()
    {
        isInDialogue = false;
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
        Debug.Log($"플레이어 레벨업! 현재 레벨: {Level}");
        Stat.MultiplyStats(1.1f);
    }
}
