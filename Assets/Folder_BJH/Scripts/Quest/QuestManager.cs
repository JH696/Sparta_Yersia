using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("현재 진행 중인 퀘스트 목록")]
    public List<QuestData> CurQuests;

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

    public QuestData GetQuest(string id)
    {
        return CurQuests.Find(quest => quest.TargetID == id);
    }

    public void QuestClear(QuestData questData)
    {
        foreach (var quest in CurQuests)
        {
            if (quest == questData)
            {
                quest.Complete();
                CurQuests.Remove(quest);
                break;
            }
        }
    } 
}

