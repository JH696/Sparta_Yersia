using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class QuestStatus
{
    public QuestData questData;
    public bool isCleared;

    public QuestStatus(QuestData data, bool cleared)
    {
        questData = data;
        isCleared = cleared;
    }
}

[System.Serializable]
public class EQProgress
{
    public QuestData questData;
    public Dictionary<string, int> killCounts;

    public EQProgress(QuestData data)
    {
        questData = data;
        killCounts = new Dictionary<string, int>();

        foreach (var enemy in data.TargetEnemy)
        {
            killCounts[enemy.EnemyID] = 0;
        }
    }
}

public class PlayerQuest : MonoBehaviour
{
    [Header("진행 중인 퀘스트 목록")]
    public Dictionary<string, QuestStatus> MyQuest = new Dictionary<string, QuestStatus>();
    public Dictionary<string, EQProgress> EQProgress = new Dictionary<string, EQProgress>();

    [SerializeField] private List<ItemData> QuestItems;

    // 퀘스트 추가
    public void AddQuest(QuestData questData)
    {
        Debug.Log($"퀘스트 추가: {questData.QuestName}");

        MyQuest.Add(questData.QuestID, new QuestStatus(questData, false));

        if (questData.ConditionType == EConditionType.Elimination)
        {
            AddEQProgress(questData);
        }
    }

    // 처치 퀘스트 추가
    public void AddEQProgress(QuestData questData)
    {
        EQProgress[questData.QuestID] = new EQProgress(questData);
    }

    public void UpCountEQ(string monsterID)
    {
        foreach (var eq in EQProgress.Values)
        {
            if (eq.killCounts.ContainsKey(monsterID))
            {
                eq.killCounts[monsterID]++;
            }
        }
    }

    public void RemoveQuest(QuestData questData)
    {
        MyQuest.Remove(questData.QuestID);
    }

    public void QuestUpdate()
    {
        var keys = MyQuest.Keys.ToList();

        foreach (string questID in keys)
        {
            QuestStatus status = MyQuest[questID];
            QuestData data = status.questData;

            switch (data.ConditionType)
            {
                // 조사 퀘스트는 무시
                default:
                    break;

                // 수집 퀘스트
                case EConditionType.Collection:
                    bool CQComplete = data.TargetItem.All(item => HasItem(item.ItemID) >= item.ItemCount);
                    if (CQComplete)
                    {
                        MyQuest[questID] = new QuestStatus(data, true);
                    }
                    break;

                // 처치 퀘스트
                case EConditionType.Elimination:
                    bool EQComplete = data.TargetEnemy.All(enemy =>
                        EQProgress[questID].killCounts.TryGetValue(enemy.EnemyID, out int killCount) &&
                        killCount >= enemy.EnemyCount);

                    if (EQComplete)
                    {
                        MyQuest[questID] = new QuestStatus(data, true);
                    }
                    break;
            }
        }
    }

    public ItemData FindItemByID(string id)
    {
        ItemData item = Resources.Load<ItemData>($"ItemDatas/{id}");

        if (item == null)
        {
            Debug.LogError($"아이템을 찾을 수 없습니다: {id}");
            return null;
        }

        return item;
    }

    public MonsterData FindMonsterByID(string id)
    {
        MonsterData monster = Resources.Load<MonsterData>($"MonsterDatas/{id}");

        if (monster == null)
        {
            Debug.LogError($"몬스터를 찾을 수 없습니다: {id}");
            return null;
        }

        return monster;
    }

    public int HasItem(string itemID)
    {
        int itemCount = 0;

        for (int i = 0; i < QuestItems.Count; i++)
        {
            if (QuestItems[i] == FindItemByID(itemID))
            {
                itemCount++;
            }
        }

        return itemCount;
    }

    public void AddQuestItem(ItemData itemData)
    {
        QuestItems.Add(itemData);
        QuestUpdate();
        QuestManager.Instance.questUI.RefreshQuestUI();
    }

    public void RemoveQuestItem(ItemData itemData, int count)
    {
        for (int i = 1; i < count; i++)
        {
            QuestItems.Remove(itemData);
        }
        QuestUpdate();
        QuestManager.Instance.questUI.RefreshQuestUI();
    }

    public void KillMonster(MonsterData monsterData)
    {
        UpCountEQ(monsterData.MonsterID);
        QuestUpdate();
        QuestManager.Instance.questUI.RefreshQuestUI();
    }
}
