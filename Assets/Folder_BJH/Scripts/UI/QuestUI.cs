using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private GameObject questInfo;
    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private TextMeshProUGUI questDesc;
    [SerializeField] private TextMeshProUGUI questPage;
    [SerializeField] private int curQuestPage;
    [SerializeField] private List<QuestData> questList;

    private void Start()
    {
        RefreshQuestUI();
    }

    public void QuestInfoToggle()
    {
        questInfo.SetActive(!questInfo.activeSelf);
    }

    public void RefreshQuestUI()
    {
        questList = TestPlayer.Instance.playerQuest.MyQuest.Values.Select(qs => qs.questData).ToList();
        questPage.text = $"{curQuestPage + 1}/{questList.Count + 1}";

        if (questList.Count <= curQuestPage)
        {
            questName.text = "여유와 행복 :";
            questDesc.text = "진행 중인 퀘스트가 없습니다.";
        }
        else
        {
            questName.text = $"{questList[curQuestPage].QuestName} :";
            questDesc.text = questList[curQuestPage].Description;
        }
    }

    public void NextQuestPage()
    {
        if (questList.Count <= curQuestPage) return;

        curQuestPage++;
        RefreshQuestUI();
    }

    public void PreviousQuestPage()
    {
        if (questList.Count < curQuestPage || curQuestPage == 0) return;

        curQuestPage--;
        RefreshQuestUI();
    }
}
