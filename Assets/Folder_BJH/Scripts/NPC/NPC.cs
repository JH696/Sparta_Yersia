using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("NPC 데이터")]
    [SerializeField] private NPCData NpcData;

    [Header("NPC 기본 / 퀘스트 대화 json")]
    public TextAsset DefaultD;
    public TextAsset QuestD;

    public void StartChat()
    {
        Debug.Log($"NPC 대화 시작: {NpcData.NpcName}");

        QuestData assignedQuest = QuestManager.Instance.GetQuest(NpcData.NpcID);

        if (assignedQuest == null)
        {
            DialogueManager.Instance.StartDialogue(DefaultD, NpcData);
        }
        else if (assignedQuest != null)
        {
            DialogueManager.Instance.StartDialogue(QuestD, NpcData);
            QuestManager.Instance.QuestClear(assignedQuest);
        }
        else
        {
            Debug.LogWarning("퀘스트 대화가 없습니다.");
        }
    }
}
