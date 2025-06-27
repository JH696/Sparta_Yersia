using System.Collections.Generic;
using UnityEngine;

public class ChoiceButtons : MonoBehaviour
{
    [Header("퀘스트 버튼 프리팹")]
    [SerializeField] private GameObject assginBtn;
    [SerializeField] private GameObject receiveBtn;

    [Header("생성된 버튼 리스트 (시각화)")]
    [SerializeField] private List<GameObject> ButtonList;

    // [외부] : 퀘스트 버튼 생성
    public void SpawnAssignBtn(QuestData questData)
    {
        if (questData == null) return;

        GameObject choiceBtn = Instantiate(assginBtn, this.transform);
        choiceBtn.GetComponent<AssginButton>().SetChoiceButton(questData);

        ButtonList.Add(choiceBtn);
    }

    public void SpawnReceiveBtn(QuestData questData)
    {
        if (questData == null) return;

        GameObject choiceBtn = Instantiate(receiveBtn, this.transform);
        choiceBtn.GetComponent<ReceiveButton>().SetChoiceButton(questData);

        ButtonList.Add(choiceBtn);
    }

    // [외부] : 생성된 모든 선택지 버튼 제거 
    public void RemoveChoiceButton()
    {
        if (ButtonList.Count <= 0) return;

        foreach (GameObject choiceBtn in ButtonList)
        {
            Destroy(choiceBtn);
        }

        ButtonList.Clear();
    }
}
