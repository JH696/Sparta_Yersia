using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class B_DButtons : MonoBehaviour
{
    [Header("기본 버튼")]
    [SerializeField] private List<Button> dButtons;

    [Header("타겟 시스템")]
    [SerializeField] private B_TargetSystem targetSystem;

    public void SetSkillButton(List<SkillStatus> skills)
    {
        int count = Mathf.Min(skills.Count, dButtons.Count);

        for (int i = 0; i < count; i++)
        {
            this.gameObject.SetActive(true);

            SkillStatus skill = skills[i];
            B_DynamicButton dButton = dButtons[i].GetComponent<B_DynamicButton>();

            dButton.gameObject.SetActive(true);
            dButton.SetIcon(skill.Data.Icon);

            dButtons[i].onClick.RemoveAllListeners();

            if (skill.Cooldown != 0)
            {
                dButton.SetText($"{skill.Cooldown}");
                dButton.SetColor(Color.gray);
            }
            else
            {
                int capturedIndex = i;
                dButtons[i].onClick.AddListener(() => targetSystem.SetBeforeUI(this.gameObject));
                dButtons[i].onClick.AddListener(() => targetSystem.SkillTargeting(skills[capturedIndex]));
            }
        }

        for (int i = skills.Count; i < dButtons.Count; i++)
        {
            dButtons[i].gameObject.SetActive(false);
            dButtons[i].onClick.RemoveAllListeners();
        }
    }

    public void SetItemButton()
    {
        this.gameObject.SetActive(true);

        PlayerInventory inventory = GameManager.Instance.Player.GetComponent<PlayerInventory>();

        Dictionary<string, int> allitems = inventory.GetAllItems();

        List<ItemData> items = new List<ItemData>();

        foreach (KeyValuePair<string, int> pair in allitems)
        {
            items.Add(inventory.SearchItemToID(pair.Key));

            Debug.Log(pair.Key);
        }

        Debug.Log(items[0]);
        Debug.Log(items[1]);

        inventory.GetItemsByCategory(EItemCategory.Consumable, items);

        int count = Mathf.Min(items.Count, dButtons.Count);

        for (int i = 0; i < count; i++)
        {
            ItemData item = items[i];
            B_DynamicButton dButton = dButtons[i].GetComponent<B_DynamicButton>();

            dButton.gameObject.SetActive(true);
            dButton.SetIcon(item.Icon);

            dButtons[i].onClick.RemoveAllListeners();

            if (inventory.GetCount(item) > 0)
            {
                dButton.SetText($"{inventory.GetCount(item)}");
            }

                int capturedIndex = i;
                dButtons[i].onClick.AddListener(() => targetSystem.SetBeforeUI(this.gameObject));
                dButtons[i].onClick.AddListener(() => targetSystem.ItemTargeting(item));
        }
    }
}
