using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public static PlayerStatus player = null;

    public B_RewardUI rewardUI;

    public BattleEncounter CurrentEncounter;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 BattleManager 유지
        }
        else
        {
            Destroy(gameObject); // 이미 존재하는 경우 중복 방지
        }
    }

    public void StartBattle(BattleEncounter encounter)
    {
        if (player == null)
        {
            player = GameManager.Instance.player.Status;
        }

        CurrentEncounter = encounter;
        SceneLoader.LoadScene("BattleScene"); // 전투 씬 로드
    }

    public void Win()
    {
        StartCoroutine(WinRoutine());
    }
    public void Lose()
    {
        StartCoroutine(LoseRoutine());
    }

    public IEnumerator WinRoutine()
    {
        List<MonsterData> monsters = CurrentEncounter.monsters;
        List<BaseItem> dropItems = new List<BaseItem>();

        int totalYp = 0;
        int totalExp = 0;

        foreach (MonsterData monster in monsters)
        {
            foreach (DropItem drop in monster.dropItems)
            {
                if (Random.Range(0f, 1f) <= drop.dropRate)
                {
                    dropItems.Add(drop.itemData);
                    player.inventory.AddItem(drop.itemData);
                }
            }

            totalExp += monster.expDrop;
            totalYp += monster.ypDrop;
        }
        player.stat.AddExp(totalExp);
        //player.AddYp(totalYp);

        yield return new WaitForSeconds(2f);

        rewardUI.ShowWinUI(dropItems, totalExp, totalYp);
    }

    public IEnumerator LoseRoutine()
    {
        yield return new WaitForSeconds(2f);

        rewardUI.ShowWinUI(null, 0, 0);
    }

    public void QuitBattle()
    {
        SceneLoader.LoadScene("Scene_BJH"); // 전투 씬 로드
    }
}