using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct BattleReward
{
    public int RewardEXP;
    public int RewardYP;
    public ItemData DropItem;

    public BattleReward(int exp, int yp, ItemData item)
    {
        RewardEXP = exp;
        RewardYP = yp;
        DropItem = item;
    }
}

public class B_Manager : MonoBehaviour
{
    public static B_Manager Instance;

    public int enemyCount = 0;
    public int partyCount = 0; 

    [Header("캐릭터")]
    [SerializeField] private B_Characters chars;

    [Header("배틀 UI")]
    [SerializeField] private BattleUI ui;

    [Header("전투 보상")]
    [SerializeField] private List<BattleReward> BattleRewards = new List<BattleReward>();

    public event System.Action InBattle;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }


    public void UpECount()
    {
        enemyCount++;

        int Count = 0;

        foreach (B_MonsterSlot slot in chars.MSlots)
        {
            if (slot.Monster != null)
            {
                Count++;
            }
        }

        if (enemyCount >= Count) 
        {
            enemyCount = 0;
            BattleWin();
            chars.StopBattle();
        }
    }

    public void UpACount()
    {
        partyCount++;

        int Count = 0;

        foreach (B_CharacterSlot slot in chars.CSlots)
        {
            if (slot.Character != null)
            {
                Count++;
            }
        }

        if (partyCount >= Count)
        {
            Debug.Log("전사");

            partyCount = 0;
            BattleLose();
            chars.StopBattle();
        }
    }


    public void AddBattleRewards(BattleReward battleRewards)
    {
        BattleRewards.Add(battleRewards);

        Debug.Log($"[보상 등록] 현재 총 보상 수: {BattleRewards.Count}");
    }

    private IEnumerator Battle(List<GameObject> monsters)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("BattleScene");

        while (!asyncOperation.isDone) yield return null;
        
        StartBattle(monsters);
    }

    public void EnterBattle(List<GameObject> monsters)
    {
        StartCoroutine(Battle(monsters));
    }

    public void SetCharacters(B_Characters chars)
    {
        this.chars = chars;
    }
    
    public void SetBattleUI(BattleUI ui)
    {
        this.ui = ui;
    }

    private void StartBattle(List<GameObject> monsters)
    {
        chars.SetAllySlots();
        chars.SetEnemySlots(monsters);
        InBattle?.Invoke();
    }

    public void BattleWin()
    {
        List<ItemData> toalItem = new List<ItemData>(); 
        int totalExp = 0;
        int totalYP = 0;

        foreach (BattleReward r in BattleRewards)
        {
            totalExp += r.RewardEXP;
            totalYP += r.RewardYP;
            toalItem.Add(r.DropItem);
        }

        Player player = GameManager.Instance.Player.GetComponent<Player>();

        foreach (GameObject go in player.Party.GetFullPartyMembers())
        {
            go.GetComponent<ILevelable>().AddExp(totalExp);
        }

        player.AddYP(totalYP);
        
        foreach (ItemData item in toalItem)
        {
            player.Inventory.AddItem(item, 1);
        }

        ui.DisplayWinUI(totalExp, totalYP, toalItem);

        BattleRewards.Clear();
    }

    public void BattleLose()
    {
        Debug.Log("전투 패배");
        ui.DisplayLoseUI();
        BattleRewards.Clear();
    }
}
