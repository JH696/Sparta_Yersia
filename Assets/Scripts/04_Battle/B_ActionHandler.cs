using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class B_ActionHandler : MonoBehaviour
{
    [Header("타겟 목록")]
    [SerializeField] private List<BattleEffecter> targets = new List<BattleEffecter>();
    [SerializeField] private List<B_StatGauge> gauges = new List<B_StatGauge>();

    [Header("최대 타겟 수")]
    [SerializeField] private int maxCount = 1;

    [Header("가이드 텍스트")]
    [SerializeField] private GuideTextUI guideText;

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

        RefreshGuideText();
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

                B_Slot slot = hit.collider.GetComponent<B_Slot>();
                B_StatGauge gauge = slot.StatGauge;

                if (!slot.Character.IsDead && effecter != null)
                {
                    AddTarget(effecter, gauge);
                }
            }
        }
    }

    private void AddTarget(BattleEffecter effecter, B_StatGauge statGauge)
    {
        if (effecter == null) return;

        if (targets.Contains(effecter))
        {
            gauges.Remove(statGauge);
            statGauge.HidePointer();
            targets.Remove(effecter);
        }
        else
        {
            if (targets.Count >= maxCount) return;

            gauges.Add(statGauge);
            statGauge.ShowPointer();
            targets.Add(effecter);
        }

        RefreshGuideText();
    }

    public void EndTargeting()
    {
        ClearAllTargets();
        slotManager.ClearCurrentSlot();
    }

    private void RefreshGuideText()
    {
        guideText.UpdateGuideText("목표 지정", $"{targets.Count}/{maxCount} 선택됨");
    }

    public void ClearAllTargets()
    {
        isTargeting = false;

        foreach (B_StatGauge statGauge in gauges)
        {
            statGauge.HidePointer();    
        }

        gauges.Clear();
        targets.Clear();

        guideText.ResetGuideText();
    }
}
