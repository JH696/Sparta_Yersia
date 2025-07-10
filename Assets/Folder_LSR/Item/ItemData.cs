using System;
using System.Collections.Generic;
using UnityEngine;

public enum EItemCategory
{
    Equipment,
    Consumable,
    Quest,
    All,
}

public enum EEquipType
{
    Weapon,
    Hat,
    Accessory,
    Clothes,
    Shoes,
}

[CreateAssetMenu(fileName = "I_e01", menuName = "Data/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("아이템 카테고리")]
    [SerializeField] private EItemCategory category;

    [Header("장비타입")]
    [SerializeField] private EEquipType equipType;

    public string ID => name;// ScriptableObject의 이름을 ID로 사용

    [Header("표시할 이름")]
    [SerializeField] private string itemName;
    public string ItemName => itemName;

    [Header("아이콘")]
    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;

    [Header("최대 소유가능 개수")]
    [SerializeField] private int maxStack = 24;
    public int MaxStack => maxStack;

    [Header("구매가")]
    public int Price;

    [Header("아이템 효과")]
    [SerializeField] private List<ItemStat> itemStats = new List<ItemStat>();

    [Serializable]
    public struct ItemStat
    {
        public EStatType eStat;
        public int value;
    }

    public IReadOnlyList<ItemStat> ItemStats => itemStats;

    public EItemCategory Category => category;
    public EEquipType EquipType => equipType;

    /// <summary>
    /// EStatType에 해당하는 아이템 효과 값을 반환 (없으면 0 반환)
    /// </summary>
    public int GetStatValue(EStatType type)
    {
        foreach (var stat in itemStats)
        {
            if (stat.eStat == type) return stat.value;

        }
        return 0; // 해당하는 효과가 없으면 0 반환
    }

    // 인벤토리에서 소모형 아이템 사용 로직
    public void OnUse()
    {
        Debug.Log($"소모형 아이템 사용됨: {itemName}");
        // TODO: 아이템 사용 효과를 실제 게임 로직에 연결 -플레이어에서 
    }

    // 인벤토리에서 장착형 아이템 장착 로직
    public void OnEquip()
    {
        Debug.Log($"장착형 아이템 장착됨: {itemName}");
        // TODO: 아이템 장착 로직 - 플레이어에서
    }


}
