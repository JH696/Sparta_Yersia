using System;
using UnityEngine;

[Serializable]
public abstract class StatData : ScriptableObject
{

    [Header("기초 스탯")]
    [SerializeField] public float maxHp = 100f;
    [SerializeField] public float maxMana = 80f;
    [SerializeField] public float attack = 20f;
    [SerializeField] public float defense = 10f;
    [SerializeField] public float luck = 5f;
    [SerializeField] public float speed = 10f;

    [Header("스탯 상승치")]
    public float Multiplier = 1.1f; // 레벨업 시 스탯 상승치
}