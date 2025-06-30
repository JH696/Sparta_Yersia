using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("다이얼로그 텍스트")]
    [SerializeField] private TextMeshProUGUI dialogueTxt;

    [Header("다이얼로그 이미지")]
    [SerializeField] private Image npcImage;

    [Header("다음 대사 버튼")]
    [SerializeField] private GameObject passBtn;

    [Header("선택지 버튼")]
    [SerializeField] private ChoiceButtons choiceBtns;

    [Header("json 헬퍼")]
    [SerializeField] private JsonHelper helper;

    [Header("자동 참조 (시각화)")]
    public NPC curNpc;
    [SerializeField] private DialogueData[] allDialogues;
    [SerializeField] private DialogueData curDialogueData;

    [Header("타이핑 속도")]
    [SerializeField] private float typingSpeed = 0.05f;

    private int curLineIndex;
    private Coroutine typingCoroutine;


    // [공용]: 다이얼로그 데이터 세팅 (다이얼로그 사용시 필수, 우선적으로 사용) 
    public void SetDialogue(NPC npc)
    {
        if (npc == null)
        {
            Debug.LogError($"올바른 NPC 데이터가 필요합니다. {npc.name}");
            return;
        }

        this.curNpc = npc;
        this.allDialogues = helper.LoadJsonFromPath($"Dialogues/{curNpc.NpcData.NpcID}");
        npcImage.sprite = curNpc.NpcData.DialogueSprite;
    }

    // [공용]: 다이얼로그 UI 활성화 (UIManager와 OnEnable로 기능 분리 예정)
    public void ShowDialogueUI()
    {
        if (curNpc == null)
        {
            Debug.Log("DialogueUI.SetNPCData(NPC)를 통해 대화할 NPC 데이터를 세팅해주세요.");
            return;
        }

        this.gameObject.SetActive(true);
        passBtn.SetActive(true);
        ChooseDialogue("Start");
        PassTyping();
    }

    // [공용]: 대화 분기 선택 (Start, End, QuestID 등)   
    public void ChooseDialogue(string id)
    {
        curLineIndex = 0;
        passBtn.SetActive(true);
        curDialogueData = LoadJsonByID(id);
    }

    // [공용, 버튼] : 대사 출력
    public void PassTyping()
    {
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

    // [버튼]: 다이얼로그 종료
    public void LeaveButton()
    {
        ChooseDialogue("End");
        PassTyping();

        passBtn.SetActive(false);
        choiceBtns.gameObject.SetActive(false);
        choiceBtns.RemoveChoiceButton();
        Invoke("ResetDialogueData", 2f);
    }

    // [내부] : UI 데이터 초기화
    private void ResetDialogueData()
    {
        DialogueManager.Instance.IsDialogueActive = false;

        curNpc = null;
        allDialogues = null;
        curDialogueData = null;
        npcImage.sprite = null;
        curLineIndex = 0;
        dialogueTxt.text = string.Empty;

        this.gameObject.SetActive(false);
    }

    // [내부] : 전체 대사 중 입력한 ID의 대사 로드
    public DialogueData LoadJsonByID(string id)
    {
        foreach (var dialogue in allDialogues)
        {
            if (dialogue.DialogueID == id)
                return dialogue;
        }

        Debug.LogWarning("해당 ID의 대사가 없습니다: " + id);
        return null;
    }

    // [내부] : 대사 타이핑 코루틴
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

    // [내부] : 선택지 버튼 생성
    private void DisplayeChoices()
    {
        passBtn.SetActive(false);
        choiceBtns.gameObject.SetActive(true);

        if (curNpc.AssignQuests.Count > 0)
        {
            for (int i = 0; i < curNpc.AssignQuests.Count; i++)
            {
                choiceBtns.SpawnAssignBtn(curNpc.AssignQuests[i]);
            }
        }

        if (curNpc.ReceiveQuests.Count > 0)
        {
            for (int i = 0; i < curNpc.ReceiveQuests.Count; i++)
            {
                choiceBtns.SpawnReceiveBtn(curNpc.ReceiveQuests[i]);
            }
        }

        if (curNpc.ReceiveQuests.Count <= 0 || curNpc.AssignQuests.Count <= 0) return;
    }
}
