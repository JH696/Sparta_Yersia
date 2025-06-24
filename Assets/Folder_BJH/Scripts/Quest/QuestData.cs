using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Quest_NameData", menuName = "Data/QuestData")]
public class QuestData : ScriptableObject
{
    public int QuestID;
    public string QuestName;
    public string Description;
    public List<ItemData> Rewards;
}
