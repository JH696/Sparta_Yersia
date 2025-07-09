using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class B_DButtonEditor : MonoBehaviour
{
    [Header("기본 버튼")]
    [SerializeField] private List<Button> dButtons;

    [Header("타겟 시스템")]
    [SerializeField] private B_TargetSystem targetSystem;

    public void SetSkillButton(BaseCharacter character)
    {
    //    this.gameObject.SetActive(true);

    //    List<SkillStatus> curSkills = character.characterSkill.curSkills;

    //    for (int i = 0; i < dButtons.Count; i++)
    //    {
    //        B_DynamicButton dButton = dButtons[i].GetComponent<B_DynamicButton>();

    //        dButton.SetIcon(curSkills[i].Data.Icon);

    //        if (curSkills[i].CoolTime != 0)
    //        {
    //            dButton.SetText($"{curSkills[i].CoolTime}");
    //            dButton.SetColor(Color.gray);
    //        }

    //        dButtons[i].onClick.RemoveAllListeners();

    //        dButtons[i].onClick.AddListener(() =>
    //        {
    //            targetSystem.SetBeforeUI(this.gameObject);
    //            targetSystem.SkillTargeting(character, curSkills[i].);
    //        });
    //    }
    }


    //private void OnItemButton(BaseCharacter character)
    //{
    //    this.gameObject.SetActive(true);

    //    //Dictionary<string, int> curItems = GameManager.Instance.GetComponent<Player>().Inventory.GetAllItems();

    //    //Dictionary<string, int> filteredItems =
    //    //    curItems.Where(pair => pair.Value.data > ).ToDictionary(pair => pair.Key, pair => pair.Value);
    //}
}
