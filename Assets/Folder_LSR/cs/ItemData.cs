using UnityEngine;

[CreateAssetMenu(fileName = "Item_Name", menuName = "Data/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("아이템 ID")]
    [SerializeField] private string id;
    [Header("아이템 이름")]
    [SerializeField] private string itemName;
    [Header("아이콘")]
    [SerializeField] private Sprite icon;

    [Header("아이템 효과")]
    [SerializeField] private int itemAtk;
    [SerializeField] private int itemDef;
    [SerializeField] private int itemSpeed; // Q: 약어? Spd?
    [SerializeField] private int itemHP;
    [SerializeField] private int itemMP;

    // 상점 구현시 주석 해제
    //[Header("구매가")]
    //public int Price;

    // public getters
    public string ID => id;
    public string ItemName => itemName;
    public Sprite Icon => icon;
    public int ItemAtk => itemAtk;
    public int ItemDef => itemDef;
    public int ItemSpeed => itemSpeed; // Q: 약어? Spd?
    public int ItemHP => itemHP;
    public int ItemMP => itemMP;


    // 인벤토리에서 소모형 아이템 사용 로직
    public void OnUse()
    {
        Debug.Log($"소모형 아이템 사용: {itemName}");
        // TODO: 아이템 사용 효과를 실제 게임 로직에 연결
    }

    // 인벤토리에서 장착형 아이템 장착 로직
    public void OnEquip()
    {
        Debug.Log($"장착형 아이템 장착: {itemName}");
        // TODO: 아이템 장착 로직
    }


}
