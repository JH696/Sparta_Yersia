using System;
using UnityEngine;

[Serializable]
public abstract class StatData : ScriptableObject
{ 
    [Header("기초 스탯")]
    public float maxHp = 100f;
    public float maxMana = 80f;
    public float attack = 20f;
    public float defense = 10f;
    public float luck = 5f;
    public float speed = 10f;

    [Header("스탯 상승치")]
    public float Multiplier = 1.1f; // 레벨업 시 스탯 상승치
}