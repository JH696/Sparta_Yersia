using System.Collections.Generic;
using UnityEngine;

public class StatTable : MonoBehaviour
{
    private Dictionary<StatType, float> statDict = new();

    public float Get(StatType type)
    {
        return statDict.TryGetValue(type, out float value) ? value : 0f;
    }

    public void Set(StatType type, float value)
    {
        statDict[type] = value;
    }

    public void Add(StatType type, float amount)
    {
        if (!statDict.ContainsKey(type))
            statDict[type] = 0f;

        statDict[type] += amount;
    }

    public Dictionary<StatType, float> GetAll()
    {
        return statDict;
    }
}
