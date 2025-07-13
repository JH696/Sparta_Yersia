using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class B_DButtons : MonoBehaviour
{
    [Header("기본 버튼")]
    [SerializeField] private List<Button> dButtons;

    [Header("뒤로가기 버튼")]
    [SerializeField] private Button returnButton;

    [Header("배틀 버튼")]
    [SerializeField] private GameObject bButton;


    [Header("타겟 시스템")]
    [SerializeField] private B_TargetSystem targetSystem;

    public void Start()
    {
        returnButton.onClick.AddListener(OnReturnButton);
    }

    public void SetSkillButton(List<SkillStatus> skills)
    {
        int count = Mathf.Min(skills.Count, dButtons.Count);

        this.gameObject.SetActive(true);

        for (int i = 0; i < count; i++)
        {
            B_DynamicButton dButton = dButtons[i].GetComponent<B_DynamicButton>();
            dButton.ResetButton();

            SkillStatus skill = skills[i];
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
                dButtons[i].onClick.AddListener(() => targetSystem.SkillTargeting(skills[capturedIndex], this.gameObject));
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

        for (int i = 0; i < dButtons.Count; i++)
        {
            B_DynamicButton dButton = dButtons[i].GetComponent<B_DynamicButton>();
            dButton.ResetButton();
        }

        List<ItemData> cItems = inventory.GetItemsByCategory(EItemCategory.Consumable, items);
        int count = Mathf.Min(cItems.Count, dButtons.Count);

        if (cItems.Count <= 0) return;

        for (int i = 0; i < count; i++)
        {
            B_DynamicButton dButton = dButtons[i].GetComponent<B_DynamicButton>();
            dButton.ResetButton();

            ItemData item = cItems[i];

            dButton.gameObject.SetActive(true);
            dButton.SetIcon(item.Icon);

            dButtons[i].onClick.RemoveAllListeners();

            if (inventory.GetCount(item) > 0)
            {
                dButton.SetText($"{inventory.GetCount(item)}");
            }

            int capturedIndex = i;
            dButtons[i].onClick.AddListener(() => targetSystem.ItemTargeting(item, this.gameObject));
        }
    }

    private void OnReturnButton()
    {
        this.gameObject.SetActive(false);
        bButton.SetActive(true);
    }
}
