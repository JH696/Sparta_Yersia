using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/MonsterData")]
public class MonsterData : ScriptableObject, ICharacterStatData
{
    [Header("몬스터 ID / 이름")]
    public string MonsterID;
    public string MonsterName;

    [Header("몬스터 등급")]
    public EMonsterType MonsterType;

    [Header("몬스터 스탯")]
    public CharacterStatData StatData;

    [Header("몬스터 스프라이트")]
    public Sprite WorldSprite;
    public Sprite Icon;


    public float MaxHp => StatData == null ? 0f : StatData.MaxHp;
    public float MaxMana => StatData == null ? 0f : StatData.MaxMana;
    public float Attack => StatData == null ? 0f : StatData.Attack;
    public float Defense => StatData == null ? 0f : StatData.Defense;
    public float Luck => StatData == null ? 0f : StatData.Luck;
    public float Speed => StatData == null ? 0f : StatData.Speed;
}
