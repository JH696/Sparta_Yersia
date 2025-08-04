using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동 관련")]
    [SerializeField] private float moveSpeed = 5f;

    // 우클릭 이동
    private Vector3 targetPos;
    private bool clickMoving = false;

    // WASD 키 이동
    private Vector2 inputDir = Vector2.zero;
    private Animator anim;

    private Player player;

    [Header("상호작용")]
    [SerializeField, Tooltip("상호작용 가능한 최대 거리")] private float interactRange = 2f;
    [SerializeField, Tooltip("이동이 가능한 위치 레이어")] private LayerMask moveableLayerMask;

    [Header("상호작용 UI")]
    [SerializeField] private GameObject interactTextPrefab;

    private GameObject interactTextInstance;
    private Transform currentTarget;

    void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
    }

    private void Start()
    {
        if (interactTextPrefab != null)
        {
            // UI용 Canvas가 씬에 반드시 존재해야 함 (Screen Space - Overlay 타입)
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                interactTextInstance = Instantiate(interactTextPrefab, canvas.transform);
                interactTextInstance.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Canvas를 찾을 수 없습니다. UI가 정상적으로 표시되지 않을 수 있습니다.");
            }
        }
        else
        {
            Debug.LogWarning("InteractTextPrefab이 할당되지 않았습니다.");
        }

        // 초기 타겟은 현재 위치
        targetPos = transform.position;

        player.ChangeSprite();
    }

    private void LateUpdate()
    {
        HandleInput();
        HandleMovement();
        UpdateAnimation();
        HandleInteractionInput();
        UpdateInteractText();
    }

    private void HandleInput()
    {
        if (DialogueManager.Instance.IsDialogueActive || BattleManager.Instance.IsBattleActive) return;

        // wasd
        inputDir.x = 0;
        inputDir.y = 0;
        if (Input.GetKey(KeyCode.W)) inputDir.y += 1;
        if (Input.GetKey(KeyCode.S)) inputDir.y -= 1;
        if (Input.GetKey(KeyCode.A)) inputDir.x -= 1;
        if (Input.GetKey(KeyCode.D)) inputDir.x += 1;
        inputDir.Normalize();

        // 키보드로 움직이면 클릭 이동 취소
        if (inputDir.sqrMagnitude > 0.01f)
        {
            clickMoving = false;
        }

        // 우클릭 이동
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0f, moveableLayerMask);
            if (hit.collider != null)
            {
                targetPos = mouseWorldPos;
                clickMoving = true;
            }
        }
    }

    private void HandleMovement()
    {
        // wasd
        if (inputDir.sqrMagnitude > 0.01f)
        {
            Vector3 delta = new Vector3(inputDir.x, inputDir.y, 0)
                          * (moveSpeed * Time.deltaTime);
            transform.position += delta;
            return;
        }

        // 우클릭 이동
        if (clickMoving)
        {
            Vector3 diff = targetPos - transform.position;
            float   distance = diff.magnitude;
            Vector3 direction = diff.normalized;

            if (distance > 0.1f)
            {
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
            else
            {
                clickMoving = false;
            }
        }
    }

    private void UpdateAnimation()
    {
        // 이동 방향
        Vector2 moveVec = Vector2.zero;
        if (inputDir.sqrMagnitude > 0.01f)
        {
            moveVec = inputDir;
        }
        else if (clickMoving)
        {
            Vector2 direction = (Vector2)targetPos - (Vector2)transform.position;
            if (direction.magnitude > 0.01f)
            {
                moveVec = direction.normalized;
            }
        }

        bool moving = moveVec.sqrMagnitude > 0.01f;
        anim.SetBool("Moving", moving);

        if (moving)
        {
            anim.SetFloat("DirX", moveVec.x);
            anim.SetFloat("DirY", moveVec.y);
        }
    }

    private void HandleInteractionInput()
    {
        if (!Input.GetKeyDown(KeyCode.F)) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange);

        Collider2D closestNpc = null;
        float closestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("NPC")) continue;

            float distance = Vector2.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestNpc = hit;
            }
        }

        if (closestNpc == null)
        {
            Debug.Log("상호작용 가능한 NPC가 없습니다.");
            return;
        }

        IInteractable interactable = closestNpc.GetComponent<IInteractable>();
        if (interactable != null)
        {
            Debug.Log($"NPC 상호작용 시도: {closestNpc.name}");
            interactable.Interact(this.gameObject);
        }
        else
        {
            Debug.Log($"NPC 태그는 있지만 IInteractable이 없음: {closestNpc.name}");
        }
    }

    private void UpdateInteractText()
    {
        if (interactTextInstance == null) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange);

        Transform closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("NPC")) continue;

            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestTarget = hit.transform;
            }
        }

        currentTarget = closestTarget;

        if (currentTarget != null)
        {
            Vector3 worldPos = currentTarget.position + Vector3.up * 1.5f;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            interactTextInstance.transform.position = screenPos;
            interactTextInstance.SetActive(true);
        }
        else
        {
            interactTextInstance.SetActive(false);
        }
    }

    //private bool IsScene(string name)
    //{
    //    for (int i = 0; i < SceneManager.sceneCount; i++)
    //    {
    //        Scene scene = SceneManager.GetSceneAt(i);
    //        if (scene.name == name)
    //        {
    //            Debug.Log($"현재 씬: {scene.name}");
    //            return true;
    //        }
    //    }
    //    return false;
    //}
}