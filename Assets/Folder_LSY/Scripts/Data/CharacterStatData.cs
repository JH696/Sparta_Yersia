using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStatData", menuName = "Data/CharacterStatData")]
public class CharacterStatData : ScriptableObject, ICharacterStatData
{
    [Header("기본 스탯")]
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float maxMana = 80f;
    [SerializeField] private float attack = 20f;
    [SerializeField] private float defense = 10f;
    [SerializeField] private float luck = 5f;
    [SerializeField] private float speed = 10f;

    // 인터페이스 구현부 (외부 접근용)
    public float MaxHp => maxHp;
    public float MaxMana => maxMana;
    public float Attack => attack;
    public float Defense => defense;
    public float Luck => luck;
    public float Speed => speed;
}