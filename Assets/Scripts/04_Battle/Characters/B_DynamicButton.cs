using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class B_DynamicButton : MonoBehaviour, ITooltipHandler
{
    [TextArea]
    [SerializeField] private string tooltipText;

    [Header("등록된 스킬 / 아이템")]
    [SerializeField] private SkillStatus skill;
    [SerializeField] private ItemStatus item;

    [Header("버튼 텍스트 / 아이콘")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image icon;

    public event System.Action<SkillStatus> OnSkillSelected;
    public event System.Action<ItemStatus> OnItemSelected;

    public void SetSkill(SkillStatus status)
    {
        this.gameObject.SetActive(true);

        int cool = status.Cooldown;

        skill = status;
        item = null;
        icon.sprite = status.Data.Icon;
        icon.color = status.IsCool ? Color.gray : Color.white;
        text.text = cool > 0 ? cool.ToString() : string.Empty;

        tooltipText =
        $"{skill.Data.Name}" +
        $"\n스킬 피해량: {skill.Data.Power * 100}%" +
        $"\n스킬 쿨타임: {skill.Data.Cooldown}턴" +
        $"\n마나 소모량: {skill.Data.Cost}";

        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (!status.IsCool)
            {
                OnSkillSelected?.Invoke(status);
            }
        });
    }

    public void SetItem(ItemStatus status)
    {
        this.gameObject.SetActive(true);

        item = status;
        icon.sprite = status.Data.Icon;
        icon.color = Color.white;
        text.text = status.Stack.ToString();

        //item.

        //foreach (ItemValue value in item.Data.)
        //{

        //}

        //tooltipText =
        //$"{item.Data.Name}";

        GetComponent<Button>().onClick.AddListener(() =>
        {
            OnItemSelected?.Invoke(status);
        });
    }

    public void ResetButton()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();

        skill = null;
        item = null;
        icon.sprite = null;
        icon.color = Color.white;
        text.text = string.Empty;
        tooltipText = string.Empty;
        this.gameObject.SetActive(false);
    }

    public string GetTooltipText()
    {
        return tooltipText;
    }
}
