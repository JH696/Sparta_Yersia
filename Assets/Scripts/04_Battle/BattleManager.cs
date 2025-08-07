using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] private BattleEncounter currentEncounter;

    [Header("배틀씬 테스트 전용")]
    public bool IsTesting;
    public MonsterData[] datas = new MonsterData[4];

    public BattleEncounter CurrentEncounter => currentEncounter;

    public event System.Action OnBattleStarted;
    public event System.Action OnBattleEnded;
    public Camera BattleCamera => battleCamera;

    //[Header("임시 위치")]
    //public float startSize = 4f;
    //public float endSize = 1f;
    //public float zoomDuration = 0.5f;

    [SerializeField] private AudioClip battleBGM;

    [SerializeField] private AudioClip winBGM;
    [SerializeField] private AudioClip loseBGM;

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
        OnBattleStarted?.Invoke();

        if (IsTesting) yield break;

        // 카메라 전환
        WorldCamera.enabled = false;
        BattleCamera.enabled = true;
        WorldCanvas.SetActive(false);

        SceneLoader.MultipleLoadScene("BattleScene");
        Debug.Log("씬 로드");
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

        yield return new WaitForSeconds(0.5f);

        // 카메라 전환
        WorldCamera.enabled = false;
        BattleCamera.enabled = true;

        // BGM 재생
        if (battleBGM != null)
            SoundManager.Instance.PlayBGM(battleBGM, loop: true, fadeDuration: 1f);

        WorldCanvas.SetActive(false);

        SceneLoader.MultipleLoadScene("BattleScene");
        Debug.Log("씬 로드");
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
        // 승리 브금 재생
        if (winBGM != null)
            SoundManager.Instance.PlayBGM(winBGM, loop: false, fadeDuration: 0.5f);

        List<MonsterData> monsters = CurrentEncounter.Monsters.ToList();
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
        // 패배 브금 재생
        if (loseBGM != null)
            SoundManager.Instance.PlayBGM(loseBGM, loop: false, fadeDuration: 0.5f);

        yield return new WaitForSeconds(1f);

        RewardUI.ShowWinUI(null, 0, 0);
    }

    public void QuitBattle()
    {
        OnBattleEnded?.Invoke();
        BattleCamera.enabled = false;
        WorldCamera.enabled = true;

        WorldCanvas.SetActive(true);

        SoundManager.Instance.StopBGM();

        SceneLoader.UnloadScene("BattleScene");
    }
}