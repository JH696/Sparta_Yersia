using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

[System.Serializable]
public class DialogueData
{
    public string NPCName;
    public List<string> Lines;
}

public class Dialogue : MonoBehaviour
{
    [Header("json파일 이름")]
    public string FileName;

    [Header("대사 출력 텍스트")]
    public TextMeshProUGUI Text;

    private DialogueData dialogueData;
    private int currentLineIndex = 0;

    [SerializeField] private QuestManager questManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PassLine();
        }
    }

    // json 파일을 불러오는 메소드
    void LoadDialogueData()
    {
        string path = Path.Combine
            (Application.streamingAssetsPath, "Dialogs", FileName + ".json.txt");

        if (!File.Exists(path))
        {
            Debug.LogError($"파일이 존재하지 않습니다: {path}");
            return;
        }

        string json = File.ReadAllText(path);
        dialogueData = JsonUtility.FromJson<DialogueData>(json);
    }

    // json 파일 속 대사를 출력하는 메소드
    void PassLine()
    {
        if (dialogueData == null)
        {
            LoadDialogueData();
        }
        else if (dialogueData.Lines.Count <= currentLineIndex)
        {
            Debug.Log("대사 고갈.");
            questManager.StartQuest(questManager.GetQuest("Q1001"));
            return;
        }

        Text.text = dialogueData.Lines[currentLineIndex];
        currentLineIndex++;
    }
}