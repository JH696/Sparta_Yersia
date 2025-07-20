using System.Collections.Generic;

[System.Serializable]
public class QuestStatusData
{
    public string QuestID; // 퀘스트 ID
    public bool IsCompleted; // 퀘스트 완료 여부
}

[System.Serializable]
public class EliQuestProgressData
{
    public string QuestID; // 퀘스트 ID
    public List<EliCountData> eliCountDatas = new List<EliCountData>(); // 엘리 카운트 데이터 리스트
}

[System.Serializable]
public class EliCountData
{
    public string EnemyID;
    public int KillCount;
}

[System.Serializable]
public class PlayerSaveData
{
    public int Level;
    public int CurrentExp;
    public float CurrnetHP;
    public float CurrentMP;
    public int YP; // 플레이어의 화폐
    public Dictionary<string, int> Inventory = new Dictionary<string, int>(); // 아이템 ID와 개수의 딕셔너리
    public List<QuestStatusData> questStatusDatas = new List<QuestStatusData>(); // 퀘스트 상태 데이터 리스트
    public List<EliQuestProgressData> eliQuestProgressDatas = new List<EliQuestProgressData>(); // 엘리 퀘스트 진행 데이터 리스트

    public List<string> ownedPetIDs = new List<string>();  // 보유한 펫 ID 리스트
    public List<string> equipPetIDs = new List<string>();  // 장착한 펫 ID 리스트
}