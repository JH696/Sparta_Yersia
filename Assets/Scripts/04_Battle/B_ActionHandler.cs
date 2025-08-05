using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class B_ActionHandler : MonoBehaviour
{
    [Header("타겟 목록")]
    [SerializeField] private List<BattleEffecter> targets = new List<BattleEffecter>();

    [Header("최대 타겟 수")]
    [SerializeField] private int maxCount = 1;

    [Header("타겟팅 여부")]
    [SerializeField] private bool isTargeting;

    [Header("슬롯 매니저")]
    [SerializeField] private B_SlotManager slotManager;


    public List<BattleEffecter> Targets => targets;

    public void StartTargeting(int maxCount)
    {
        targets.Clear();
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

                BattleEffecter effecter = hit.collider.GetComponentInChildren<BattleEffecter>();

                if (effecter != null)
                {
                    AddTarget(effecter);
                }
            }
        }
    }

    private void AddTarget(BattleEffecter effecter)
    {
        if (effecter == null) return;

        if (targets.Contains(effecter))
        {
            //slot.Pointer.SetActive(false);
            targets.Remove(effecter);
        }
        else
        {
            if (targets.Count >= maxCount) return;

            //slot.Pointer.SetActive(true);
            targets.Add(effecter);
        }
    }

    // 대상 반환 초기화 

    public void EndTargeting(bool isDead)
    {
        ClearAllTargetsPointer();
        slotManager.ClearCurrentSlot();
    }

    public void ClearAllTargetsPointer()
    {
        isTargeting = false;

        foreach (BattleEffecter slot in targets)
        {
            if (slot != null)
            {
                //slot.Pointer.SetActive(false);
            }
        }
        targets.Clear();
    }
}
