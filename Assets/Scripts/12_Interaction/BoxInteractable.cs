using UnityEngine;

public class BoxInteractable : MonoBehaviour, IInteractable
{
    [Header("상자 스프라이트 상태")]
    [SerializeField] private GameObject closedBox;
    [SerializeField] private GameObject openedBox;

    [Header("필요한 망치 아이템 ID")]
    [SerializeField] private string hammerItemID = "Hammer";

    [Header("대화 데이터 (Locked / Unlocked)")]
    [SerializeField] private DialogueData lockedDialogue;
    [SerializeField] private DialogueData unlockedDialogue;

    [Header("F키 안내 텍스트 (선택)")]
    [SerializeField] private GameObject interactText;

    private bool _canInteract;
    private GameObject _player;

    void Start()
    {
        // 트리거 콜라이더 세팅
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
        // 초기 상태
        closedBox?.SetActive(true);
        openedBox?.SetActive(false);
        interactText?.SetActive(false);
    }

    void Update()
    {
        if (_canInteract && Input.GetKeyDown(KeyCode.F))
            Interact(_player);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        _player = other.gameObject;
        _canInteract = true;
        interactText?.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        _canInteract = false;
        interactText?.SetActive(false);
        _player = null;
    }

    public void Interact(GameObject interactor)
    {

    }
}
