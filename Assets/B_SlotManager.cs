using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class B_SlotManager : MonoBehaviour
{
    [Header("행동 중인 슬롯")]
    [SerializeField] private B_Slot currentSlot;

    [Header("아군 캐릭터 슬롯")]
    [SerializeField] private List<B_Slot> allySlots = new List<B_Slot>();

    [Header("적 몬스터 슬롯")]
    [SerializeField] private List<B_Slot> enemySlots = new List<B_Slot>();

    [Header("리워드 매니저")]
    [SerializeField] private B_RewardUI rewardUI;

    [Header("행동 버튼 / 몬스터 행동")]
    [SerializeField] private B_BattleButtons battleButtons;

    [Header("전투 종료 여부")]
    [SerializeField] private bool isBattlePage = false;

    [SerializeField] private int allyDeadCount = 0;
    [SerializeField] private int enemyDeadCount = 0;

    public List<B_Slot> AllSlots
    {
        get
        {
            return allySlots.Concat(enemySlots).ToList();
        }
    }

    private void Update() // 전투 종료 상태면 리턴
    {
        if (currentSlot != null || !isBattlePage) return;

        foreach (var slot in AllSlots)
        {
            if (slot.IncreacedAP())
            {
                currentSlot = slot;

                switch (slot.GetSlotType())
                {
                    case E_B_SlotType.Ally:
                        battleButtons.OnTurnStart(slot);
                        break;
                    case E_B_SlotType.Enemy:
                        battleButtons.OnMonsterturn(slot);
                        break;
                }

                break;
            }
        }
    }

    public void StartBattlePage(PlayerStatus player, BattleEncounter encounter)
    {
        isBattlePage = true;

        SetAllySlots(player);
        SetEnemySlots(encounter);

        foreach (B_Slot slot in allySlots)
        {
            if (slot.Character != null)
            {
                slot.Character.OnCharacterDead += CheckDeadASlot;
            }
        }

        foreach (B_Slot slot in enemySlots)
        {
            if (slot.Character != null)
            {
                slot.Character.OnCharacterDead += CheckDeadESlot;
            }
        }
    }

    private void SetAllySlots(PlayerStatus player)
    {
        allySlots[0].SetSlot(player);

        PlayerParty party = player.party;

        for (int i = 0; i < party.partyPets.Count; i++)
        {
            B_Slot allySlot = allySlots[i + 1];

            allySlot.SetSlot(party.partyPets[i]);
        }
    }

    private void SetEnemySlots(BattleEncounter encounter)
    {
        List<MonsterStatus> monsters = new List<MonsterStatus>();

        foreach (var data in encounter.monsters)
        {
            MonsterStatus status = new MonsterStatus(data);
            monsters.Add(status);
        }

        for (int i = 0; i < monsters.Count; i++)
        {
            B_Slot enemySlot = enemySlots[i];

            enemySlot.SetSlot(monsters[i]);
        }
    }

    private void CheckDeadASlot() // 슬롯 죽음 확인 승리
    {
        int allyCount = allySlots.Count(slot => slot.Character != null); // 슬롯에 캐릭터가 있는지 확인
        allyDeadCount++;

        if (allyDeadCount >= allyCount)
        {
            isBattlePage = false;

            foreach (var slot in allySlots)
            {
                //slot.Character.OnCharacterDead -= CheckDeadASlot; // 이벤트 핸들러 제거
                slot.ResetSlot();
            }

            BattleManager.Instance.Lose();
            return;
        }
    }

    private void CheckDeadESlot() // 슬롯 죽음 확인 패배
    {
        int enemyCount = enemySlots.Count(slot => slot.Character != null); // 슬롯에 캐릭터가 있는지 확인
        enemyDeadCount++;

        if (enemyDeadCount >= enemyCount)
        {
            isBattlePage = false;

            foreach (var slot in enemySlots)
            {

                slot.ResetSlot();
            }

            BattleManager.Instance.Win();
            return;
        }
    }

    public void SetCurrentSlot(B_Slot slot)
    {
        currentSlot = slot;
    }

    public void ClearCurrentSlot()
    {
        if (currentSlot != null)
        {
            currentSlot = null;
        }
    }

    // 몬스터 용
    public List<B_Slot> GetNonEmptySlots()
    {
        List<B_Slot> nonEmptySlots = allySlots.Where(slot => slot.Character != null && !slot.IsDead).ToList();

        return nonEmptySlots;
    }
}
