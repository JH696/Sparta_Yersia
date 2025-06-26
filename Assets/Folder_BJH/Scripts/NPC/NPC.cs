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

    public List<QuestData> RequestList;

    public void Interact()
    {
        if (NpcData == null) return;

        RequestList.Clear();

        RequestList.Add(QuestManager.Instance.SearchQuestsForNpc(NpcData.NpcID));
        DialogueManager.Instance.StartDialogue(this);
    }
}