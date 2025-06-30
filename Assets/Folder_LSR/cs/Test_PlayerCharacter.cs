using UnityEngine;

public class Test_PlayerCharacter : MonoBehaviour
{
    /// <summary>장착했을 때 호출</summary>
    public void Equip(ItemData item)
    {
        Debug.Log($"[PlayerCharacter] Equip: {item.ItemName}");
        // TODO: 실제 스탯에 반영 (item.GetStatValue 등)
    }

    /// <summary>해제했을 때 호출</summary>
    public void Unequip(ItemData item)
    {
        Debug.Log($"[PlayerCharacter] Unequip: {item.ItemName}");
        // TODO: 실제 스탯에서 제거
    }

    /// <summary>소모품 사용했을 때 호출</summary>
    public void Use(ItemData item)
    {
        Debug.Log($"[PlayerCharacter] Use: {item.ItemName}");
        // TODO: 소모품 효과 발동
    }

    /// <summary>퀘스트용 건네주기 호출</summary>
    public void GiveQuestItem(ItemData item)
    {
        Debug.Log($"[PlayerCharacter] GiveQuestItem: {item.ItemName}");
        // TODO: 퀘스트 시스템에 통지
    }
}
