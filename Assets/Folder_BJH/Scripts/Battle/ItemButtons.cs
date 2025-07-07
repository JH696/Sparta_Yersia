using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtons : MonoBehaviour
{
    [Header("테스트")]
    [SerializeField] private List<ItemData> itemDatas;

    [Header("아이템 버튼")]
    [SerializeField] private List<ItemButton> itemButtons;

    [Header("뒤로가기 버튼")]
    [SerializeField] private Button returnButton;

    public void Start()
    {
        returnButton.onClick.AddListener(OnReturnButton);
    }

    public void SetItemButton()
    {
        this.gameObject.SetActive(true);

        for (int i = 0; i < itemDatas.Count; i++)
        {
            itemButtons[i].SetSkillData(itemDatas[i]);
            itemButtons[i].gameObject.SetActive(true);
        }
    }

    public void OnReturnButton()
    {
        this.gameObject.SetActive(false);
        BattleUI.Instance.ShowActionButtons();
    }
}
