using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class QuestCondition
{
    public String TargetID;
    public int TargetCount; 
}

[Serializable]
[CreateAssetMenu(fileName = "Q0000Data", menuName = "Data/QuestData")]
public class QuestData : ScriptableObject
{
    [Header("퀘스트 제출 위치")]
    public string TargetID;

    [Header("퀘스트 완료 조건 (아이템 ID / 몬스터 ID, 수량 )")]
    public List<QuestCondition> QuestConditions;

    [Header("퀘스트 ID / 이름")]
    public string QuestID;          
    public string QuestName;        

    [Header("퀘스트 설명")]
    public string Description;      

    [Header("퀘스트 보상")]
    public int RewardExp;       
    public int RewardYP;
    public List<ItemData> RewardItems;


    public void Complete()
    {
        //GameManager.Instance.Player.AddExp(RewardExp);
        //GameManager.Instance.Player.AddYP(RewardYP);

        //foreach (var itemData in RewardItems)
        //{
        //    GameManager.Instance.Player.Inventory.AddItem(itemData);
        //}

        Debug.Log($"퀘스트 클리어:{QuestName}, 보상 획득: {RewardExp}Exp, {RewardYP}YP");
    }
}

