//using System.Collections.Generic;
//using UnityEngine;

//[CreateAssetMenu(fileName = "NewDropTable", menuName = "Data/DropTable")]
//public class DropTableSO : ScriptableObject
//{
//    public List<DropItem> dropItems;

//    public BaseItem[] GetDrops()
//    {
//        List<BaseItem> drops = new List<BaseItem>();
//        foreach (var item in dropItems)
//        {
//            if (Random.value < item.dropRate)
//            {
//                drops.Add(item.itemData);
//            }
//        }
//        return drops.ToArray();
//    }
//}