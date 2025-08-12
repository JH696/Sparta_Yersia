using System.Linq;

public static class ItemInventoryExtensions
{
    // 문자열 ID로 아이템 존재 여부를 검사
    public static bool HasItem(this ItemInventory inv, string itemID)
    {
        return inv.Items.Any(slot => slot.Data.ID == itemID && slot.Stack > 0);
    }

    // 문자열 ID로 아이템을 1개만 제거
    public static void RemoveItemByID(this ItemInventory inv, string itemID)
    {
        var slot = inv.Items.FirstOrDefault(s => s.Data.ID == itemID);
        if (slot != null)
            slot.LoseItem(1);
    }
}