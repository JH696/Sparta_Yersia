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

    private Rigidbody2D rb;

    [Header("상호작용 센서")]
    [SerializeField] private InteractCensor censor;

    [Header("이동 가능한 타일 레이어")]
    [SerializeField] private LayerMask moveableLayerMask;

    private GameObject interactTextInstance;
    private Transform currentTarget;

    private void OnBattleStarted() => StopPlayer(true);
    private void OnBattleEnded() => StopPlayer(false);

    void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();

        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
        else
        {
            Debug.LogWarning("[PlayerController] Rigidbody2D가 없습니다. 부드러운 이동/보간이 적용되지 않습니다.");
        }
    }

    private void Start()
    {
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
        var wait = new WaitForFixedUpdate();

        while (true)
        {
            if (dir.sqrMagnitude <= 0.01f || !canMove)
                break;

            if (rb != null)
            {
                Vector2 step = dir * moveSpeed * Time.fixedDeltaTime;
                rb.MovePosition(rb.position + step);
            }
            else
            {
                transform.position += new Vector3(dir.x, dir.y, 0) * moveSpeed * Time.deltaTime;
            }

            yield return wait;

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
        var wait = new WaitForFixedUpdate();

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

            if (rb != null)
            {
                Vector2 step = (Vector2)dir * moveSpeed * Time.fixedDeltaTime;
                rb.MovePosition(rb.position + step);
            }
            else
            {
                transform.position += dir * moveSpeed * Time.deltaTime;
            }

            yield return wait;
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

            if (rb != null) rb.velocity = Vector2.zero;
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

        if (censor.GetTarget() != null)
        {
            censor.GetTarget().Interact(this.gameObject);
        }
    }

    private bool canMove = true;
}
