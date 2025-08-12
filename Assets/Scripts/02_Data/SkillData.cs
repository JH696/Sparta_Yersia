using UnityEngine;

[CreateAssetMenu(fileName = "SF_a01", menuName = "Data/SkillData")]
public class SkillData : ScriptableObject
{
    [Header("ID / 이름")]
    public string ID;
    public string Name;

    [Header("속성 / 등급")]
    public E_ElementalType Type;
    public E_Rank Rank;

    [Header("스킬 성능")]
    public float Power;
    public int Range;
    public int Cooldown;
    public int Cost;

    [Header("아이콘")]
    public Sprite Icon;

    [Header("사운드")]
    public AudioClip CastSFX;
    public AudioClip HitSFX;
}