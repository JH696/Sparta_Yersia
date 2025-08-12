// 일반 아이템 (퀘스트 아이템)
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "I_q00", menuName = "ItemData/QuestItem")]
public class QuestItemData : BaseItem
{
    [Header("설명")]
    public string Description;

    public override E_CategoryType GetCategory()
    {
        return E_CategoryType.Quest;
    }

    public void Give()
    {
        Debug.Log($"[QuestItemData] {Name}을(를) 받았습니다.");
    }
}