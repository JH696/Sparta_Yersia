using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct QuestStatus
{
    public QuestData questData;
    public bool isCleared;

    public QuestStatus(QuestData data, bool cleared)
    {
        questData = data;
        isCleared = cleared;
    }
}

public class PlayerQuest : MonoBehaviour
{
    [Header("진행 중인 퀘스트 목록")]
    public Dictionary<string, QuestStatus> MyQuest = new Dictionary<string, QuestStatus>();

    public List<ItemData> QuestItems;

    public void AddQuest(QuestData questData)
    {
        Debug.Log($"퀘스트 추가: {questData.QuestName}");

        MyQuest.Add(questData.QuestID, new QuestStatus(questData, false));
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
                case EConditionType.Investigation:
                    // 조사 조건 처리
                    break;

                case EConditionType.Collection:
                    bool collectionComplete = data.ItemTarget.All(item =>
                       HasItem(item.ItemID, item.ItemCount));

                    if (collectionComplete)
                    {
                        MyQuest[questID] = new QuestStatus(data, true);
                    }
                    break;

                //case EConditionType.Elimination:
                //    bool eliminationComplete = data.MonsterTarget.All(enemy =>
                //        PlayerCombat.Instance.HasDefeatedEnemy(enemy.EnemyID, enemy.EnemyCount));

                //    if (eliminationComplete)
                //    {
                //        MyQuest[questID] = new QuestStatus(data, true);
                //    }
                //    break;
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

    public bool HasItem(string itemID, int count)
    {
        int itemCount = 0;

        for (int i = 0; i < QuestItems.Count; i++)
        {
            if (QuestItems[i] == FindItemByID(itemID))
            {
                itemCount++;
            }
        }

        if (itemCount >= count)
        {
            return true;
        }
        else
        {
            Debug.Log($"아이템 부족: {itemID}, 필요 개수: {count}, 현재 개수: {itemCount}");
            return false;
        }
    }   
}
