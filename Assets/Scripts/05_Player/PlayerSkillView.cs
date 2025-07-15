using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillView : MonoBehaviour
{
    [Header("스킬 아이콘")]
    public Image IconImg;

    [Header("레벨 텍스트")]
    public Text LevelTxt;

    [Header("탈착 버튼")]
    public Button UseBtn;

    [Header("레벨업 버튼")]
    public Button LevelUpBtn;

    [Header("잠금 상태 오버레이")]
    public Image LockedOverlay;

    [Header("장착 상태 아웃라인 효과-테스트용")]
    public Outline EquippedOutline;
}
