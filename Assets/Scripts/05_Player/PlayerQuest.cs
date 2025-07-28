using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 퀘스트 클리어 상태 저장
[System.Serializable]
public class QuestStatus 
{
    public QuestData QuestData;
    public bool IsCleared;

    public QuestStatus(QuestData data, bool cleared)
    {
        QuestData = data;
        IsCleared = cleared;
    }
}

// 처치 퀘스트 진행 상태 저장
[System.Serializable]
public class EliQuestProgress
{
    public string QuestID;
    public Dictionary<string, int> EliCounts;

    public EliQuestProgress(QuestData data)
    {
        Debug.Log($"처치 퀘스트 진행 상태 생성: {data.QuestName} ({data.QuestID})");

        QuestID = data.QuestID;
        EliCounts = new Dictionary<string, int>();

        foreach (var enemy in data.TargetEnemy)
        {
            EliCounts[enemy.EnemyID] = 0;
        }
    }
}

[System.Serializable]
public class PlayerQuest
{
    private ItemInventory Inventory = null; // 플레이어 인벤토리

    private Dictionary<string, QuestStatus> MyQStatus = null; // 진행 중인 퀘스트 상태 저장
    private Dictionary<string, EliQuestProgress> EliQProgress = null; // 진행 중인 처치 퀘스트 상태 저장

    public PlayerQuest(ItemInventory inventory)
    {
        Inventory = inventory;

        MyQStatus = new Dictionary<string, QuestStatus>();
        EliQProgress = new Dictionary<string, EliQuestProgress>();
    }

    // 진행 중인 퀘스트 가져오기
    public Dictionary<string, QuestStatus> GetMyQStatus()
    {
        if (MyQStatus == null)
        {
            Debug.Log("MyQStatus가 초기화되지 않았습니다. 새로 생성합니다.");
            MyQStatus = new Dictionary<string, QuestStatus>();
        }

        return MyQStatus;
    }

    // 진행 중인 처치 퀘스트 진행 상태 가져오기
    public Dictionary<string, EliQuestProgress> GetEliQProgress()
    {
        return EliQProgress;
    }

    // 진행 중인 퀘스트 추가
    public void AddMyQ(QuestData questData)
    {
        Debug.Log($"퀘스트 추가: {questData.QuestName}");

        if (questData == null)
        {
            Debug.LogError("[PlayerQuest] AddMyQ: questData가 null입니다.");
            return;
        }

        if (MyQStatus == null)
        {
            Debug.LogError("[PlayerQuest] MyQStatus가 초기화되지 않았습니다.");
            return;
        }

        MyQStatus.Add(questData.QuestID, new QuestStatus(questData, false));

        if (questData.ConditionType == EConditionType.Elimination)
        {
            AddEliQ(questData);
        }
    }

    // 진행 중인 퀘스트 삭제
    public void RemoveMyQ(QuestData questData)
    {
        MyQStatus.Remove(questData.QuestID);
    }

    // 처치 퀘스트 진행 상태 저장용 딕셔너리 생성
    public void AddEliQ(QuestData questData)
    {
        EliQProgress[questData.QuestID] = new EliQuestProgress(questData);
    }

    // 처치 퀘스트 진행 상태 저장용 딕셔너리 삭제
    public void RemoveEliQ(QuestData questData)
    {
        if (!EliQProgress.ContainsKey(questData.QuestID)) return;

        EliQProgress.Remove(questData.QuestID);
    }

    // 처치 퀘스트 진행 상태 업데이트    
    public void EliCountUp(string monsterID)
    {
        foreach (var eq in EliQProgress.Values)
        {
            if (eq.EliCounts.ContainsKey(monsterID))
            {
                eq.EliCounts[monsterID]++;
            }
        }
    }

    // 모든 퀘스트 진행 상태 업데이트 (고유 조건 만족시 IsCleared)
    public void QuestUpdate()
    {
        var keys = MyQStatus.Keys.ToList();

        foreach (string questID in keys)
        {
            QuestStatus status = MyQStatus[questID];
            QuestData data = status.QuestData;

            switch (data.ConditionType)
            {
                // 조사 퀘스트는 무시
                default:
                    break;

                // 수집 퀘스트
                case EConditionType.Collection:
                    bool CQComplete = data.TargetItem.All(item => Inventory.GetItemCount(item.Item) >= item.ItemCount);

                    if (CQComplete)
                    {
                        MyQStatus[questID] = new QuestStatus(data, true);
                    }
                    break;

                // 처치 퀘스트
                case EConditionType.Elimination:
                    bool EQComplete = data.TargetEnemy.All(enemy =>
                        EliQProgress[questID].EliCounts.TryGetValue(enemy.EnemyID, out int killCount) &&
                        killCount >= enemy.EnemyCount);

                    if (EQComplete)
                    {
                        MyQStatus[questID] = new QuestStatus(data, true);
                    }
                    break;
            }
        }
    }

    // 몬스터 데이터 찾기
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

    // 몬스터 처치 (삭제 예정)
    public void KillMonster(MonsterData monsterData)
    {
        EliCountUp(monsterData.MonsterID);
        QuestUpdate();
        QuestManager.Instance.questUI.RefreshQuestUI();
    }
}
