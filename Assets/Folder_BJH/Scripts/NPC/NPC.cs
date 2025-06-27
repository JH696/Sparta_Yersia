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

    public List<QuestData> RequestList;

    void Start()
    {
        SetNPCRequest();
    }

    public void Interact()
    {
        if (NpcData == null) return;

        DialogueManager.Instance.StartDialogue(this);
    }
    public void SetNPCRequest()
    {
        QuestData[] allQuests = Resources.LoadAll<QuestData>("QuestDatas");

        RequestList.Clear();
        RequestList = allQuests.Where(q => q.AssignerID == NpcData.NpcID).ToList();
    }
}