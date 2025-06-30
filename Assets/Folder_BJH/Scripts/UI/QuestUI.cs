using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [Header("퀘스트 정보창")]
    [SerializeField] private GameObject questInfo;

    [Header("텍스트")]
    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private TextMeshProUGUI questDesc;
    [SerializeField] private TextMeshProUGUI questCond;
    [SerializeField] private TextMeshProUGUI questPage;

    [Header("현재 페이지 (시각화)")]
    [SerializeField] private int curQuestPage;

    [Header("퀘스트 목록")]
    [SerializeField] private List<QuestData> questList;

    private void Start()
    {
        RefreshQuestUI();
    }

    public void QuestInfoToggle()
    {
        questInfo.SetActive(!questInfo.activeSelf);
    }

    public void RefreshQuestUI()
    {
        questList = TestPlayer.Instance.playerQuest.MyQuest.Values.Select(qs => qs.questData).ToList();
        questPage.text = $"{curQuestPage + 1}/{questList.Count}";

        if (questList.Count <= curQuestPage)
        {
            questName.text = "여유로운 삶 :";
            questDesc.text = "진행 중인 퀘스트가 없습니다.";
            questCond.text = string.Empty;
        }
        else
        {
            questName.text = $"{questList[curQuestPage].QuestName} :";
            questDesc.text = $"{questList[curQuestPage].Description}";
            questCond.text = $"{QuestCondition(questList[curQuestPage])}";
        }
    }

    public void NextQuestPage()
    {
        if (questList.Count <= curQuestPage) return;

        curQuestPage++;
        RefreshQuestUI();
    }

    public void PreviousQuestPage()
    {
        if (questList.Count < curQuestPage || curQuestPage == 0) return;

        curQuestPage--;
        RefreshQuestUI();
    }

    private string QuestCondition(QuestData questData)
    {
        string message;
        PlayerQuest playerQuest = TestPlayer.Instance.playerQuest;

        switch (questData.ConditionType)
        {
            default:
                return null;

            case EConditionType.Collection:
                message = ""; // 초기화
                for (int i = 0; i < questData.TargetItem.Count; i++)
                {
                    string itemID = questData.TargetItem[i].ItemID;
                    string itemName = playerQuest.FindItemByID(itemID).ItemName;
                    int current = playerQuest.HasItem(itemID);
                    int required = questData.TargetItem[i].ItemCount;

                    message += $"{itemName}: {current}/{required}\n";
                }
                return message;

            case EConditionType.Elimination:
                message = ""; // 초기화
                for (int i = 0; i < questData.TargetEnemy.Count; i++)
                {
                    string enemyID = questData.TargetEnemy[i].EnemyID;
                    string monsterName = playerQuest.FindMonsterByID(enemyID).MonsterName;
                    int current = 0;
                    playerQuest.EQProgress[questData.QuestID].killCounts.TryGetValue(enemyID, out current);
                    int required = questData.TargetEnemy[i].EnemyCount;

                    message += $"{monsterName}: {current}/{required}\n";
                }
                return message;
        }
    }
}
