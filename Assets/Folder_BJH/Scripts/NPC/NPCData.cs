using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NPC_NameData", menuName = "Data/NPCData")]
public class NPCData : ScriptableObject
{
    public string NpcID;              // NPC ID
    public string NpcName;            // NPC 이름
}
