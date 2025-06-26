using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStatData", menuName = "Data/CharacterStatData")]
public class CharacterStatData : ScriptableObject
{
    [Header("기본 스탯")]
    public float maxHp = 100f;
    public float maxMana = 50f;
    public float attack = 20f;
    public float defense = 10f;
    public float luck = 1f;
    public float speed = 5f;
}