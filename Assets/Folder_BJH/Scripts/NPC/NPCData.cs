using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NPC_NameData", menuName = "Data/NPCData")]
public class NPCData : CharacterData
{
    [Header("NPC ID / 이름")]
    public string NpcID;   
    public string NpcName;

    [Header("NPC 다이얼로그 스프라이트")]
    public Sprite DialogueSprite;
}