using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using System.Xml.Linq;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("수락 가능한 퀘스트 목록")]
    [SerializeField] private List<QuestData> GameQuests;

    [Header("현재 진행 중인 퀘스트 목록")]
    [SerializeField] private List<QuestData> AvailableQuests;

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

    public void GetQuest(QuestData questData)
    {
        if (questData == null || !GameQuests.Contains(questData)) return;


        AvailableQuests.Add(questData);
        GameQuests.Remove(questData); 

        Debug.Log($"퀘스트 시작: {questData.QuestName}");
    }

    public QuestData SearchQuestsForNpc(string id)
    {
        foreach (var quest in QuestManager.Instance.GameQuests)
        {
            if (quest.AssignerID == id)
            {
                return quest;
            }
        }

        return null;
    }
}

