// 장비 아이템
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "I_e00", menuName = "ItemData/EquipItem")]
public class EquipItemData : BaseItem
{
    [Header("장비 분류")]
    public E_EquipType Type;

    [Header("장비 성능")]
    public List<ItemValue> Values;

    [Header("가격")]
    public int Price;

    public override E_CategoryType GetCategory()
    {
        return E_CategoryType.Equip;
    }
}