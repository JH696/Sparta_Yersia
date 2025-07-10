using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class B_DButtons : MonoBehaviour
{
    [Header("기본 버튼")]
    [SerializeField] private List<Button> dButtons;

    [Header("타겟 시스템")]
    [SerializeField] private B_TargetSystem targetSystem;

    public void SetSkillButton(B_CharacterSlot slot)
    {
        this.gameObject.SetActive(true);

        List<SkillStatus> curSkills = slot.GetLearnedSkill();

        for (int i = 0; i < curSkills.Count; i++)
        {
            B_DynamicButton dButton = dButtons[i].GetComponent<B_DynamicButton>();

            dButton.gameObject.SetActive(true);
            dButton.SetIcon(curSkills[i].Data.Icon);

            if (curSkills[i].Cooldown != 0)
            {
                dButton.SetText($"{curSkills[i].Cooldown}");
                dButton.SetColor(Color.gray);
            }

            dButtons[i].onClick.RemoveAllListeners();

            dButtons[i].onClick.AddListener(() =>
            {
                dButton.ResetButton();
                targetSystem.SetBeforeUI(this.gameObject);
                targetSystem.SkillTargeting(curSkills[i].Data);
            });
        }
    }


    //private void OnItemButton(BaseCharacter character)
    //{
    //    this.gameObject.SetActive(true);

    //    //Dictionary<string, int> curItems = GameManager.Instance.GetComponent<Player>().Inventory.GetAllItems();

    //    //Dictionary<string, int> filteredItems =
    //    //    curItems.Where(pair => pair.Value.data > ).ToDictionary(pair => pair.Key, pair => pair.Value);
    //}
}
