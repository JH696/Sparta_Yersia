using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueData
{
    public string DialogueID;   
    public List<string> Lines;
}

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("대사 출력 텍스트")]
    public DialogueUI dialogueUI;

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

        LoadJson($"Dialogues/{npc.NpcData.NpcID}");

        dialogueUI.SetDialogueUIData(npc, LoadJson($"Dialogues/{npc.NpcData.NpcID}"));
        dialogueUI.ShowDialogueUI();
    }

    DialogueData LoadJson(string path)
    {
        TextAsset jsonText = Resources.Load<TextAsset>(path);

        if (jsonText == null)
        {
            Debug.LogError("JSON 파일을 찾을 수 없습니다: " + path);
            return null;
        }

        DialogueData data = JsonUtility.FromJson<DialogueData>(jsonText.text);
        return data;
    }
}