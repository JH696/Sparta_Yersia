using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PetData", menuName = "Data/PetData")]
public class PetData : StatData
{
    [Header("펫 ID 및 이름")]
    public string PetID;
    public string PetName;

    [Header("진화 관련")]
    [Tooltip("진화 단계별 스프라이트 (ex. 0 = 기본, 1 = 1차 진화, 2 = 2차 진화)")]
    public PetSprite[] sprites = new PetSprite[3];

    [Header("펫 시작 스킬 목록")]
    [Tooltip("SkillBase 구현 SO를 드래그하세요")]
    public List<SkillData> startingSkills = new List<SkillData>();
}

[Serializable]
public class PetSprite
{
    public Sprite WorldSprite;
    public Sprite Icon;
}
