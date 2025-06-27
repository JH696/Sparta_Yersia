using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IInteractable
{
    void Interact();
}

public class NPC : MonoBehaviour, IInteractable
{
    [Header("NPC 데이터")]
    public NPCData NpcData;

    [Header("수락 / 완료 대기 퀘스트 (시각화)")]
    public List<QuestData> AssignQuests;        // 수락 대기
    public List<QuestData> ReceiveQuests;       // 완료 대기

    void Start()
    {
        SetNPCRequest();
    }

    // [외부] : NPC와 상호작용
    public void Interact()
    {
        if (NpcData == null) return;

        SetNPCRequest();

        DialogueManager.Instance.StartDialogue(this);
    }

    // [공용] : NPC의 의뢰 목록을 설정
    public void SetNPCRequest()
    {
        AssignQuests = QuestManager.Instance.GetAvailableQuests().Where(q => q.AssignerID == NpcData.NpcID).ToList();
        ReceiveQuests = QuestManager.Instance.GetInProgressQuests().Where(q => q.ReceiverID == NpcData.NpcID).ToList();
    }
}