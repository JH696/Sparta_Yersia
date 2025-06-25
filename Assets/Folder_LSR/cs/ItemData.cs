using System;
using System.Collections.Generic;
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

    // 상점 구현시 주석 해제
    //[Header("구매가")]
    //public int Price;

    [Header("아이템 효과")]
    [SerializeField] private List<ItemStat> itemStats = new List<ItemStat>();

    [Serializable] public struct ItemStat
    {
        public EStatType eStat;
        public int value;
    }

    // public getters
    public string ID => id;
    public string ItemName => itemName;
    public Sprite Icon => icon;
    public IReadOnlyList<ItemStat> ItemStats => itemStats;

    /// <summary>
    /// EStatType에 해당하는 아이템 효과 값을 반환 (없으면 0 반환)
    /// </summary>
    public int GetStatValue(EStatType type)
    {
        for (int i = 0; i < ItemStats.Count; i++)
        {
            if (ItemStats[i].eStat == type)
            {
                return ItemStats[i].value; // 해당하는 효과가 있으면 값 반환
            }
        }
        return 0; // 해당하는 효과가 없으면 0 반환
    }

    // 인벤토리에서 소모형 아이템 사용 로직
    public void OnUse()
    {
        Debug.Log($"소모형 아이템 사용: {itemName}");
        // TODO: 아이템 사용 효과를 실제 게임 로직에 연결 -플레이어에서 
    }

    // 인벤토리에서 장착형 아이템 장착 로직
    public void OnEquip()
    {
        Debug.Log($"장착형 아이템 장착: {itemName}");
        // TODO: 아이템 장착 로직 - 플레이어에서
    }


}
