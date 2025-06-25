using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
[CreateAssetMenu(fileName = "QID_Data", menuName = "Data/QuestData")]
public class QuestData : ScriptableObject
{
    [Header("퀘스트 ID / 이름")]
    public string QuestID;            // 퀘스트 ID
    public string QuestName;          // 퀘스트 이름

    [Header("퀘스트 설명")]
    public string Description;         // 퀘스트 설명

    [Header("퀘스트 보상")]
    public int RewardExp;              // 퀘스트 완료 시 보상 경험치
    public int RewardYP;             // 퀘스트 완료 시 보상 골드

    [Header("퀘스트 목표")]
    public List<QuestBase> QuestBase;       // 퀘스트 기본 정보
}

[Serializable]
public class QuestBase
{
    public EQuestType QuestType;
    public String TargetID;
}

public enum EQuestType
{
    Investigation,
    Collection,
    Elimination
}
