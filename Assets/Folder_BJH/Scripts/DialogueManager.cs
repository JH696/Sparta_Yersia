using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("다이얼로그 UI")]
    public DialogueUI DialogueUI;

    [Header("다이얼로그 진행 여부")]
    public bool IsDialogueActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // [외부] : 다이얼로그 UI 작동
    public void StartDialogue(NPC npc)
    {
        if (IsDialogueActive)
        {
            Debug.Log("이미 대화 중입니다.");
            return;
        }

        IsDialogueActive = true;
        DialogueUI.SetDialogue(npc);
        DialogueUI.ShowDialogueUI();
    }
}