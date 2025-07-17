using System;
using UnityEngine;

[Serializable]
public class StatData : ScriptableObject
{
    [Header("기초 스탯")]
    public float MaxHp = 100f;
    public float MaxMana = 80f;
    public float Attack = 20f;
    public float Defense = 10f;
    public float Luck = 5f;
    public float Speed = 10f;

    [Header("스탯 상승치")]
    public float Multiplier = 1.1f;
}