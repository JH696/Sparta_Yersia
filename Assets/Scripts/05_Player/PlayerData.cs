using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
public class PlayerData : StatData, ISkillUsable
{
    [Header("플레이어 이름")]
    public string Name = "플레이어";

    [Header("플레이어 성별")]
    public EGender gender = EGender.Male;

    [Header("플레이어 계급")]
    [Tooltip("초급(Basic), 중급(Advanced), 상급(Expert)")]
    public E_Rank Rank = E_Rank.Basic;

    [Header("초기 스킬 리스트")]
    [SerializeField] private List<SkillData> startSkills = new List<SkillData>();

    [Header("플레이어 스프라이트")]
    public Sprite WSprite; // 월드
    public Sprite DSprite; // 대화
    public Sprite Icon; // 아이콘

    [Header("배틀씬")]
    public BattleVisuals BattleVisuals;

    public List<SkillData> StartSkills => startSkills;
}