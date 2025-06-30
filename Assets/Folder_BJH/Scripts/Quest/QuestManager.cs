using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("퀘스트 UI")]
    [SerializeField] public QuestUI questUI;

    [Header("현재 스토리 진행 단계")]
    [SerializeField] private int stortStage = 001;

    [Header("수락 가능한 퀘스트 목록")]
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

        NextStoryQuestUnlock();
    }

    // 퀘스트 획득
    public void GetQuest(QuestData questData)
    {
        if (questData == null || !AvailableQuests.Contains(questData)) return;

        AvailableQuests.Remove(questData);
        TestPlayer.Instance.playerQuest.AddQuest(questData);
        questUI.RefreshQuestUI();
    }

    public List<QuestData> GetAvailableQuests()
    {
        return AvailableQuests;
    }

    public void CostConditionItem(QuestData questData)
    {
        foreach (var item in questData.TargetItem)
        {
            ItemData targetItem = TestPlayer.Instance.playerQuest.FindItemByID(item.ItemID);

            TestPlayer.Instance.playerQuest.RemoveQuestItem(targetItem, item.ItemCount);
        }
    }

    // 퀘스트 완료 
    public void QuestClear(QuestData questData)
    {
        if (questData == null) return;

        if (questData.QuestType == EQuestType.Story)
        {
            NextStoryQuestUnlock();
        }

        TestPlayer.Instance.playerQuest.RemoveQuest(questData);
        GetRawards(questData);
        questUI.RefreshQuestUI();

        Debug.Log($"퀘스트 완료: {questData.QuestName}");
    }

    private void GetRawards(QuestData questData)
    {
        foreach (ItemData item in questData.RewardItems)
        {
            Debug.Log($"퀘스트 보상 아이템 획득: {item.ItemName}");
            TestPlayer.Instance.playerQuest.AddQuestItem(item);;
        }

        Debug.Log($"퀘스트 보상 획득: 경험치: {questData.RewardExp}, YP: {questData.RewardYP}");
    }

    // 단일 퀘스트 해금
    private void QuestUnlock(string id)
    {
        QuestData quest = Resources.Load<QuestData>($"QuestDatas/{id}");

        if (quest == null)
        {
            Debug.LogError($"Quest Manager: 해당 퀘스트는 존재하지 않습니다.");
            return;
        }

        AvailableQuests.Add(quest);
    }

    // 다음 스토리 퀘스트 해금
    private void NextStoryQuestUnlock()
    {
        QuestData nextQuest = Resources.Load<QuestData>($"QuestDatas/Q_s{stortStage:D3}");
        stortStage++;

        if (nextQuest == null)
        {
            Debug.LogError($"Quest Manager: 다음 스토리 퀘스트가 존재하지 않습니다.");
            return;
        }

        Debug.Log($"Quest Manager: 다음 스토리 퀘스트 해금: {nextQuest.QuestName}");  
        AvailableQuests.Add(nextQuest);
    }
}

