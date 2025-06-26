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

    private DialogueData dialogueData;
    private int currentLineIndex;
    private Coroutine typingCoroutine;

    // UI : SET
    public void SetDialogueUIData(NPC npc, DialogueData dialogueData)
    {
        this.CurNpc = npc;
        this.dialogueData = dialogueData;
    }

    // UI : ON
    public void ShowDialogueUI()
    {
        if (CurNpc == null) return;

        this.gameObject.SetActive(true);
        PassBtn.SetActive(true);
        SetSprite();
        TypingText();
    }

    // UI : OFF
    public void HideDialogueUI()
    {
        this.gameObject.SetActive(false);
        ResetDialogueUI();
    }

    private void ResetDialogueUI()
    {
        currentLineIndex = 0;
        NpcImage.sprite = null;
        TextScreen.text = string.Empty;
        ButtonCreator.RemoveChoiceButton();
        ButtonCreator.gameObject.SetActive(false);
    }

    private void SetSprite()
    {
        if (NpcImage == null) return;

        NpcImage.sprite = CurNpc.NpcData.DialogueSprite;
    }

    //public DialogueData FindDialogueData(string id)
    //{
    //}

    public void TypingText()
    {
        PassBtn.SetActive(true);

        if (currentLineIndex < dialogueData.Lines.Count)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            string line = dialogueData.Lines[currentLineIndex];
            typingCoroutine = StartCoroutine(TypeLine(line));
            currentLineIndex++;
        }
        else
        {
            currentLineIndex = 0;
            DisplayeChoices();
        }
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
        ButtonCreator.gameObject.SetActive(true);

        if (CurNpc.RequestList.Count <= 0) return;
        ButtonCreator.CreateChoiceButton(CurNpc.RequestList[0]);
        PassBtn.SetActive(false);   
    }
}
