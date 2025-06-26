using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/MonsterData")]
public class MonsterData : ScriptableObject
{
    [Header("몬스터 ID / 이름")]
    public string MonsterID;
    public string MonsterName;

    [Header("몬스터 스탯")]
    public CharacterStatData statData;

    [Header("몬스터 스프라이트")]
    public Sprite WorldSprite;
    public Sprite Icon;
}
