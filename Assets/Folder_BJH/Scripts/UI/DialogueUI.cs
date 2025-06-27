using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("NPC 데이터")]
    public NPC CurNpc;

    [Header("NPC 대화 UI")]
    public Image NpcImage;
    public TextMeshProUGUI TextScreen;
    public ChoiceButtonCreator ButtonCreator;

    [Header("대화 선택지 / 이탈 버튼")]
    public GameObject PassBtn;

    [Header("타이핑 속도")]
    [SerializeField] private float typingSpeed = 0.05f;

    // 대화 데이터
    private DialogueData[] allDialogues;
    private DialogueData curDialogueData;

    // 대화 진행 상태
    private int currentLineIndex;
    private Coroutine typingCoroutine;

    // UI : 기본 설정
    public void SetNPCData(NPC npc)
    {
        this.CurNpc = npc;
        this.allDialogues = DialogueManager.Instance.Helper.LoadJsonFromPath($"Dialogues/{npc.NpcData.NpcID}");
    }

    // UI : 활성화
    public void ShowDialogueUI()
    {
        if (CurNpc == null) return;

        this.gameObject.SetActive(true);
        PassBtn.SetActive(true);
        SetSprite();
        SetDialogueData("Start");
        PassTyping();
    }

    public void LeaveButton()
    {
        ButtonCreator.LeaveButtonToggle();
        SetDialogueData("End");
        PassTyping();
        PassBtn.SetActive(false);
        ButtonCreator.RemoveChoiceButton();
        Invoke("HideDialogueUI", 2f);
        Invoke("ResetDialogueData", 2f);
    }

    // UI : 비활성화
    public void HideDialogueUI()
    {
        this.gameObject.SetActive(false);
    }

    // UI : 리셋
    private void ResetDialogueData()
    {
        CurNpc = null;
        allDialogues = null;
        curDialogueData = null;
        currentLineIndex = 0;
        NpcImage.sprite = null;
        TextScreen.text = string.Empty;
    }

    // UI : NPC 스프라이트 변경
    private void SetSprite()
    {
        if (NpcImage == null) return;

        NpcImage.sprite = CurNpc.NpcData.DialogueSprite;
    }

    public void SetDialogueData(string id)
    {
        currentLineIndex = 0;
        PassBtn.SetActive(true);
        curDialogueData = LoadJsonByID(id);
    }

    //// UI : 대화 진행
    public void PassTyping()
    {
        if (currentLineIndex < curDialogueData.Lines.Count)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            string line = curDialogueData.Lines[currentLineIndex];
            typingCoroutine = StartCoroutine(TypeLine(line));
            currentLineIndex++;
        }
        else
        {
            DisplayeChoices();
            PassBtn.SetActive(false);
        }
    }

    // UI : Json 로드
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

    private IEnumerator TypeLine(string line)
    {
        TextScreen.text = "";
        foreach (char c in line)
        {
            TextScreen.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        typingCoroutine = null;
    }

    private void DisplayeChoices()
    {
        ButtonCreator.LeaveButtonToggle();

        if (CurNpc.RequestList.Count <= 0) return;

        for (int i = 0; i < CurNpc.RequestList.Count; i++)
        {
            ButtonCreator.CreateChoiceButton(CurNpc.RequestList[i]);
        }
        PassBtn.SetActive(false);   
    }
}
