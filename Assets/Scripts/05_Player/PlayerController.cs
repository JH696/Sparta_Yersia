using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동 관련")]
    [SerializeField] private float moveSpeed = 5f;

    private Vector3 targetPos;
    private bool clickMoving = false;
    private Vector2 inputDir = Vector2.zero;

    private Animator anim;
    private Player player;
    private Coroutine moveRoutine;

    [Header("상호작용")]
    [SerializeField] private float interactRange = 2f;
    [SerializeField] private LayerMask moveableLayerMask;

    [Header("상호작용 UI")]
    [SerializeField] private GameObject interactTextPrefab;

    private GameObject interactTextInstance;
    private Transform currentTarget;

    private void OnBattleStarted() => StopPlayer(true);
    private void OnBattleEnded() => StopPlayer(false);

    void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
    }

    private void Start()
    {
        if (interactTextPrefab != null)
        {
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

        targetPos = transform.position;

        BattleManager.Instance.OnBattleStarted += OnBattleStarted;
        BattleManager.Instance.OnBattleEnded += OnBattleEnded;

        player.ChangeSprite();
    }

    private void OnDestroy()
    {
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.OnBattleStarted -= OnBattleStarted;
            BattleManager.Instance.OnBattleEnded -= OnBattleEnded;
        }
    }

    private void Update()
    {
        HandleInput();
        UpdateAnimation();
        HandleInteractionInput();
        UpdateInteractText();
    }

    private void HandleInput()
    {
        if (DialogueManager.Instance.IsDialogueActive || !canMove) return;

        Vector2 newInputDir = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) newInputDir.y += 1;
        if (Input.GetKey(KeyCode.S)) newInputDir.y -= 1;
        if (Input.GetKey(KeyCode.A)) newInputDir.x -= 1;
        if (Input.GetKey(KeyCode.D)) newInputDir.x += 1;
        newInputDir.Normalize();

        if (newInputDir.sqrMagnitude > 0.01f)
        {
            inputDir = newInputDir;
            clickMoving = false;
            StartMove(inputDir);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0f, moveableLayerMask);
            if (hit.collider != null)
            {
                targetPos = mouseWorldPos;
                clickMoving = true;
                inputDir = Vector2.zero;
                StartMoveToClick(targetPos);
            }
        }
    }

    private void StartMove(Vector2 direction)
    {
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveRoutine(direction));
    }

    private void StartMoveToClick(Vector3 target)
    {
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveToPositionRoutine(target));
    }

    private IEnumerator MoveRoutine(Vector2 dir)
    {
        while (true)
        {
            if (dir.sqrMagnitude <= 0.01f || !canMove)
                break;

            transform.position += new Vector3(dir.x, dir.y, 0) * moveSpeed * Time.deltaTime;

            yield return null;

            dir = Vector2.zero;
            if (Input.GetKey(KeyCode.W)) dir.y += 1;
            if (Input.GetKey(KeyCode.S)) dir.y -= 1;
            if (Input.GetKey(KeyCode.A)) dir.x -= 1;
            if (Input.GetKey(KeyCode.D)) dir.x += 1;
            dir.Normalize();

            inputDir = dir;
        }

        inputDir = Vector2.zero;
        moveRoutine = null;
    }

    private IEnumerator MoveToPositionRoutine(Vector3 target)
    {
        while (true)
        {
            if (!canMove) break;

            Vector3 diff = target - transform.position;
            float dist = diff.magnitude;
            Vector3 dir = diff.normalized;

            if (dist < 0.1f)
            {
                clickMoving = false;
                break;
            }

            transform.position += dir * moveSpeed * Time.deltaTime;
            yield return null;
        }

        moveRoutine = null;
    }

    private void StopPlayer(bool isActive)
    {
        if (isActive)
        {
            canMove = false;
            clickMoving = false;
            inputDir = Vector2.zero;
            anim.SetBool("Moving", false);

            if (moveRoutine != null)
            {
                StopCoroutine(moveRoutine);
                moveRoutine = null;
            }
        }
        else
        {
            canMove = true;
        }
    }

    private void UpdateAnimation()
    {
        if (anim.runtimeAnimatorController == null) return;

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

    private bool canMove = true;
}
