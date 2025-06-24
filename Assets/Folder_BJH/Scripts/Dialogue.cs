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

    void Start()
    {
        LoadDialogueData();
    }

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
        if (dialogueData == null || dialogueData.Lines.Count <= currentLineIndex)
        {
            Debug.Log("다이얼로그가 비었습니다.");
            return;
        }

        Text.text = dialogueData.Lines[currentLineIndex];

        currentLineIndex++;
    }
}