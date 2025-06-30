using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("다이얼로그 UI")]
    public DialogueUI DialogueUI;

    [Header("json 헬퍼")]
    [SerializeField] private JsonHelper helper;

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

    // NPC용 다이얼로그 실행
    public void StartNPCDialogue(NPC npc)
    {
        if (IsDialogueActive)
        {
            Debug.Log("Dialogue Manager: 이미 대화 중입니다.");
            return;
        }

        IsDialogueActive = true;
        DialogueUI.SetAllDialogue(helper.LoadJsonFromPath("Dialogues/" + npc.NpcData.NpcID));
        DialogueUI.SetDialogueResource(npc.NpcData.DialogueSprite, npc.NpcData.NpcName);
        DialogueUI.SetDailogueNPC(npc);
        ChangeCurDialogue("Start");
        DialogueUI.ShowDialogueUI();
    }

    // 다이얼로그 UI 현재 대사 변경
    public void ChangeCurDialogue(string id)
    {
        DialogueUI.SetCurDialogue(DialogueUI.GetDialogueData($"{id}"));
    }
}