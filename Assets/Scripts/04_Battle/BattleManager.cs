using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    [Header("배틀, 월드 씬 관리")]
    [SerializeField] private Camera battleCamera;
    [SerializeField] private Camera WorldCamera;
    [SerializeField] private GameObject WorldCanvas;

    public GameObject player;

    [Header("리워드 UI (자동 참조)")]
    public B_RewardUI RewardUI;

    public Vector2 hospital;

    [SerializeField] private BattleEncounter currentEncounter;

    [Header("배틀씬 테스트 전용")]
    public bool IsTesting;
    public MonsterData[] datas = new MonsterData[4];

    [Header("해당 포탈로 통하는 방의 카메라 Bounds")]
    [SerializeField] private PolygonCollider2D roomBounds;

    public BattleEncounter CurrentEncounter => currentEncounter;

    public event System.Action OnBattleStarted;
    public event System.Action<bool> OnBattleEnded;
    public Camera BattleCamera => battleCamera;

    [Header("사운드")]
    [SerializeField] private AudioClip battleBGM;
    [SerializeField] private AudioClip winBGM;
    [SerializeField] private AudioClip loseBGM;

    private enum BattleOutcome { None, Victory, Defeat }
    private BattleOutcome lastOutcome = BattleOutcome.None;
    private E_StageType lastEncounterStage = E_StageType.None;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (!IsTesting) return;

        currentEncounter = new BattleEncounter(datas, E_StageType.Upper);
    }

    public IEnumerator StartBattle(BattleEncounter encounter, GameObject player)
    {
        this.player = player ?? gameObject;
        currentEncounter = encounter;
        OnBattleStarted?.Invoke();

        if (IsTesting) yield break;

        StartCoroutine(BattleDelay());
    }

    private IEnumerator BattleDelay()
    {
        // 1. 페이드 인
        yield return FadeScreen.Instance.FadeIn();

        LightManager.Instance?.OnBattleStartSnapshot();

        // 2. 배틀씬 로드
        SceneLoader.MultipleLoadScene("BattleScene");

        // 3. 카메라/UI 전환
        WorldCamera.enabled = false;
        BattleCamera.enabled = true;

        if (battleBGM != null)
            SoundManager.Instance.PlayBGM(battleBGM, loop: true, fadeDuration: 1f);

        WorldCanvas.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        // 4. 페이드 아웃
        yield return FadeScreen.Instance.FadeOut();
    }

    public void Win()
    {
        lastOutcome = BattleOutcome.Victory;
        StartCoroutine(WinRoutine());
    }

    public void Lose()
    {
        lastOutcome = BattleOutcome.Defeat;
        StartCoroutine(LoseRoutine());
    }

    private IEnumerator WinRoutine()
    {
        if (winBGM != null)
            SoundManager.Instance.PlayBGM(winBGM, loop: false, fadeDuration: 0.5f);

        var monsters = CurrentEncounter.Monsters.ToList();
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

        foreach (PetStatus pet in GameManager.player.party.partyPets)
        {
            pet.stat.AddExp(totalExp);
        }

        yield return new WaitForSeconds(1f);

        RewardUI.ShowWinUI(dropItems, totalExp, totalYp);
    }

    private IEnumerator LoseRoutine()
    {
        if (loseBGM != null)
            SoundManager.Instance.PlayBGM(loseBGM, loop: false, fadeDuration: 0.5f);

        yield return new WaitForSeconds(1f);

        // 패배 UI 재사용 중이면 유지
        RewardUI.ShowWinUI(null, 0, 0);
    }

    public void QuitBattle(bool isWin)
    {
        if (!isWin)
        {
            var vcam = FindObjectOfType<CinemachineVirtualCamera>();
            var confiner = vcam.GetComponent<CinemachineConfiner2D>();
            PlayerStatus playerStatus = GameManager.player;

            Vector2 vec = hospital;
            vec.y -= 0.5f;
            player.transform.position = vec;

            Vector3 oldPos = player.transform.position;
            confiner.m_BoundingShape2D = roomBounds;
            confiner.InvalidateCache();

            Vector3 displacement = player.transform.position - oldPos;
            vcam.OnTargetObjectWarped(player.transform, displacement);

            player.transform.position = hospital;
            playerStatus.Revive();

            List<PetStatus> pets = playerStatus.party.curPets;
            foreach (PetStatus pet in pets)
            {
                pet.Revive();
            }
        }

        OnBattleEnded?.Invoke(isWin);

        BattleCamera.enabled = false;
        WorldCamera.enabled = true;
        WorldCanvas.SetActive(true);
        SoundManager.Instance.StopBGM();
        SceneLoader.UnloadScene("BattleScene");

        // 조명 복귀/적용은 LightManager가 전담
        var stage = lastEncounterStage;
        LightManager.Instance?.OnBattleEnd(isWin, stage);

        lastOutcome = BattleOutcome.None;
    }

    public void SetEncounterStage(E_StageType stage)
    {
        lastEncounterStage = stage;
    }
}
