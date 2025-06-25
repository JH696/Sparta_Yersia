using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NPC_NameData", menuName = "Data/NPCData")]
public class NPCData : ScriptableObject
{
    [Header("NPC ID / 이름")]
    public string NpcID;   
    public string NpcName;

    [Header("NPC 스프라이트")]
    public Sprite DialogueSprite;
    public Sprite WorldSprite;
    public Sprite Icon;
}
