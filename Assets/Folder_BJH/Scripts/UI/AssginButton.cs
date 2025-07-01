using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssginButton : MonoBehaviour
{
    [Header("퀘스트 버튼 텍스트")]
    [SerializeField] private TextMeshProUGUI buttonText;

    [Header("등록된 퀘스트 데이터 (시각화)")]
    [SerializeField] private QuestData curQuestData;

    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(AcceptQuest);
    }

    // 버튼 세팅
    public void SetButton(QuestData questData)
    {
        if (questData == null)
        {
            Debug.LogError($"버튼에 등록할 퀘스트가 없습니다. {this.gameObject.name}");
            return;
        }

        this.curQuestData = questData;

        buttonText.text = questData.QuestName;
    }

    // 퀘스트 수락 및 대사 출력
    public void AcceptQuest()
    {
        GetComponentInParent<ChoiceButtons>().RemoveChoiceButton();
        GetComponentInParent<ChoiceButtons>().gameObject.SetActive(false);

        QuestManager.Instance.GetQuest(curQuestData);
        DialogueManager.Instance.ChangeCurDialogue(curQuestData.QuestID + "S");
        DialogueManager.Instance.DialogueUI.PassTyping();
    }
}
