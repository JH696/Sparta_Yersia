using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillDetailUI : MonoBehaviour
{
    [Header("텍스트 필드")]
    [SerializeField] private TextMeshProUGUI skillNameTxt;
    [SerializeField] private TextMeshProUGUI tierTxt;
    [SerializeField] private TextMeshProUGUI damageTxt;
    [SerializeField] private TextMeshProUGUI manaCostTxt;
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private TextMeshProUGUI myPointsText;

    [Header("액션 버튼")]
    [SerializeField] private Button actionBtn;
    [SerializeField] private TextMeshProUGUI actionBtnTxt;

    private SkillData currentSkill;
    private SkillNodeUI currentNodeUI;
    private PlayerSkillController skillController;

    private void Awake()
    {
        skillController = FindObjectOfType<PlayerSkillController>();
        Hide();
    }

    public void Show(SkillData skill, SkillNodeUI nodeUI)
    {
        currentSkill = skill;
        currentNodeUI = nodeUI;

        // 정보 표시
        skillNameTxt.text = skill.SkillName;
        tierTxt.text = $"등급: {skill.Tier}";
        damageTxt.text = $"데미지: {skill.Damage}";
        manaCostTxt.text = $"공격시 마나소모량: {skill.ManaCost}";
        levelTxt.text = $"레벨 {skillController.GetSkillLevel(skill)} / {skill.MaxLevel}";
        myPointsText.text = $"보유 스킬 포인트: {skillController.AvailableSkillPoints()}";

        // 버튼 세팅
        actionBtn.onClick.RemoveAllListeners();

        if (!skillController.IsUnlocked(skill))
        {
            if (skillController.CanUnlock(skill))
            {
                actionBtnTxt.text = "잠금 해제";
                actionBtn.interactable = true;
                actionBtn.onClick.AddListener(OnUnlockClick);
            }
            else
            {
                actionBtnTxt.text = "잠김";
                actionBtn.interactable = false;
            }
        }
        else
        {
            actionBtnTxt.text = "레벨업";
            actionBtn.interactable = true;
            actionBtn.onClick.AddListener(OnLevelUpClick);
        }
        gameObject.SetActive(true);
    }

    // 패널 숨기기
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnUnlockClick()
    {
        skillController.UnlockSkill(currentSkill);
        currentNodeUI.RefreshLockState();
        Show(currentSkill, currentNodeUI);
    }

    private void OnLevelUpClick()
    {
        skillController.LevelUpSkill(currentSkill);
        Show(currentSkill, currentNodeUI);
    }
}
