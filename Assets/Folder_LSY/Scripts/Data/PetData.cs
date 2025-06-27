using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PetData", menuName = "Data/PetData")]
public class PetData : ScriptableObject
{
    [Header("펫 ID 및 이름")]
    public string PetID;
    public string PetName;

    [Header("펫 스탯")]
    public CharacterStatData StatData;

    [Header("진화 관련")]
    [Tooltip("진화 단계별 스프라이트 (ex. 0 = 기본, 1 = 1차 진화, 2 = 2차 진화)")]
    public PetSprite[] sprites = new PetSprite[3];

    [Tooltip("각 진화 단계에 도달하기 위한 레벨")]
    public EvoLevel[] evoLevels = new EvoLevel[2];

    [Header("진화 시 적용할 스탯 배율")]
    public float StatMultiplier = 1.5f;
}

[Serializable]
public class PetSprite
{
    public Sprite WorldSprite;
    public Sprite Icon;
}

[Serializable]
public class EvoLevel
{
    public int Level;
}