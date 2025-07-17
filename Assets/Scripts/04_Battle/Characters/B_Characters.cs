using System.Collections.Generic;
using UnityEngine;

public class B_Characters : MonoBehaviour
{
    //[Header("캐릭터 슬롯")]
    //[SerializeField] private List<B_CharacterSlot> cSlots;

    //[Header("몬스터 슬롯")]
    //[SerializeField] private List<B_MonsterSlot> mSlots;

    //[Header("배틀 UI")]
    //[SerializeField] private BattleButton ui;

    //[Header("행동 중인 캐릭터")]
    //[SerializeField] private B_CharacterSlot spotLight;

    //[Header("전투 종료 여부")]
    //[SerializeField] private bool isOver;

    //public List<B_CharacterSlot> CSlots => cSlots;
    //public List<B_MonsterSlot> MSlots => mSlots;
    //public B_CharacterSlot SpotLight => spotLight;

    //private void Awake()
    //{
    //    B_Manager.Instance.SetCharacters(this);
    //}

    //private void Update()
    //{
    //    IncreaseAPoint();
    //}

    //public void StopBattle()
    //{
    //    isOver = true;
    //}

    //public void SetAllySlots()
    //{
    //    List<GameObject> myParty = GameManager.Instance.Player.GetComponent<PlayerParty>().GetFullPartyMembers();

    //    for (int i = 0; i < myParty.Count; i++)
    //    {
    //        CSlots[i].SetCharSlot(myParty[i]);
    //    }
    //}

    //public void SetEnemySlots(List<GameObject> monsters)
    //{
    //    for (int i = 0; i < monsters.Count; i++)
    //    {
    //        MSlots[i].SetMonSlot(monsters[i]);
    //    }
    //}

    //public void ResetSpotLight()
    //{
    //    spotLight.TurnEnd();
    //    spotLight = null;
    //}

    //// 전체 슬롯 행동력 상승 메서드
    //private void IncreaseAPoint()
    //{
    //    if (CharHasTurn() || isOver) return;

    //    foreach (var slot in CSlots)
    //    {
    //        slot.IncreaseAPoint();

    //        if (slot.HasTurn())
    //        {
    //            spotLight = slot;
    //            ui.SetButton(spotLight.transform);
    //            break;
    //        }
    //    }

    //    foreach (var slot in MSlots)
    //    {
    //        slot.IncreaseAPoint();
    //    }
    //}

    //private bool CharHasTurn()
    //{
    //    foreach (var slot in CSlots)
    //    {
    //        if (slot.HasTurn())
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}
}
