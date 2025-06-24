using UnityEngine;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class QuestData
{
    public string QuestID;              // 퀘스트 ID
    public string QuestName;            // 퀘스트 이름
    public string Description;          // 퀘스트 설명 
    public List<string> RewardItems;    // 보상 아이템 목록
}

public class QuestManager : MonoBehaviour
{
    public QuestData GetQuest(string questID)
    {
        string path = Path.Combine
            (Application.streamingAssetsPath, "Quests", questID + ".json.txt");

        if (!File.Exists(path))
        {
            Debug.Log($"{path}는 존재하지 않습니다.");
            return null;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<QuestData>(json);
    }

    public void StartQuest(QuestData questData)
    {
        if (questData == null) return;

        Debug.Log($"퀘스트 시작: {questData.QuestName}");
    }
}

