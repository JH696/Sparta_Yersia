using UnityEngine;

public class NPC : MonoBehaviour
{
    public NPCData NpcData;

    public void StartChat()
    {
        Debug.Log($"NPC 대화 시작: {NpcData.NpcName}");

        DialogueManager.Instance.dialogueUI.ToggleDialogueBox();
        DialogueManager.Instance.LoadDialogueData(NpcData.NpcID);
        DialogueManager.Instance.PassLine();   
    }
}
