using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [Header("플레이어")]
    [SerializeField] private Player player;

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
        QuestManager.Instance.SetQuestUI(this);
        player.Status.inventory.InventoryChanged += RefreshQuestUI;

        RefreshQuestUI();
    }

    private void OnDestroy()
    {
        GameManager.player.inventory.InventoryChanged -= RefreshQuestUI;
    }

    // 퀘스트 정보창 표시 (버튼 연결)
    public void QuestInfoToggle()
    {
        questInfo.SetActive(!questInfo.activeSelf);
    }

    // 퀘스트 정보창 초기화 및 갱신
    public void RefreshQuestUI()
    {
        questList = player.Status.quest
       .GetMyQStatus()
       .Values
       .Select(qs => qs.QuestData)
       .ToList();

        if (questList.Count == 0)
        {
            curQuestPage = 0;
        }
        else
        {
            curQuestPage = Mathf.Clamp(curQuestPage, 0, questList.Count - 1);
        }

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

    // 퀘스트 다음 페이지 이동 (버튼 연결)
    public void NextQPage()
    {
        if (questList.Count <= curQuestPage) return;

        curQuestPage++;
        RefreshQuestUI();
    }

    // 퀘스트 이전 페이지 이동 (버튼 연결)
    public void PreviousQPage()
    {
        if (questList.Count < curQuestPage || curQuestPage == 0) return;

        curQuestPage--;
        RefreshQuestUI();
    }

    // 퀘스트 현황 메시지 생성
    private string QuestCondition(QuestData questData)
    {
        ItemInventory inventory = player.Status.inventory;

        string message;

        switch (questData.ConditionType)
        {
            default:
                return string.Empty;

            case EConditionType.Collection:
                message = ""; // 초기화
                for (int i = 0; i < questData.TargetItem.Count; i++)
                {
                    string itemName = questData.TargetItem[i].Item.Name;
                    int current = inventory.GetItemCount(questData.TargetItem[i].Item);
                    int required = questData.TargetItem[i].ItemCount;

                    message += $"{itemName}: {current}/{required}\n";
                }
                return message;

            case EConditionType.Elimination:
                message = ""; // 초기화
                for (int i = 0; i < questData.TargetEnemy.Count; i++)
                {
                    string enemyID = questData.TargetEnemy[i].EnemyID;
                    string monsterName = player.Status.quest.FindMonsterByID(enemyID).MonsterName;
                    int current = 0;
                    player.Status.quest.GetEliQProgress()[questData.QuestID].EliCounts.TryGetValue(enemyID, out current);
                    int required = questData.TargetEnemy[i].EnemyCount;

                    message += $"{monsterName}: {current}/{required}\n";
                }
                return message;
        }
    }
}
