using System;
using UnityEngine;

[Serializable]
public abstract class CharacterData : ScriptableObject, ICharacterStatData
{
    public Sprite WorldSprite;
    public Sprite Icon;

    [Header("기초 스탯")]
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float maxMana = 80f;
    [SerializeField] private float attack = 20f;
    [SerializeField] private float defense = 10f;
    [SerializeField] private float luck = 5f;
    [SerializeField] private float speed = 10f;

    public float MaxHp => maxHp;
    public float MaxMana => maxMana;
    public float Attack => attack;
    public float Defense => defense;
    public float Luck => luck;
    public float Speed => speed;
}