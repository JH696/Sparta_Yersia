using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("플레이어 기본 정보")]
    [Tooltip("플레이어 성별")]
    public EGender gender;

    [Header("기본 프로필 아이콘")]
    [Tooltip("남성 기본 아이콘")]
    public Sprite defaultMaleIcon;

    [Tooltip("여성 기본 아이콘")]
    public Sprite defaultFemaleIcon;

    [Header("기본 스탯")]
    public float MaxHp = 100f;
    public float MaxMana = 80f;
    public float Attack = 10f;
    public float Defense = 5f;
    public float Luck = 5f;
    public float Speed = 10f;

    [Header("레벨 및 경험치")]
    public int StartLevel = 1;
    public int StartExp = 0;
    public int BaseExpToLevelUp = 100;
    public float StatMultiplierPerLevel = 1.1f;

    [Header("YP (화폐)")]
    public int StartYP = 0;

    // 성별에 따라 기본 프로필 아이콘 반환
    public Sprite GetDefaultProfileIcon()
    {
        if (gender == EGender.Male && defaultMaleIcon != null)
            return defaultMaleIcon;

        if (gender == EGender.Female && defaultFemaleIcon != null)
            return defaultFemaleIcon;

        return null;
    }
}