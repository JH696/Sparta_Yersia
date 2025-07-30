using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButtons : MonoBehaviour
{
    [Header("퀘스트 버튼 프리팹")]
    [SerializeField] private GameObject assginBtn;
    [SerializeField] private GameObject receiveBtn;
    [SerializeField] private GameObject clearBtn;

    [Header("유틸리티 버튼 프리팹")]
    [SerializeField] private GameObject utilityBtn;

    [Header("생성된 버튼 리스트 (시각화)")]
    [SerializeField] private List<GameObject> ButtonList;

    // [외부] : 퀘스트 버튼 생성
    public void SpawnAssignBtn(QuestData questData)
    {
        if (questData == null) return;

        GameObject choiceBtn = Instantiate(assginBtn, this.transform);
        choiceBtn.GetComponent<AssginButton>().SetButton(questData);

        ButtonList.Add(choiceBtn);
    }

    public void SpawnReceiveBtn(QuestData questData)
    {
        if (questData == null) return;

        GameObject choiceBtn = Instantiate(receiveBtn, this.transform);
        choiceBtn.GetComponent<ReceiveButton>().SetButton(questData);

        ButtonList.Add(choiceBtn);
    }

    public void SpawnClearBtn(QuestData questData)
    {
        if (questData == null) return;

        GameObject choiceBtn = Instantiate(clearBtn, this.transform);
        choiceBtn.GetComponent<ClearButton>().SetButton(questData);

        ButtonList.Add(choiceBtn);
    }

    public Button SpawnUtilityBtn(string text)
    {
        GameObject choiceBtn = Instantiate(utilityBtn, this.transform);
        choiceBtn.GetComponent<UtillityButton>().SetUtillityButton(text);
        ButtonList.Add(choiceBtn);

        return choiceBtn.GetComponent<Button>();
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
