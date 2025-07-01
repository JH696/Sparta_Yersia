using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact();
}

public class NPC : MonoBehaviour, IInteractable
{
    [Header("NPC 데이터")]
    public NPCData NpcData;

    void Start()
    {
        GetNPCRequest();
    }

    // 상호작용
    public void Interact()
    {
        if (NpcData == null) return;

        GetNPCRequest();

        DialogueManager.Instance.StartNPCDialogue(this);
    }

    // NPC의 의뢰 목록 갱신
    public List<QuestData> GetNPCRequest()
    {
        return null;
    }
}