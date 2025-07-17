using UnityEngine;

public class Monster : CharacterStatus
{
    [Header("몬스터 데이터")]
    [SerializeField, Tooltip("몬스터의 이름과 ID가 포함된 데이터")]
    private MonsterData monsterData;
    [SerializeField] private CharacterSkill skill;

    //public override Sprite Icon => monsterData.Icon;

    public MonsterData MonsterData => monsterData;
    public CharacterSkill Skill => skill; // 읽기 전용

    private void Awake()
    {
        if (monsterData == null) return;
        //DontDestroyOnLoad(this.gameObject);
        InitStat(monsterData); // 스탯 초기화
        skill.Init(monsterData.startingSkills);
        ApplyMonsterSprite();
    }

    private void ApplyMonsterSprite()
    {
        //if (monsterData == null || monsterData.WorldSprite == null) return;

        //var spriteRenderer = GetComponent<SpriteRenderer>();
        //if (spriteRenderer != null)
        //{
        //    spriteRenderer.sprite = monsterData.WorldSprite;
        //}
    }

    protected override void CharacterDie()
    {
        KillCountUP();
        DropItem();
    }

    private void DropItem()
    {
        BattleReward reward = new BattleReward();

        float totalChance = 0;

        // 총 확률 합산
        foreach (DropItemData i in MonsterData.dropItems)
        {
            totalChance += i.dropChance;
        }

        // 랜덤 값 뽑기
        float roll = Random.Range(0f, totalChance);
        float current = 0f;

        ItemData selectedItem = null;

        // 누적 확률로 아이템 선택
        foreach (DropItemData i in MonsterData.dropItems)
        {
            current += i.dropChance;
            if (roll < current)
            {
                selectedItem = i.itemData;
                break;
            }
        }

        // 보상 세팅
        reward.RewardEXP = MonsterData.expDrop;
        reward.RewardYP = MonsterData.ypDrop;
        reward.DropItem = selectedItem;

        B_Manager.Instance.AddBattleRewards(reward);
    }

    private void KillCountUP()
    {
        GameManager.Instance.Player.GetComponent<Player>().Quest.KillMonster(MonsterData);
    }
}