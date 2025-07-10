using System.Collections.Generic;
using UnityEngine;

public class B_Characters : MonoBehaviour
{
    [Header("캐릭터 슬롯")]
    [SerializeField] private List<B_CharacterSlot> slots;

    [Header("행동력 게이지")]
    [SerializeField] private List<B_ActionGauge> gauges;

    [Header("배틀 UI")]
    [SerializeField] private BattleUI ui;

    [Header("행동 중인 캐릭터")]
    [SerializeField] private B_CharacterSlot spotLight;

    public List<B_CharacterSlot> Slots => slots;
    public B_CharacterSlot SpotLight => spotLight;

    private void Start()
    {
        LinkAllGauges();
    }

    private void Update()
    {
        IncreaseAPoint();
    }

    public void ResetSpotLight()
    {
        spotLight.TurnEnd();
        spotLight = null;
    }

    // 전체 슬롯 행동력 상승 메서드
    private void IncreaseAPoint()
    {
        foreach (var slot in slots)
        {
            if (!CharHasTurn())
            {
                slot.IncreaseAPoint();

                if (slot.HasTurn())
                {
                    spotLight = slot;
                    ui.SetButton(spotLight.transform);
                    break;
                }
            }
        }
    }

    private bool CharHasTurn()
    {
        foreach (var slot in slots)
        {
            if (slot.HasTurn())
            {
                return true;
            }
        }

        return false;
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
}
