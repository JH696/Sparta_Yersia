using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Vector3 targetPos;
    private bool isMoving = false;

    private StatTable statTable = new();

    private void Start()
    {
        InitStats();
    }

    private void Update()
    {
        HandleInput();
        HandleMovement();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(10f);
        }
    }

    // 임시 테스트용 나중에 베이스캐릭터 만들어서 옮길 예정
    private void InitStats()
    {
        statTable.Set(EStatType.MaxHp, 100f);
        statTable.Set(EStatType.MaxMana, 50f);
        statTable.Set(EStatType.Attack, 20f);
        statTable.Set(EStatType.Defense, 10f);
        statTable.Set(EStatType.Luck, 5f);
        statTable.Set(EStatType.Speed, 10f);
    }

    private void HandleInput()
    {
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

    // 임시 테스트용
    private void TakeDamage(float amount)
    {
        float curHp = statTable.Get(EStatType.MaxHp);
        curHp -= amount;
        curHp = Mathf.Max(0, curHp);
        statTable.Set(EStatType.MaxHp, curHp);

        Debug.Log($"[피해] 현재 체력: {curHp}");
    }
}
