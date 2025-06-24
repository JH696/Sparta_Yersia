using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
[CreateAssetMenu(fileName = "Quest_NameData", menuName = "Data/QuestData")]
public class NPCData : ScriptableObject
{
    [Header("NPC 이름")]
    public string NpcName;

    [Header("NPC 스프라이트")]
    public Sprite IconSprite;
    public Sprite WorldSprite;
    public Sprite DialogSprite;

    [Header("NPC 스킬")]
    //public List<Skill> Skills;

    [Header("NPC 기본 스탯")]
    public float BaseHp;
    public float BaseMp;
    public float BaseAtk;
    public float BaseDef;
    public float BaseLuck;
    public float BaseSpeed;
}
