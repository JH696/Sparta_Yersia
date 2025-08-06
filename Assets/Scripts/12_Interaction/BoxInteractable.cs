//using UnityEngine;

//public class BoxInteractable : MonoBehaviour, IInteractable
//{
//    [Header("상자 스프라이트 상태")]
//    [SerializeField] private GameObject closedBox;
//    [SerializeField] private GameObject openedBox;

//    [Header("필요한 망치 아이템 ID")]
//    [SerializeField] private string hammerItemID = "l_q50";

//    [Header("대화 JSON")]
//    [Tooltip("Locked JSON")]
//    [SerializeField] private string lockedJsonName = "N_n010";
//    [Tooltip("Unlocked JSON")]
//    [SerializeField] private string unlockedJsonName = "N_n010";

//    [Header("DialogueID (Locked / Unlocked)")]
//    [SerializeField] private string lockedDialogueId = "Locked";
//    [SerializeField] private string unlockedDialogueId = "Unlocked";

//    private bool canInteract;
//    private GameObject player;

//    void Start()
//    {
//        // Collider2D 트리거 설정
//        var col = GetComponent<Collider2D>();
//        col.isTrigger = true;

//        // 초기 상태
//        closedBox?.SetActive(true);
//        openedBox?.SetActive(false);
//    }

//    void Update()
//    {
//        if (canInteract && Input.GetKeyDown(KeyCode.F))
//            Interact(player);
//    }

//    void OnTriggerEnter2D(Collider2D other)
//    {
//        if (!other.CompareTag("Player")) return;
//        player = other.gameObject;
//        canInteract = true;
//    }

//    void OnTriggerExit2D(Collider2D other)
//    {
//        if (!other.CompareTag("Player")) return;
//        canInteract = false;
//        player = null;
//    }

//    public void Interact(GameObject interactor)
//    {
//        var inv = GameManager.player.inventory;

//        // 망치 없으면 Locked 대사
//        if (!inv.HasItem(hammerItemID))
//        {
//            var dlg = LoadDialogue(lockedJsonName, lockedDialogueId);
//            DialogueManager.Instance.StartDialogue(dlg);
//            return;
//        }

//        // 2) 망치 있으면 Unlocked 선택지 대사
//        var unlocked = LoadDialogue(unlockedJsonName, unlockedDialogueId);
        
//        );
//    }

//    // JSON → DialogueData 변환 헬퍼
//    private DialogueData LoadDialogue(string jsonName, string dialogueId)
//    {
//        // Resources/Dialogues/{jsonName}.json 읽기
//        var ta = Resources.Load<TextAsset>($"Dialogues/{jsonName}");
//        if (ta == null)
//        {
//            Debug.LogError($"JSON not found: Dialogues/{jsonName}.json");
//            return default;
//        }
//        // JsonHelper 이용 파싱 (배열 형태)
//        var all = JsonHelper.FromJson<DialogueData>(ta.text);
//        // DialogueID 일치하는 항목 반환
//        var dlg = all.FirstOrDefault(d => d.DialogueID == dialogueId);
//        if (dlg == null)
//            Debug.LogError($"DialogueID '{dialogueId}' not found in {jsonName}.json");
//        return dlg;
//    }
//}