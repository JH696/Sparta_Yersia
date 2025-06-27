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

    [Header("의뢰 진행 중인 퀘스트 (시각화)")]
    public List<QuestData> RequestList;

    void Start()
    {
        SetNPCRequest();
    }

    // [외부] : NPC와 상호작용
    public void Interact()
    {
        if (NpcData == null) return;

        DialogueManager.Instance.StartDialogue(this);
    }

    // [공용] : NPC의 의뢰 목록을 설정
    public void SetNPCRequest()
    {
        QuestData[] allQuests = Resources.LoadAll<QuestData>("QuestDatas");

        RequestList.Clear();
        RequestList = allQuests.Where(q => q.AssignerID == NpcData.NpcID).ToList();
    }
}