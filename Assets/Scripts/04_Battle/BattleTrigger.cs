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

    public void SetTriggerMonster(BattleEncounter encounter)
    {
        battleEncounter = encounter;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.freezeRotation = true;
        }

        PickRandomDirection();

        groundTilemap = GetComponentInParent<Tilemap>();    
    }

    private void Update()
    {
        if (BattleManager.Instance.IsBattleActive) return;

        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0f)
        {
            PickRandomDirection();
        }

        Vector2 nextPos = (Vector2)transform.position + moveDirection * moveSpeed * Time.deltaTime;

        // 이동 예정 위치가 타일맵 안에 있는지 확인
        Vector3Int cell = groundTilemap.WorldToCell(nextPos);
        if (groundTilemap.HasTile(cell))
        {
            rb.MovePosition(nextPos);
        }
        else
        {
            PickRandomDirection(); // 벗어날 경우 방향 변경
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
        if (!other.CompareTag("Player")) return;

        Debug.Log("충돌");
        BattleManager.Instance.StartBattle(battleEncounter);

        Destroy(gameObject); // 충돌 후 트리거 제거
    }
}