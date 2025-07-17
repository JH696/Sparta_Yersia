using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NPC_NameData", menuName = "Data/NPCData")]
public class NPCData : StatData, ICharacterSkillSetData
{
    [Header("NPC ID / 이름")]
    public string NpcID;   
    public string NpcName;

    [Header("NPC 다이얼로그 스프라이트")]
    public Sprite DialogueSprite;

    [Header("NPC 시작 스킬 목록")]
    [Tooltip("SkillBase 구현 SO를 드래그하세요")]
    public List<SkillBase> startingSkills = new List<SkillBase>();

    public List<SkillBase> StartingSkills => startingSkills;
}