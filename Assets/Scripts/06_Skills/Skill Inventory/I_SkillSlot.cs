using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class I_SkillSlot : MonoBehaviour
{
    [Header("등록된 스킬")]
    [SerializeField] private SkillStatus curStatus;

    [Header("아이콘")]
    [SerializeField] private Image icon;

    [Header("레벨 텍스트")]
    [SerializeField] private TextMeshProUGUI level;

    [Header("버튼")]
    [SerializeField] private Button button;

    public SkillStatus CurStatus => curStatus;

    public event System.Action<I_SkillSlot> OnSlotClicked;

    public void SetSlot(SkillStatus status)
    {
        curStatus = status;
        status.StatusChanged += UpdateSlot;

        UpdateSlot();
    }

    public void UpdateSlot()
    {
        if (curStatus == null)
        {
            Destroy(this);  
            return;
        }

        icon.sprite = curStatus.Data.Icon;
        level.text = $"LV.{curStatus.Level}";
    }

    public void OnClick()
    {
        icon.color = Color.red;
        OnSlotClicked.Invoke(this);
    }

    public void ResetColor()
    {
        icon.color = Color.white;
    }
}
