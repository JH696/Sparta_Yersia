using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueData
{
    public string QuestID;
    public List<string> Lines;
}

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("대사 출력 텍스트")]
    public DialogueUI dialogueUI;

    private DialogueData dialogueData;
    private int currentLineIndex;

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

    public void StartDialogue(TextAsset dialogue, NPCData npcData)
    {
        if (dialogue == null) return;

        DialogueData dialogueData = JsonUtility.FromJson<DialogueData>(dialogue.text);

        this.dialogueData = dialogueData;
        currentLineIndex = 0;
        dialogueUI.SetSprite(npcData.DialogueSprite);
        dialogueUI.ShowDialogueBox();
        NextLine();
    }

    public void NextLine()
    {
        if (currentLineIndex >= dialogueData.Lines.Count)
        {
            EndDialogue();
            return;
        }

        string line = dialogueData.Lines[currentLineIndex];
        dialogueUI.SetTypingText(line);
        currentLineIndex++;
    }

    void EndDialogue()
    {
        dialogueUI.ResetDialogueUI();
        dialogueData = null;
        currentLineIndex = 0;
        Debug.Log("대화 종료");
    } 

}