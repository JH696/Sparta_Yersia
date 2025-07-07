using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [SerializeField] private ItemData curItem;
    [SerializeField] private Image itemIcon;

    public void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnItemButton);
    }

    public void SetSkillData(ItemData item)
    {
        curItem = item;
        itemIcon.sprite = item.Icon;
    }

    public void OnItemButton()
    {
        Debug.Log($"선택된 아이템 {curItem.name}");
        BattleUI.Instance.ItemTargeting(curItem);
    }
}
