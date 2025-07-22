using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class B_DynamicButton : MonoBehaviour
{
    [SerializeField] private SkillStatus skill;
    [SerializeField] private ItemStatus item;

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
        text.text = "";    
        this.gameObject.SetActive(false);
    }
}
