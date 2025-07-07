using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동 관련")]
    [SerializeField] private float moveSpeed = 5f;

    private Vector3 targetPos;
    private bool isMoving = false;

    [Header("상호작용")]
    [SerializeField, Tooltip("상호작용 가능한 최대 거리")] private float interactRange = 2f;
    [SerializeField, Tooltip("상호작용 대상이 될 NPC의 레이어 마스크")] private LayerMask npcLayerMask;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
        player.Init();
    }

    private void Update()
    {
        HandleInput();
        HandleMovement();
        HandleInteractionInput();

        // 테스트용 키
        if (Input.GetKeyDown(KeyCode.H)) player.HealHP(10f);
        if (Input.GetKeyDown(KeyCode.J)) player.TakeDamage(20f);
        if (Input.GetKeyDown(KeyCode.E)) player.AddExp(30);
        if (Input.GetKeyDown(KeyCode.Z)) player.AddYP(100);
        if (Input.GetKeyDown(KeyCode.X)) player.SpendYP(50);
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log($"플레이어 스탯 확인: HP {player.CurrentHp}/{player.MaxHp}, MP {player.CurrentMana}/{player.MaxMana}, Attack {player.Attack}, Defense {player.Defense}, Luck {player.Luck}, Speed {player.Speed}");
        }
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

    private void HandleInteractionInput()
    {
        if (!Input.GetKeyDown(KeyCode.F)) return;

        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRange, npcLayerMask);
        if (hit == null) return;

        IInteractable interactable = hit.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactable.Interact();
        }
    }
}