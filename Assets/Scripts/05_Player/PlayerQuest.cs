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
        QuestID = data.QuestID;
        EliCounts = new Dictionary<string, int>();

        foreach (var enemy in data.TargetEnemy)
        {
            EliCounts[enemy.EnemyID] = 0;
        }
    }
}

public class PlayerQuest : MonoBehaviour
{
    //private ItemInventory Inventory;

    //private Dictionary<string, QuestStatus> MyQStatus = new Dictionary<string, QuestStatus>();
    //private Dictionary<string, EliQuestProgress> EliQProgress = new Dictionary<string, EliQuestProgress>();


    //private void Start()
    //{
    //    Inventory = this.gameObject.GetComponent<ItemInventory>();
    //}

    //// 진행 중인 퀘스트 가져오기
    //public Dictionary<string, QuestStatus> GetMyQStatus()
    //{
    //    if (MyQStatus == null)
    //    {
    //        Debug.Log("MyQStatus가 초기화되지 않았습니다. 새로 생성합니다.");
    //        MyQStatus = new Dictionary<string, QuestStatus>();
    //    }

    //    return MyQStatus;
    //}

    //// 진행 중인 처치 퀘스트 진행 상태 가져오기
    //public Dictionary<string, EliQuestProgress> GetEliQProgress()
    //{
    //    return EliQProgress;
    //}

    //// 진행 중인 퀘스트 추가
    //public void AddMyQ(QuestData questData)
    //{
    //    Debug.Log($"퀘스트 추가: {questData.QuestName}");

    //    MyQStatus.Add(questData.QuestID, new QuestStatus(questData, false));

    //    if (questData.ConditionType == EConditionType.Elimination)
    //    {
    //        AddEliQ(questData);
    //    }
    //}

    //// 진행 중인 퀘스트 삭제
    //public void RemoveMyQ(QuestData questData)
    //{
    //    MyQStatus.Remove(questData.QuestID);
    //}

    //// 처치 퀘스트 진행 상태 저장용 딕셔너리 생성
    //public void AddEliQ(QuestData questData)
    //{
    //    EliQProgress[questData.QuestID] = new EliQuestProgress(questData);
    //}

    //// 처치 퀘스트 진행 상태 저장용 딕셔너리 삭제
    //public void RemoveEliQ(QuestData questData)
    //{
    //    if (!EliQProgress.ContainsKey(questData.QuestID)) return;

    //    EliQProgress.Remove(questData.QuestID);
    //}

    //// 처치 퀘스트 진행 상태 업데이트    
    //public void EliCountUp(string monsterID)
    //{
    //    foreach (var eq in EliQProgress.Values)
    //    {
    //        if (eq.EliCounts.ContainsKey(monsterID))
    //        {
    //            eq.EliCounts[monsterID]++;
    //        }
    //    }
    //}

    //// 모든 퀘스트 진행 상태 업데이트 (고유 조건 만족시 IsCleared)
    //public void QuestUpdate()
    //{
    //    var keys = MyQStatus.Keys.ToList();

    //    foreach (string questID in keys)
    //    {
    //        QuestStatus status = MyQStatus[questID];
    //        QuestData data = status.QuestData;

    //        switch (data.ConditionType)
    //        {
    //            // 조사 퀘스트는 무시
    //            default:
    //                break;

    //            // 수집 퀘스트
    //            case EConditionType.Collection:
    //                bool CQComplete = data.TargetItem.All(item => Inventory.GetCount(item.ItemData) >= item.ItemCount);
    //                if (CQComplete)
    //                {
    //                    MyQStatus[questID] = new QuestStatus(data, true);
    //                }
    //                break;

    //            // 처치 퀘스트
    //            case EConditionType.Elimination:
    //                bool EQComplete = data.TargetEnemy.All(enemy =>
    //                    EliQProgress[questID].EliCounts.TryGetValue(enemy.EnemyID, out int killCount) &&
    //                    killCount >= enemy.EnemyCount);

    //                if (EQComplete)
    //                {
    //                    MyQStatus[questID] = new QuestStatus(data, true);
    //                }
    //                break;
    //        }
    //    }
    //}

    //// 몬스터 데이터 찾기
    //public MonsterData FindMonsterByID(string id)
    //{
    //    MonsterData monster = Resources.Load<MonsterData>($"MonsterDatas/{id}");

    //    if (monster == null)
    //    {
    //        Debug.LogError($"몬스터를 찾을 수 없습니다: {id}");
    //        return null;
    //    }

    //    return monster;
    //}

    //// 몬스터 처치 (삭제 예정)
    //public void KillMonster(MonsterData monsterData)
    //{
    //    EliCountUp(monsterData.MonsterID);
    //    QuestUpdate();
    //    QuestManager.Instance.questUI.RefreshQuestUI();
    //}
}
