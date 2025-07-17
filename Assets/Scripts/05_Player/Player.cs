using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("플레이어 스테이터스")]
    [SerializeField] private playerStatus status;

    [Header("월드 스프라이트")]
    [SerializeField] private SpriteRenderer worldSR;

    [Header("플레이어 파티")]
    [SerializeField] private PlayerParty party;

    [Header("플레이어 퀘스트")]
    [SerializeField] private PlayerQuest quest;

    [Header("플레이어 인벤토리")]
    [SerializeField] private ItemInventory inventory;

    [Header("플레이어 스킬")]
    [SerializeField] private SkillInventory skills;

    public PlayerParty Party => party;
    public PlayerQuest Quest => quest; // 읽기 전용
    public ItemInventory Inventory => inventory; // 읽기 전용
    public SkillInventory Skills => skills; // 읽기 전용

    public void Init(PlayerParty party, PlayerQuest quest, ItemInventory inventory, SkillInventory skills)
    {
         this.party = party;
         this.quest = quest;
         this.inventory = inventory;
         this.skills = skills;
    }

    public void ChangeSprite()
    {
        // worldSR.sprite = sprite  
    }


    //// YP(돈) 획득 메서드
    //public void AddYP(int amount)
    //{
    //    yp += Mathf.Max(0, amount);
    //}

    //// YP(돈) 소비 메서드
    //public bool SpendYP(int amount)
    //{
    //    if (yp >= amount)
    //    {
    //        yp -= amount;
    //        return true;
    //    }
    //    return false;
    //}

    public void Equip(EquipItemData item)
    {
        Debug.Log($"[Player] Equip: {item.Name}");
        if (item == null) return;

        foreach (var v in item.Values)
        {
            ApplyStat(v.Stat, v.Value);
        }
    }

    public void Unequip(EquipItemData item)
    {
        Debug.Log($"[Player] Unequip: {item.Name}");
        if (item == null) return;

        foreach (var v in item.Values)
        {
            ApplyStat(v.Stat, -v.Value);
        }
    }

    public void Consume(ConsumeItemData item)
    {
        Debug.Log($"[Player] Use: {item.Name}");
        if (item == null) return;

        foreach (var v in item.Values)
        {
            if (v.Stat == EStatType.MaxHp)
                status.RecoverHealth(v.Value);
            else if (v.Stat == EStatType.MaxMana)
                status.RecoverMana(v.Value);
        }
    }

    private void ApplyStat(EStatType type, float value)
    {
        status.stat.IncreaseStat(type, value);
    }
}