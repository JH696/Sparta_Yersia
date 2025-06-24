using System.Collections.Generic;
using UnityEngine;

public class StatTable : MonoBehaviour
{
    private Dictionary<EStatType, float> statDict = new();

    public float Get(EStatType type)
    {
        return statDict.TryGetValue(type, out float value) ? value : 0f;
    }

    public void Set(EStatType type, float value)
    {
        statDict[type] = value;
    }

    public void Add(EStatType type, float amount)
    {
        if (!statDict.ContainsKey(type))
            statDict[type] = 0f;

        statDict[type] += amount;
    }

    public Dictionary<EStatType, float> GetAll()
    {
        return statDict;
    }
}
