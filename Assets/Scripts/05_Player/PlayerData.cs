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

    [Header("프로필 아이콘 (갈색)")]
    public Sprite brownProfileIcon;
    [Header("프로필 아이콘 (다크)")]
    public Sprite darkProfileIcon;

    [Header("월드 씬용 스프라이트 (갈색)")]
    public Sprite brownWorldSprite;
    [Header("월드 씬용 스프라이트 (다크)")]
    public Sprite darkWorldSprite;

    [Header("대화용 스프라이트 (갈색)")]
    public Sprite brownDialogSprite;
    [Header("대화용 스프라이트 (다크)")]
    public Sprite darkDialogSprite;

    [Header("애니메이터 컨트롤러 (갈색)")]
    public RuntimeAnimatorController brownController;

    [Header("애니메이터 컨트롤러 (다크)")]
    public RuntimeAnimatorController darkController;

    //////[Header("배틀 비주얼 (갈색)")]
    public BattleVisuals brownBattleVisuals;

    [Header("배틀 비주얼 (다크)")]
    public BattleVisuals darkBattleVisuals;

    public List<SkillData> StartSkills => startSkills;
}