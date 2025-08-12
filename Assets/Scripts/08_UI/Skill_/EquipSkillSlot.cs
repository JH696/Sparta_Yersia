using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipSkillSlot : MonoBehaviour
{
    [Header("아이콘")]
    [SerializeField] private Image icon;

    [Header("투명 이미지")]
    [SerializeField] private Sprite defaultImg;

    [Header("텍스트")]
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI power;
    [SerializeField] private TextMeshProUGUI range;
    [SerializeField] private TextMeshProUGUI cooldown;
    [SerializeField] private TextMeshProUGUI cost;

    public void SetSlot(SkillStatus status)
    {
        icon.sprite = status.Data.Icon;
        skillName.text = status.Data.Name;
        level.text = $"레벨: {status.Level}";
        power.text = $"피해량: {status.Power * 100:N2}%";
        range.text = $"범위: {status.Data.Range}";
        cooldown.text = $"쿨다운: {status.Data.Cooldown}";
        cost.text = $"마나 소모량: {status.Data.Cost}";
    }

    public void ResetSlot()
    {
        icon.sprite = defaultImg;   
        skillName.text = "미장착";
        level.text = string.Empty;
        power.text = string.Empty;
        range.text = string.Empty;
        cooldown.text = string.Empty;
        cost.text = string.Empty;
    }
}
