using UnityEngine;

[CreateAssetMenu(fileName = "Item_Name", menuName = "Data/ItemData")]
public class ItemData : ScriptableObject
{
    public string ID;
    public string Name;
    public Sprite Icon;
    [TextArea] public string Description;

    // 상점 구현시 주석 해제
    // public int PriceBuy;
    // public int PriceSell;

    // 아이템 사용 로직
    public void OnUse()
    {
        Debug.Log($"아이템 사용: {Name}");
        // TODO: 아이템 사용 로직 구현
    }
}
