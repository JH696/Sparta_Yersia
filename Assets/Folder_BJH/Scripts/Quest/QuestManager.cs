using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("수락 가능한 퀘스트 목록")]
    [SerializeField] private List<QuestData> AvailableQuests;
    [Header("진행 중인 퀘스트 목록")]
    [SerializeField] private List<QuestData> InProgressQuests; 
    [Header("완료 가능한 퀘스트 목록")]
    [SerializeField] public List<QuestData> ClearQuests; 

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
        if (questData == null || !AvailableQuests.Contains(questData)) return;


        InProgressQuests.Add(questData);
        AvailableQuests.Remove(questData); 

        Debug.Log($"퀘스트 시작: {questData.QuestName}");
    }

    public List<QuestData> GetAvailableQuests()
    {
        return AvailableQuests;
    }

    public List<QuestData> GetInProgressQuests()
    {
        return InProgressQuests;
    }

    public void GetQuestReward(QuestData questData)
    {
        if (questData == null) return;

        ClearQuests.Remove(questData);
        Debug.Log($"퀘스트 완료: {questData.QuestName}");
    }
}

