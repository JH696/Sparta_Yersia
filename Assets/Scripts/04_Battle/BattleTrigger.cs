using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class TriggerMonster : MonoBehaviour
{
    [Header("생성 위치")]
    [SerializeField] private E_StageType nowStage;

    [Header("포함된 몬스터")]
    [SerializeField, Tooltip("직접 할당도 가능")] private MonsterData[] monsters = new MonsterData[4];

    [Header("트리거 스프라이트")]
    [SerializeField] private SpriteRenderer triggerSprite;

    [Header("이동 관련")]
    [SerializeField] private bool canMove = false;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float changeDirectionInterval = 3f;
    [SerializeField] private Tilemap groundTilemap;

    [Header("애니메이터")]
    [SerializeField] private Animator animator;

    private Vector2 moveDirection;
    private Rigidbody2D rb;

    private State currentState = State.None;
    private Coroutine moveRoutine;

    public event System.Action OnTrigged;

    private void OnBattleStarted() => StopMoving(true);
    private void OnBattleEnded() => StopMoving(false);

    private enum State
    {
        None,
        Idle,
        Moving
    }

    public void SetTriggerMonster(MonsterData[] monsters, E_StageType nowStage)
    {
        this.monsters = monsters;
        this.nowStage = nowStage;

        BattleManager.Instance.OnBattleStarted += OnBattleStarted;
        BattleManager.Instance.OnBattleEnded += OnBattleEnded;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundTilemap = GetComponentInParent<Tilemap>();

        if (canMove)
        {
            ChangeState(State.Idle);
        }
    }

    private void ChangeState(State newState)
    {
        if (currentState == newState || !canMove) return;

        currentState = newState;

        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        switch (newState)
        {
            case State.Idle:
                animator.SetBool("IsMoving", false);
                moveRoutine = StartCoroutine(IdleCoroutine());
                break;

            case State.Moving:
                animator.SetBool("IsMoving", true);
                moveRoutine = StartCoroutine(MoveCoroutine());
                break;
        }
    }

    private IEnumerator IdleCoroutine()
    {
        yield return new WaitForSeconds(1f);
        ChangeState(State.Moving);
    }

    private IEnumerator MoveCoroutine()
    {
        PickRandomDirection();

        float interval = Random.Range(1f, changeDirectionInterval);

        float elapsed = 0f;

        while (elapsed < interval)
        {
            if (!canMove)
            {
                yield break;
            }

            Vector2 nextPos = (Vector2)transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
            Vector3Int cell = groundTilemap.WorldToCell(nextPos);
            cell.z = 0;

            if (groundTilemap.HasTile(cell))
            {
                rb.MovePosition(nextPos);
            }
            else
            {
                PickRandomDirection();
            }

            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        ChangeState(State.Idle);
    }

    private void PickRandomDirection()
    {
        Vector2[] directions = new Vector2[]
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        moveDirection = directions[Random.Range(0, directions.Length)];

        if (moveDirection == Vector2.up)
            animator.SetFloat("Move", 4);
        else if (moveDirection == Vector2.down)
            animator.SetFloat("Move", 1);
        else if (moveDirection == Vector2.left)
            animator.SetFloat("Move", 3);
        else if (moveDirection == Vector2.right)
            animator.SetFloat("Move", 2);
    }

    private void StopMoving(bool isActive)
    {
        if (isActive == true)
        {
            if (moveRoutine != null)
            {
                StopCoroutine(MoveCoroutine());
            }

            canMove = false;

            currentState = State.None;
            animator.SetBool("IsMoving",false);
        }
        else
        {
            canMove = true;

            ChangeState(State.Moving);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (monsters == null) return;

        if (other.gameObject.CompareTag("Player"))
        {
            OnTrigged?.Invoke();
            BattleEncounter encounter = new BattleEncounter(monsters, nowStage);
            StartCoroutine(BattleManager.Instance.StartBattle(encounter));
            Destroy(gameObject);
        }
        else
        {
            PickRandomDirection();
        }
    }

    private void OnDestroy()
    {
        BattleManager.Instance.OnBattleStarted -= OnBattleStarted;
        BattleManager.Instance.OnBattleEnded -= OnBattleEnded;
    }
}