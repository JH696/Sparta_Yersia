using System.Collections.Generic;
using UnityEngine;

public enum E_SizeType
{
    Medium = 1,   // 중형 (플레이어, 대부분의 캐릭터)
    Small = 0,    // 소형 (쥐, 버섯 괴물 등)
    Large = 2     // 대형 (골렘, 보스급 몬스터 등)
}

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/MonsterData")]
public class MonsterData : StatData, ISkillUsable
{
    [Header("몬스터 ID / 이름")]
    public string MonsterID;
    public string MonsterName;

    [Header("스폰 지역 / 체격")]
    public E_StageType StageType;
    public E_SizeType Size;

    [Header("초기 스킬 리스트")]
    [SerializeField] private List<SkillData> startSkills = new List<SkillData>();

    [Header("처치 보상")]
    public int ypDrop = 0;
    public int expDrop = 0;
    public List<DropItem> dropItems = new List<DropItem>();

    [Header("배틀씬")]
    public BattleVisuals BattleVisuals;

    public List<SkillData> StartSkills => startSkills;
}

[System.Serializable]
public struct DropItem
{
    public BaseItem itemData;
    public float dropRate;
}

[System.Serializable]
public struct BattleVisuals
{
    public Sprite Stand;
    public AnimationClip Move;
    public AnimationClip Idle;
    public AnimationClip Attack;
    public AnimationClip Cast;
    public AnimationClip Hit;
    public AnimationClip Die;
}