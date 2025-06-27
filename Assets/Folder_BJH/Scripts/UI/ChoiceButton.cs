using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    [Header("초이스 버튼 텍스트")]
    public TextMeshProUGUI ButtonText;

    [Header("연결된 퀘스트 데이터")]
    [SerializeField] private QuestData curQuestData;

    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(AcceptQuest);
    }

    public void SetChoiceButton(QuestData questData)
    {
        this.curQuestData = questData;

        ButtonText.text = questData.QuestName;
    }

    public void AcceptQuest()
    {
        QuestManager.Instance.GetQuest(curQuestData);

        GetComponentInParent<ChoiceButtonCreator>().RemoveChoiceButton();

        DialogueManager.Instance.dialogueUI.CurNpc.RequestList.Remove(curQuestData);
        DialogueManager.Instance.dialogueUI.SetDialogueData(curQuestData.QuestID);
        DialogueManager.Instance.dialogueUI.PassTyping();
    }
}
