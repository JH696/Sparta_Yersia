using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
public class PlayerData : ScriptableObject, ICharacterStatData
{
    [Header("플레이어 기본 정보")]
    [Tooltip("플레이어 성별")]
    public EGender gender;

    [Header("기본 프로필 아이콘")]
    [Tooltip("남성 기본 아이콘")]
    public Sprite MaleIcon;

    [Tooltip("여성 기본 아이콘")]
    public Sprite FemaleIcon;

    [Header("기본 스탯 데이터")]
    [Tooltip("플레이어의 스탯을 정의한 ScriptableObject")]
    public CharacterStatData StatData;

    [Header("레벨 및 경험치")]
    [Tooltip("시작 레벨")]
    public int StartLevel = 1;

    [Tooltip("시작 경험치")]
    public int StartExp = 0;

    [Tooltip("레벨업 기준 경험치 (기본값 * 현재 레벨)")]
    public int BaseExpToLevelUp = 100;

    [Tooltip("레벨업 시 스탯 배율 증가")]
    public float StatMultiplierPerLevel = 1.1f;

    [Header("YP (화폐)")]
    [Tooltip("시작 YP(돈)")]
    public int StartYP = 0;

    // 성별에 따라 기본 프로필 아이콘 반환
    public Sprite GetDefaultProfileIcon()
    {
        if (gender == EGender.Male && MaleIcon != null)
            return MaleIcon;

        if (gender == EGender.Female && FemaleIcon != null)
            return FemaleIcon;

        return null;
    }

    // ICharacterStatData 구현
    public float MaxHp => StatData == null ? 0f : StatData.MaxHp;
    public float MaxMana => StatData == null ? 0f : StatData.MaxMana;
    public float Attack => StatData == null ? 0f : StatData.Attack;
    public float Defense => StatData == null ? 0f : StatData.Defense;
    public float Luck => StatData == null ? 0f : StatData.Luck;
    public float Speed => StatData == null ? 0f : StatData.Speed;
}