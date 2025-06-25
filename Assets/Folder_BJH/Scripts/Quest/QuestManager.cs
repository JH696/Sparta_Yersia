using UnityEngine;
using System.IO;
using System.Collections.Generic;

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
    }
}

