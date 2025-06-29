using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("텍스트")]
    [SerializeField] private TextMeshProUGUI dialogueTxt;
    [SerializeField] private TextMeshProUGUI NameTxt;

    [Header("이미지")]
    [SerializeField] private Image dialogueImg;

    [Header("버튼")]
    [SerializeField] private GameObject passBtn;
    [SerializeField] private ChoiceButtons choiceBtns;

    [Header("자동 참조 (시각화)")]
    [SerializeField] private DialogueData[] allDialogues;
    [SerializeField] private DialogueData curDialogueData;
    [SerializeField] public NPC curNpc;

    [Header("타이핑 속도")]
    [SerializeField] private float typingSpeed = 0.05f;

    private int curLineIndex;
    private Coroutine typingCoroutine;


    // 전체 대사 데이터 설정 (다이얼로그 사용시 필수, 우선적으로 사용) 
    public void SetAllDialogue(DialogueData[] allDatas)
    {
        if (allDatas == null)
        {
            Debug.Log($"Dialogue UI: 올바른 데이터 배열을 설정 해주세요.");
            return;
        }

        allDialogues = allDatas;
        passBtn.SetActive(true);
    }

    // 현재 대사 데이터 설정 (다이얼로그 사용시 필수, 우선적으로 사용) 
    public void SetCurDialogue(DialogueData curData)
    {
        if (curData == null)
        {
            Debug.Log($"Dialogue UI: 올바른 데이터를 설정 해주세요.");
            return;
        }

        curLineIndex = 0;
        this.curDialogueData = curData;
        passBtn.SetActive(true);
    }

    // 대화 상대 NPC 설정 (NPC 사용시 필수)
    public void SetDailogueNPC(NPC npc)
    {
        if (npc == null) return;

        this.curNpc = npc;
    }

    // 대화 상대 이미지 및 이름 설정
    public void SetDialogueResource(Sprite sprite, string name)
    {
        if (sprite == null || name == null) return;

        NameTxt.text = name;
        dialogueImg.sprite = sprite;
    }

    // 다이얼로그 UI 활성화 (UIManager와 OnEnable로 기능 분리 예정)
    public void ShowDialogueUI()
    {
        if (passBtn == null)
        {
            Debug.Log("할당되지 않은 컴포넌트가 있습니다.");
            return;
        }

        this.gameObject.SetActive(true);
        passBtn.SetActive(true);
        PassTyping();
    }

    // 대사 출력 (Pass Button)
    public void PassTyping()
    {
        choiceBtns.gameObject.SetActive(false);

        if (curLineIndex < curDialogueData.Lines.Count)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            string line = curDialogueData.Lines[curLineIndex];
            typingCoroutine = StartCoroutine(TypeLine(line));
            curLineIndex++;
        }
        else
        {
            DisplayeChoices();
        }
    }

    // 대화 종료 (Leave Button) 
    public void ExitDialogue()
    {
        SetCurDialogue(GetDialogueData("End"));
        PassTyping();

        passBtn.SetActive(false);
        choiceBtns.gameObject.SetActive(false);
        choiceBtns.RemoveChoiceButton();
        Invoke("ResetDialogueData", 2f);
    }

    public DialogueData GetDialogueData(string id)
    {
        foreach (DialogueData dialogue in allDialogues)
        {
            if (dialogue.DialogueID == id)
            {
                return dialogue;
            }
        }

        return null;
    }

    // UI 데이터 초기화
    private void ResetDialogueData()
    {
        DialogueManager.Instance.IsDialogueActive = false;

        curDialogueData = null;
        dialogueImg.sprite = null;
        curLineIndex = 0;
        NameTxt.text = string.Empty;
        dialogueTxt.text = string.Empty;

        this.gameObject.SetActive(false);
    }


    // 대사 타이핑 효과 코루틴
    private IEnumerator TypeLine(string line)
    {
        dialogueTxt.text = "";
        foreach (char c in line)
        {
            dialogueTxt.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        typingCoroutine = null;
    }

    // 선택지 버튼 생성
    private void DisplayeChoices()
    {
        choiceBtns.RemoveChoiceButton();
        choiceBtns.gameObject.SetActive(true);
        passBtn.SetActive(false);   

        foreach (var questPair in TestPlayer.Instance.playerQuest.MyQuest)
        {
            QuestStatus status = questPair.Value;
            QuestData data = status.questData;

            if (data.ReceiverID == curNpc.NpcData.NpcID || status.isCleared)
            {
                if (status.isCleared || data.ConditionType == EConditionType.Investigation)
                {
                    choiceBtns.SpawnClearBtn(data);
                }
                else
                {
                    choiceBtns.SpawnReceiveBtn(data);
                }
            }
        }

        foreach (var quest in QuestManager.Instance.GetAvailableQuests())
        {
            if (quest.AssignerID == curNpc.NpcData.NpcID)
            {
                choiceBtns.SpawnAssignBtn(quest);
            }
        }
    }
}
