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
    [SerializeField, Tooltip("이동이 가능한 위치 레이어")] private LayerMask moveableLayerMask;
    [Header("테스트용 펫 프리팹")]
    [SerializeField] private Pet testPetPrefab;

    [Header("UI")]
    [SerializeField] private GameObject petUI; // 펫 UI 패널 오브젝트

    private Player player;

    private void Start()
    {
        player = GameManager.Instance.Player.GetComponent<Player>();
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

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (testPetPrefab != null)
            {
                Debug.Log(player);

                // 프리팹을 Player의 AddPetFromPrefab 메서드에 넘겨서 처리
                player.AddPetFromPrefab(testPetPrefab);
            }
        }

        // 펫 UI 열기
        if (Input.GetKeyDown(KeyCode.U) && petUI != null)
        {
            if (!petUI.activeSelf)
                petUI.SetActive(true);
        }
    }

    private void HandleInput()
    {
        if (DialogueManager.Instance.IsDialogueActive) return;

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            // 지정된 레이어에 대해서만 Raycast
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0f, moveableLayerMask);
            if (hit.collider != null)
            {
                targetPos = mouseWorldPos;
                isMoving = true;
            }
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