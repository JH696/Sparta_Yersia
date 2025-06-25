using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("인벤토리 UI 설정")]
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private Transform SlotContainer;
    [SerializeField] private GameObject SlotPrefab;
    [SerializeField] private List<ItemData> ItemDB;

    // 인벤토리 변경 이벤트 구독
    private void OnEnable()
    {
        inventory.OnInventoryChanged += RefreshUI;
        RefreshUI();
    }

    // 인벤토리 변경 이벤트 구독 해제
    private void OnDisable()
    {
        inventory.OnInventoryChanged -= RefreshUI;

        foreach (Transform child in SlotContainer)
        {
            Destroy(child.gameObject); // UI 비활성화 시 기존 슬롯 제거
        }
    }

    public void RefreshUI()
    {
        // 기존 슬롯 제거
        foreach (Transform child in SlotContainer)
        {
            Destroy(child.gameObject);
        }

        // 인벤토리 아이템 데이터 가져오기
        var invenItems = inventory.GetAllItems();
        foreach (var keyValue in invenItems)
        {
            var id = keyValue.Key;
            var count = keyValue.Value;

            // DB에서 ItemData 찾기
            var data = ItemDB.Find(db => db.ID == id);
            if (data == null) continue;

            // 아이템 슬롯 생성 및 Setup(설정)
            var slotObj = Instantiate(SlotPrefab, SlotContainer);
            var slot = slotObj.GetComponent<ItemSlot>();
            slot.Setup(data, count, OnSlotClicked);
        }
    }

    private void OnSlotClicked(ItemData data)
    {
        Debug.Log($"[InventoryUI] 슬롯 클릭: {data.ItemName}");
        data.OnUse();
        data.OnEquip();

        inventory.RemoveItem(data, 1); // 아이템 제거
    }
}
