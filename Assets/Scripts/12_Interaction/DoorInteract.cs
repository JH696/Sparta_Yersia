using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapCollider2D))]
public class DoorInteract : MonoBehaviour, IInteractable
{
    [Header("Door States")]
    [SerializeField] private GameObject lockedDoor;
    [SerializeField] private GameObject closedDoor;
    [SerializeField] private GameObject openDoor;

    [Header("Key & Unlock Settings")]
    [Tooltip("키 아이템 ID")]
    [SerializeField] private string keyItemID;

    [Header("언락해야 하는 문 개수")]
    [SerializeField] private int doorsToUnlock = 3;

    [Header("Scene Load")]
    [SerializeField] private string battleSceneName = "BattleScene";
    // 한 번 언락된 문을 기억 (세션 동안 유지)
    private static HashSet<int> unlockedDoors = new HashSet<int>();
    private int doorID;
    private int unlockCount => unlockedDoors.Count;

    // 런타임 상태
    private bool canInteract = false;
    private GameObject player;

    void Awake()
    {
        // 개별 문을 구분하기 위한 ID
        doorID = GetInstanceID();
    }

    void Start()
    {
        // 이미 언락된 문은 locked→closed 상태로
        bool wasUnlocked = unlockedDoors.Contains(doorID);
        lockedDoor?.SetActive(!wasUnlocked);
        closedDoor?.SetActive(wasUnlocked);
        openDoor?.SetActive(false);

        // Collider2D(isTrigger)를 붙여 주세요
        var col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
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
        // 잠긴 문 -> 키가 있으면 언락(locked→closed)
        if (lockedDoor != null && lockedDoor.activeSelf)
        {
            if (KeyManager.HasKey())
            {
                lockedDoor.SetActive(false);
                closedDoor.SetActive(true);
                unlockedDoors.Add(doorID);

                // 3개 문 모두 언락하면 키 소멸
                if (unlockCount >= doorsToUnlock)
                    KeyManager.ConsumeKey();
            }
            else
            {
                Debug.Log("열쇠가 필요합니다");
            }
            return;
        }

        // 닫힌 문 -> 열림 표시 후 씬 이동
        if (closedDoor != null && closedDoor.activeSelf)
        {
            closedDoor.SetActive(false);
            openDoor?.SetActive(true);

            // 곧바로 전투 씬으로 이동
            if (!string.IsNullOrEmpty(battleSceneName))
                SceneManager.LoadScene(battleSceneName);
        }
    }
}