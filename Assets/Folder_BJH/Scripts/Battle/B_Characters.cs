using System.Collections.Generic;
using UnityEngine;

public class B_Characters : MonoBehaviour
{
    [Header("캐릭터 슬롯")]
    [SerializeField] private List<B_CharacterSlot> slots;

    [Header("행동력 게이지")]
    [SerializeField] private List<B_ActionGauge> gauges;

    [Header("행동 실행자")]
    [SerializeField] private B_ActionExecutor executor;

    [Header("캐릭터 행동 중 여부")]
    [SerializeField] private bool hasTurn;

    private void Start()
    {
        LinkAllGauges();
    }

    private void LinkAllGauges()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (gauges[i] == null)
            {
                Debug.Log("[B_Characters]: 슬롯의 수와 게이지의 수가 일치하지 않습니다.");
                break;
            }

            gauges[i].SetGauge(slots[i]);
        }
    }

    private void Update()
    {
        IncreaseAPoint();
    }

    // 전체 슬롯 행동력 상승 메서드
    private void IncreaseAPoint()
    {
        if (hasTurn) return;

        foreach (var slot in slots)
        {
            slot.IncreaseAPoint(slot.GetCharacter().Speed * Time.deltaTime);

            if (slot.IsAPointMax())
            {
                hasTurn = true;
                executor.SetActionButton(slot);
                break;
            }
        }
    }
}
