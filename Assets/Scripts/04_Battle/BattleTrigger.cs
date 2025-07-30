using UnityEngine;
using UnityEngine.Tilemaps;

public class TriggerMonster : MonoBehaviour
{
    [Header("포함된 몬스터")]
    [SerializeField] private BattleEncounter battleEncounter;

    [Header("트리거 스프라이트")]
    [SerializeField] private SpriteRenderer triggerSprite;

    [Header("이동 관련")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float changeDirectionInterval = 2f;
    [SerializeField] private Tilemap groundTilemap;

    private Vector2 moveDirection;
    private float moveTimer;

    private Rigidbody2D rb;

    public event System.Action<TriggerMonster> OnDestroyed;


    public void SetTriggerMonster(BattleEncounter encounter)
    {
        battleEncounter = encounter;
    }

    private void Start()
    {
        groundTilemap = GetComponentInParent<Tilemap>();
        rb = GetComponent<Rigidbody2D>();

        PickRandomDirection();
    }

    private void FixedUpdate()
    {
        if (BattleManager.Instance.IsBattleActive) return;
        moveTimer -= Time.fixedDeltaTime;
        if (moveTimer <= 0f)
        {
            PickRandomDirection();
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
    }

    private void PickRandomDirection()
    {
        moveTimer = changeDirectionInterval;

        Vector2[] directions = new Vector2[]
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        moveDirection = directions[Random.Range(0, directions.Length)];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || BattleManager.Instance.IsBattleActive) return;

        Debug.Log("충돌");
        BattleManager.Instance.StartBattle(battleEncounter);

        OnDestroyed?.Invoke(this); // 트리거 제거 이벤트 호출
    }
}