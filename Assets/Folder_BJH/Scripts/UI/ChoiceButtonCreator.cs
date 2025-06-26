using System.Collections.Generic;
using UnityEngine;

public class ChoiceButtonCreator : MonoBehaviour
{
    [Header("초이스 버튼 프리팹")]
    [SerializeField] private GameObject ChoiceButton;

    [Header("생성된 버튼 리스트")]
    [SerializeField] private List<GameObject> ChoiceButtonsList;

    public void CreateChoiceButton(QuestData questData)
    {
        if (questData == null) return;

        GameObject choiceBtn = Instantiate(ChoiceButton, this.transform);
        choiceBtn.GetComponent<ChoiceButton>().SetChoiceButton(questData);

        ChoiceButtonsList.Add(choiceBtn);
    }

    public void RemoveChoiceButton()
    {
        if (ChoiceButtonsList.Count <= 0) return;

        foreach (GameObject choiceBtn in ChoiceButtonsList)
        {
            Destroy(choiceBtn);
        }

        ChoiceButtonsList.Clear();
    }
}
