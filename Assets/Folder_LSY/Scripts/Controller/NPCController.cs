using UnityEngine;

public class NPCController : BaseCharacter, INPCInteractable
{
    [Header("NPC 데이터")]
    [SerializeField, Tooltip("NPC의 이름과 ID가 포함된 데이터")] private NPCData npcData;

    public void Interact()
    {
        Debug.Log($"{npcData.NpcName} 와(과) 상호작용했습니다.");
        // 여기서 대화창 출력 등 구현 가능
    }
}
