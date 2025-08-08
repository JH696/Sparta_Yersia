using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class B_RewardUI : MonoBehaviour
{
    [SerializeField] private GameObject rewardUI;

    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotParent;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI ypText;
    [SerializeField] private TextMeshProUGUI expText;

    [SerializeField] private Button quitButton;

    public void Start()
    {
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.RewardUI = this;
        }
        else
        {
            Debug.LogError("[B_RewardUI] BattleManager.Instance가 null입니다. 실행 순서를 확인하세요.");
        }
    }

    public void ShowWinUI(List<BaseItem> Datas, int totalExp, int totalYp) 
    {
        rewardUI.SetActive(true);
        quitButton.onClick.RemoveAllListeners();

        ypText.text = $"+ {totalYp} YP";
        expText.text = $"+ {totalExp} EXP";

        if (Datas == null)
        {
            titleText.text = "전투 패배";
            quitButton.onClick.AddListener(() => OnQuitButton(false));
            return;
        }

        titleText.text = "전투 승리";
        quitButton.onClick.AddListener(() => OnQuitButton(true));

        for (int i = 0; i < Datas.Count; i++)
        {
            Image slotImage = Instantiate(slotPrefab, slotParent).transform.GetChild(0).GetComponent<Image>();
            slotImage.sprite = Datas[i].Icon;
        }
    }

    private void OnQuitButton(bool isWin)
    {
        BattleManager.Instance.QuitBattle(isWin);
    }
}
