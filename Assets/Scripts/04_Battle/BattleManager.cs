using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public Camera BattleCamera;
    public Camera WorldCamera;

    public B_RewardUI rewardUI;

    public BattleEncounter CurrentEncounter;

    public bool IsBattleActive = false;

    [SerializeField] private AudioClip battleBGM;

    [SerializeField] private AudioClip winBGM;
    [SerializeField] private AudioClip loseBGM;

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
        IsBattleActive = true;
        CurrentEncounter = encounter;
        WorldCamera.enabled = false;
        BattleCamera.enabled = true;

        // BGM 재생
        if (battleBGM != null)
            SoundManager.Instance.PlayBGM(battleBGM, loop: true, fadeDuration: 1f);

        SceneLoader.MultipleLoadScene("BattleScene");
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
        // 승리 브금 재생
        if (winBGM != null)
            SoundManager.Instance.PlayBGM(winBGM, loop: false, fadeDuration: 0.5f);

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
                    GameManager.player.inventory.AddItem(drop.itemData);
                }
            }

            totalExp += monster.expDrop;
            totalYp += monster.ypDrop;
        }
        GameManager.player.stat.AddExp(totalExp);
        GameManager.player.Wallet.AddYP(totalYp);

        yield return new WaitForSeconds(1f);

        rewardUI.ShowWinUI(dropItems, totalExp, totalYp);
    }

    public IEnumerator LoseRoutine()
    {
        // 패배 브금 재생
        if (loseBGM != null)
            SoundManager.Instance.PlayBGM(loseBGM, loop: false, fadeDuration: 0.5f);

        yield return new WaitForSeconds(1f);

        rewardUI.ShowWinUI(null, 0, 0);
    }

    public void QuitBattle()
    {
        IsBattleActive = false;
        BattleCamera.enabled = false;
        WorldCamera.enabled = true;

        SoundManager.Instance.StopBGM();

        SceneLoader.UnloadScene("BattleScene");
    }
}