using System.Collections.Generic;
using System.IO;
using TMPro;
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

    private DialogueData dialogueData;
    private int currentLineIndex = 0;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PassLine();
        }
    }

    // json 파일을 불러오는 메소드
    public void LoadDialogueData(string npcID)
    {
        string path = Path.Combine
            (Application.streamingAssetsPath, "Dialogs", npcID + ".json");

        if (!File.Exists(path))
        {
            Debug.LogError($"파일이 존재하지 않습니다: {path}");
            return;
        }

        string json = File.ReadAllText(path);
        dialogueData = JsonUtility.FromJson<DialogueData>(json);
    }

    // json 파일 속 대사를 출력하는 메소드
    public void PassLine()
    {
        if (dialogueData == null)
        {
            return;
        }
        else if (dialogueData.Lines.Count <= currentLineIndex)
        {
            Debug.Log("대사 고갈.");
            DialogueManager.Instance.dialogueUI.ToggleDialogueBox();
            ResetDialogue();
            return;
        }

        dialogueUI.RefreshText(dialogueData.Lines[currentLineIndex]);
        currentLineIndex++;
    }

    void ResetDialogue()
    {
        currentLineIndex = 0;
        dialogueData = null;
    }
}