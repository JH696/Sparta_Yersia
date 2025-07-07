using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtons : MonoBehaviour
{
    [Header("테스트")]
    [SerializeField] private List<SkillData> skillDatas;

    [Header("스킬 버튼")]
    [SerializeField] private List<SkillButton> skillButtons;

    [Header("뒤로가기 버튼")]
    [SerializeField] private Button returnButton;

    public void Start()
    {
        returnButton.onClick.AddListener(OnReturnButton);
    }

    public void SetSkillButton()
    {
        this.gameObject.SetActive(true);   

        for (int i = 0; i < skillDatas.Count; i++)
        { 
            skillButtons[i].SetSkillData(skillDatas[i]);
            skillButtons[i].gameObject.SetActive(true);
        }
    }

    public void OnReturnButton()
    {
        this.gameObject.SetActive(false);
        BattleUI.Instance.ShowActionButtons();
    }
}

