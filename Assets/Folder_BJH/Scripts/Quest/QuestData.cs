using System;
using UnityEngine;
using System.Collections.Generic;

public enum EQuestConditionType
{
    Investigation,      //조사
    Collection,         //수집
    Elimination         //처치
}

[Serializable]
public class QuestCondition
{
    public String TargetID;
    public int TargetCount; 
}

[Serializable]
[CreateAssetMenu(fileName = "Quest_NameData", menuName = "Data/QuestData")]
public class QuestData : ScriptableObject
{
    public EQuestConditionType ConditionType;

    public String AssignerID;

    [Header("퀘스트 ID / 이름")]
    public string QuestID;          
    public string QuestName;

    [Header("퀘스트 설명")]
    public string Description;

    [Header("퀘스트 조건 (조사 퀘스트는 설정 X)")]
    public List<QuestCondition> Conditions = new List<QuestCondition>();

    [Header("퀘스트 보상")]
    public int RewardExp;
    public int RewardYP;
    public List<ItemData> RewardItems;
}



