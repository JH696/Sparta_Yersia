using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPC : BaseCharacter, IInteractable
{
    [Header("NPC 데이터")]
    [SerializeField] private NPCData npcData;

    [Header("퀘스트 아이콘")]
    [SerializeField] private GameObject questIcon;

    [Header("수락 가능 퀘스트")]
    [SerializeField] private List<QuestData> AssignerQuests;

    [Header("진행 중인 퀘스트")]
    [SerializeField] private List<QuestData> ReceiverQuests;


    public void OnEnable()
    {
        QuestManager.Instance.QuestUpdate += UpdateRequests;
    }
    void OnDisable()
    {
        QuestManager.Instance.QuestUpdate -= UpdateRequests;
    }

    public void Start()
    {
        UpdateRequests();
    }

    // 상호작용
    public void Interact()
    {
        if (npcData == null) return;

        DialogueManager.Instance.StartNPCDialogue(this);
    }

    // NPC 데이터 반환
    public NPCData GetNpcData()
    {
        return npcData;
    }

    public List<QuestData> GetAssignerQuests()
    {
        return AssignerQuests;
    }

    public List<QuestData> GetReceiverQuests()
    {
        return ReceiverQuests;
    }

    // NPC 퀘스트 리스트 업데이트
    private void UpdateRequests()
    {
        var availableQuests = QuestManager.Instance.GetAvailableQuests();

        var myQuests = GameManager.Instance.Player
            .GetComponent<PlayerQuest>()
            .GetMyQStatus()
            .Values
            .Select(status => status.QuestData)
            .ToList();

        // 이 NPC가 주는 퀘스트
        AssignerQuests = availableQuests
            .Where(q => q.AssignerID == npcData.NpcID)
            .ToList();

        // 이 NPC가 받는 퀘스트
        ReceiverQuests = myQuests
            .Where(q => q.ReceiverID == npcData.NpcID)
            .ToList();

        if (AssignerQuests.Count > 0 || ReceiverQuests.Count > 0)
        {
            ShowIcon();
        }
        else
        {
            HideIcon();
        }
    }

    private void ShowIcon()
    {
        questIcon.SetActive(true);
    }

    private void HideIcon()
    {
        questIcon.SetActive(false);
    }
}