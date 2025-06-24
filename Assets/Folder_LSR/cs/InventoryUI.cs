using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform SlotContainer;
    [SerializeField] private GameObject SlotPrefab;
    [SerializeField] private List<ItemData> ItemDB;

    private void OnEnable()
    {
        RefreshUI();
    }

    private void OnDisable()
    {
        
    }

    public void RefreshUI()
    {
        // 기존 슬롯 제거
        // 인벤토리 아이템 데이터 가져오기
        // 아이템 슬롯 생성 및 Setup
    }

    private void OnSlotClicked(ItemData data)
    {
        Debug.Log($"[InventoryUI] 슬롯 클릭: {data.ItemName}");
        data.OnUse(); // 소모형 아이템 사용
        // data.OnEquip(); // 장착형 아이템 장착 (필요시 주석 해제)
        // InventoryManager.Instance.RemoveItem(data); // 아이템 제거
    }
}
