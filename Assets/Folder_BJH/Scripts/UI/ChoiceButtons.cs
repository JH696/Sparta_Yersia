using System.Collections.Generic;
using UnityEngine;

public class ChoiceButtons : MonoBehaviour
{
    [Header("퀘스트 버튼 프리팹")]
    [SerializeField] private GameObject questButton;

    [Header("생성된 버튼 리스트 (시각화)")]
    [SerializeField] private List<GameObject> questButtonList;

    // [외부] : 퀘스트 버튼 생성
    public void CreateQuestButton(QuestData questData)
    {
        if (questData == null) return;
        GameObject choiceBtn = Instantiate(questButton, this.transform);
        choiceBtn.GetComponent<QuestButton>().SetChoiceButton(questData);

        questButtonList.Add(choiceBtn);
    }

    // [외부] : 생성된 모든 선택지 버튼 제거 
    public void RemoveChoiceButton()
    {
        if (questButtonList.Count <= 0) return;

        foreach (GameObject choiceBtn in questButtonList)
        {
            Destroy(choiceBtn);
        }

        questButtonList.Clear();
    }
}
