using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public enum EConditionType
{
    Investigation,      //조사
    Collection,         //수집
    Elimination         //처치
}

public enum EQuestType
{
    Story,          //스토리 퀘스트
    Partner,        //파트너 퀘스트
}

[Serializable]
public class CollectionCondition
{
    public string ItemID;  // 아이템 ID
    public int ItemCount; // 필요한 아이템 숫자
}

[Serializable]
public class EliminationCondition
{
    public string EnemyID; // 적 ID
    public int EnemyCount; // 처치해야 할 적 숫자
}

[Serializable]
[CreateAssetMenu(fileName = "Quest_NameData", menuName = "Data/QuestData")]
public class QuestData : ScriptableObject
{
    [Header("퀘스트 완료 조건")]
    public EConditionType ConditionType;
    public List<CollectionCondition> TargetItem = new List<CollectionCondition>();
    public List<EliminationCondition> TargetEnemy = new List<EliminationCondition>();

    [Header("퀘스트 유형")]
    public EQuestType QuestType;

    [Header("퀘스트 지급자 NPC ID")]
    public String AssignerID;

    [Header("퀘스트 보고자 NPC ID")]
    public string ReceiverID;

    [Header("퀘스트 ID / 이름")]
    public string QuestID;          
    public string QuestName;

    [Header("퀘스트 설명")]
    public string Description;

    [Header("퀘스트 보상")]
    public int RewardExp;
    public int RewardYP;
    public List<ItemData> RewardItems;
    public List<PetData> RewardPets;
}

[CustomEditor(typeof(QuestData))]
public class QuestDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("QuestType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("AssignerID"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ReceiverID"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("QuestID"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("QuestName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Description"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("RewardExp"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("RewardYP"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("RewardItems"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("RewardPets"));

        SerializedProperty questTypeProp = serializedObject.FindProperty("ConditionType");
        EditorGUILayout.PropertyField(questTypeProp);

        EConditionType questType = (EConditionType)questTypeProp.enumValueIndex;

        // 조건에 따라 필드 노출
        switch (questType)
        {
            case EConditionType.Collection:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TargetItem"));
                break;
            case EConditionType.Elimination:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TargetEnemy"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}



