using System.Collections;
using System.Text;
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
    [SerializeField] private Image playerImg;

    [Header("버튼")]
    [SerializeField] private GameObject passBtn;
    [SerializeField] private GameObject skipBtn;
    [SerializeField] private ChoiceButtons choiceBtns;

    [Header("스킬 학습 UI")]
    [SerializeField] private SkillMasteryUI masteryUI;

    [Header("자동 참조 (시각화)")]
    [SerializeField] private DialogueData[] allDialogues;
    [SerializeField] private DialogueData curDialogueData;
    [SerializeField] public NPC curNpc;

    [Header("타이핑 속도")]
    [SerializeField] private float typingSpeed = 0.05f;

    [Header("대화 스킵, 패스 가능 상태 여부")]
    [SerializeField] private bool cantPass;

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
    public void SetDialogueResource(Sprite sprite)
    {
        if (sprite == null || name == null) return;

        dialogueImg.sprite = sprite;
    }

    // 다이얼로그 UI 활성화
    public void ShowDialogueUI()
    {
        this.gameObject.SetActive(true);
        passBtn.SetActive(true);
    }

    // 다이얼로그 UI 비활성화
    public void HideDialogueUI()
    {
        this.gameObject.SetActive(false);
        passBtn.SetActive(false);
    }

    // 대사 출력 (Pass Button)
    public void PassTyping()
    {
        choiceBtns.gameObject.SetActive(false);

        if (HasNextLine())
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            DialogueLine lineData = curDialogueData.Lines[curLineIndex];
            SetSpeaker(lineData.Speaker);
            typingCoroutine = StartCoroutine(TypeLine(lineData.Text));
            curLineIndex++;
        }
        else
        {
            DisplayChoices();
        }

        passBtn.SetActive(false);
    }

    // 다음 대사 존재 여부 확인
    private bool HasNextLine()
    {
        if (curDialogueData != null && curLineIndex < curDialogueData.Lines.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // 대화 종료 (Leave Button) 
    public void ExitDialogue()
    {
        cantPass = false;
        skipBtn.SetActive(true);
        passBtn.SetActive(false);
        choiceBtns.gameObject.SetActive(false);
        choiceBtns.RemoveChoiceButton();
        ResetDialogueData();
        HideDialogueUI();
        UIManager.Instance.ShowPlayerUI();
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

    // 대화 상대 이름, 스프라이트 투명도 설정 (Player, NPC 등)
    private void SetSpeaker(string speaker)
    {
        NameTxt.text = speaker switch
        {
            "Player" => "나", // PlayerName
            "NPC" => curNpc?.GetNpcData()?.NpcName ?? "???",
            _ => speaker
        };

        if (speaker == "Player")
        {
            playerImg.GetComponent<Animator>().SetBool("IsSpeak", true);
            dialogueImg.GetComponent<Animator>().SetBool("IsSpeak", false);
        }
        else if (speaker == "NPC")
        {
            playerImg.GetComponent<Animator>().SetBool("IsSpeak", false);
            dialogueImg.GetComponent<Animator>().SetBool("IsSpeak", true);
        }
    }

    // UI 데이터 초기화
    private void ResetDialogueData()
    {
        DialogueManager.Instance.IsDialogueActive = false;
        curDialogueData = null;
        dialogueImg.sprite = null;
        curLineIndex = 0;
        curNpc = null;
        NameTxt.text = string.Empty;
        dialogueTxt.text = string.Empty;
    }


    // 대사 타이핑 효과 코루틴
    private IEnumerator TypeLine(string line)
    {
        if (dialogueTxt == null)
        {
            Debug.LogWarning("DialogueUI: dialogueTxt가 설정되지 않았습니다!");
            yield break;
        }

        dialogueTxt.text = "";
        StringBuilder sb = new StringBuilder();

        foreach (char c in line)
        {
            sb.Append(c);
            dialogueTxt.text = sb.ToString();
            yield return new WaitForSeconds(typingSpeed);
        }

        if (!cantPass)
        {
            passBtn.SetActive(true);
        }

        typingCoroutine = null;
    }

    public void DialogueSkip()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        curLineIndex = curDialogueData.Lines.Count;
        passBtn.SetActive(false);
        DisplayChoices();
    }

    //선택지 버튼 생성
    private void DisplayChoices()
    {
        GameManager.player.quest.QuestUpdate();
        choiceBtns.RemoveChoiceButton();
        choiceBtns.gameObject.SetActive(true);

        foreach (var quest in curNpc.GetReceiverQuests())
        {
            GameManager.player.quest.GetMyQStatus().TryGetValue(quest.QuestID, out QuestStatus status);

            if (status.IsCleared == true || quest.ConditionType == EConditionType.Investigation)
            {
                choiceBtns.SpawnClearBtn(quest);
            }
            else if (status.IsCleared == false)
            {
                choiceBtns.SpawnReceiveBtn(quest);
            }
        }

        foreach (var quest in QuestManager.Instance.GetAvailableQuests())
        {
            if (quest.AssignerID == curNpc.GetNpcData().NpcID)
            {
                choiceBtns.SpawnAssignBtn(quest);
            }
        }

        if (curNpc.IsTeacher)
        {
            Button studyBtn = choiceBtns.SpawnUtilityBtn("마법 배우기");
            studyBtn.onClick.AddListener(OnStudyButton);
        }
    }

    private void OnStudyButton()
    {
        cantPass = true;
        skipBtn.SetActive(false);
        playerImg.enabled = false;

        E_ElementalType type = new E_ElementalType();

        switch (curNpc.GetNpcData().NpcID)
        {
            case "N_n002":
                type = E_ElementalType.Physical;
                break;
            case "N_n003":
                type = E_ElementalType.Fire;
                break;
            case "N_n004":
                type = E_ElementalType.Ice;
                break;
            case "N_n005":
                type = E_ElementalType.Nature;
                break;
            default:
                Debug.Log("이 NPC는 선생님이 아닙니다.");
                break;
        }

        masteryUI.ShowSkillMasteryUI(type);
    }
}
