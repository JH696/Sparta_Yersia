using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] private SkillData curSkill;
    [SerializeField] private Image SkillIcon;

    public void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnSkillButton);
    }

    public void SetSkillData(SkillData skill)
    {
        curSkill = skill;
        SkillIcon.sprite = skill.Icon;
    }

    public void OnSkillButton()
    {
        Debug.Log($"선택된 스킬 {curSkill.name}, 범위 {curSkill.Range}");
        BattleUI.Instance.SkillTargeting(curSkill);
    }
}
