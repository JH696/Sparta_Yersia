using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("대사 출력 텍스트")]
    public DialogueUI dialogueUI;

    [Header("Json Handler")]
    public JsonHelper Helper;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartDialogue(NPC npc)
    {
        if (npc == null) return;

        dialogueUI.SetNPCData(npc);
        dialogueUI.ShowDialogueUI();
    }
}