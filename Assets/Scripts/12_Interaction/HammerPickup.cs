using UnityEngine;

public class HammerPickup : MonoBehaviour, IInteractable
{
    [Header("사용할 아이템 ID")]
    [SerializeField] private string hammerItemID = "Hammer";

    private bool canInteract;
    private GameObject player;

    void Start()
    {
        // 트리거 콜라이더 세팅
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.F))
            Interact(player);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        player = other.gameObject;
        canInteract = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        canInteract = false;
        player = null;
    }

    public void Interact(GameObject interactor)
    {
        // 맵에서 망치 오브젝트 제거
        Destroy(gameObject);
    }
}
