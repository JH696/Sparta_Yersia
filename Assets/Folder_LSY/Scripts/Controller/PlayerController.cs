using UnityEngine;

public class PlayerController : BaseCharacter
{
    [SerializeField] private float moveSpeed = 5f;

    private Vector3 targetPos;
    private bool isMoving = false;

    private bool isInDialogue = false;

    [Header("상호작용")]
    [SerializeField, Tooltip("상호작용 가능한 최대 거리")] private float interactRange = 2f;
    [SerializeField, Tooltip("상호작용 대상이 될 NPC의 레이어 마스크")] private LayerMask npcLayerMask;


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

        HandleInteractionInput();
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
}
