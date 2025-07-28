using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class B_ActionHandler : MonoBehaviour
{
    [Header("타겟 목록")]
    [SerializeField] private List<B_Slot> targetSlots = new List<B_Slot>();

    [Header("최대 타겟 수")]
    [SerializeField] private int maxCount = 1;

    [Header("타겟팅 여부")]
    [SerializeField] private bool isTargeting;

    [Header("슬롯 매니저")]
    [SerializeField] private B_SlotManager slotManager;


    public List<B_Slot> Targets => targetSlots;

    public void StartTargeting(int maxCount)
    {
        targetSlots.Clear();
        this.maxCount = maxCount;
        isTargeting = true;
    }

    private void Update()
    {
        if (isTargeting)
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = BattleManager.Instance.BattleCamera.ScreenToWorldPoint(Input.mousePosition);

                int layerMask = LayerMask.GetMask("Battle");

                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMask);

                if (hit.collider == null) return;

                if (hit.collider.TryGetComponent<B_Slot>(out B_Slot slot))
                {
                    AddTarget(slot);
                }
            }
        }
    }

    private void AddTarget(B_Slot slot)
    {
        if (slot.Character == null) return;

        if (targetSlots.Contains(slot))
        {
            slot.Pointer.SetActive(false);
            targetSlots.Remove(slot);
        }
        else
        {
            if (targetSlots.Count >= maxCount) return;

            slot.Pointer.SetActive(true);
            targetSlots.Add(slot);
        }
    }

    // 대상 반환 초기화 

    public void EndTargeting(bool isDead)
    {
        Debug.Log($"타겟팅 종료, isDead: {isDead}");

        ClearAllTargetsPointer();
        slotManager.ClearCurrentSlot();
    }

    public void ClearAllTargetsPointer()
    {
        isTargeting = false;

        foreach (B_Slot slot in targetSlots)
        {
            if (slot != null)
            {
                slot.Pointer.SetActive(false);
            }
        }
        targetSlots.Clear();
    }
}
