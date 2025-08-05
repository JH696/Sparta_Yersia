using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    [Header("배틀, 월드 씬 관리")]
    [SerializeField] private Camera battleCamera;
    [SerializeField] private Camera WorldCamera;
    [SerializeField] private GameObject WorldCanvas;

    [Header("리워드 UI (자동 참조)")]
    public B_RewardUI RewardUI;

    [Header("현재 전투 구성")]
    [SerializeField] private BattleEncounter currentEncounter;

    [Header("전투 중 여부")]
    [SerializeField] private bool isBattleActive = false;

    [Header("배틀씬 테스트 전용")]
    public bool IsTesting;
    public List<MonsterData> datas = new List<MonsterData>();

    public BattleEncounter CurrentEncounter => currentEncounter;
    public bool IsBattleActive => isBattleActive;
    public Camera BattleCamera => battleCamera;

    //[Header("임시 위치")]
    //public float startSize = 4f;
    //public float endSize = 1f;
    //public float zoomDuration = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 이미 존재하는 경우 중복 방지
        }

        // 테스트용 코드 삭제 예정
        if (!IsTesting) return;

        currentEncounter = new BattleEncounter(datas, E_StageType.Upper);
    }

    public IEnumerator StartBattle(BattleEncounter encounter)
    {
        currentEncounter = encounter;
        isBattleActive = true;

        if (IsTesting) yield break;

        yield return BattleDelay();
    }

    private IEnumerator BattleDelay()
    {
        //WorldCamera.orthographic = true;
        //WorldCamera.orthographicSize = startSize;

        //float elapsed = 0f;

        //while (elapsed < zoomDuration)
        //{
        //    elapsed += Time.deltaTime;
        //    float t = elapsed / zoomDuration;
        //    WorldCamera.orthographicSize = Mathf.Lerp(startSize, endSize, t);
        //    yield return null;
        //}

        //WorldCamera.orthographicSize = endSize;

        yield return new WaitForSeconds(0f);

        // 카메라 전환
        WorldCamera.enabled = false;
        BattleCamera.enabled = true;
        WorldCanvas.SetActive(false);
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

    private IEnumerator WinRoutine()
    {
        List<MonsterData> monsters = CurrentEncounter.Monsters;
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

        RewardUI.ShowWinUI(dropItems, totalExp, totalYp);
    }

    private IEnumerator LoseRoutine()
    {
        yield return new WaitForSeconds(1f);

        RewardUI.ShowWinUI(null, 0, 0);
    }

    public void QuitBattle()
    {
        isBattleActive = false;
        BattleCamera.enabled = false;
        WorldCamera.enabled = true;
        WorldCanvas.SetActive(true);

        SceneLoader.UnloadScene("BattleScene");
    }
}