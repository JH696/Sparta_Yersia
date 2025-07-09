using UnityEngine;

public class Player : BaseCharacter, ILevelable
{
    [Header("플레이어 데이터")]
    [SerializeField] private CharacterData playerData;
    public CharacterData PlayerData => playerData; // 읽기 전용

    // 레벨, YP 관련 데이터 인터페이스로 접근
    private ILevelData LevelData => playerData as ILevelData;
    private IYPHolder YPData => playerData as IYPHolder;

    // 성별 (플레이어 전용)
    public EGender Gender { get; private set; } = EGender.Male;

    // 레벨, 경험치
    public int Level { get; private set; } = 1;
    public int CurrentExp { get; private set; } = 0;
    public int ExpToNextLevel => LevelData?.BaseExpToLevelUp * Level ?? 100 * Level;

    [Header("YP(화폐)")]
    private int yp = 0;
    public int YP => yp;

    public void Init()
    {
        if (playerData == null || LevelData == null) return;

        InitStat(playerData);
        Level = LevelData.StartLevel;
        CurrentExp = LevelData.StartExp;
        yp = YPData.StartYP;

        Gender = (playerData as PlayerData)?.gender ?? EGender.Male;
    }

    // 경험치 추가 메서드
    public void AddExp(int amount)
    {
        CurrentExp += amount;
        while (CurrentExp >= ExpToNextLevel)
        {
            CurrentExp -= ExpToNextLevel;
            LevelUp();
        }
    }

    public void LevelUp()
    {
        Level++;
        Debug.Log($"플레이어 레벨업 현재 레벨: {Level}");

        float multiplier = LevelData?.StatMultiplierPerLevel ?? 1.1f;
        Stat.MultiplyStats(multiplier);
    }

    // YP(돈) 획득 메서드
    public void AddYP(int amount)
    {
        yp += Mathf.Max(0, amount);
    }

    // YP(돈) 소비 메서드
    public bool SpendYP(int amount)
    {
        if (yp >= amount)
        {
            yp -= amount;
            return true;
        }
        return false;
    }

    /// <summary>장착했을 때 호출</summary>
    public void Equip(ItemData item)
    {
        Debug.Log($"[Player] Equip: {item.ItemName}");
        if (item == null || item.Category != EItemCategory.Equipment) return;

        foreach (var stat in item.ItemStats)
        {
            ApplyStat(stat.eStat, stat.value);
        }
    }

    /// <summary>해제했을 때 호출</summary>
    public void Unequip(ItemData item)
    {
        Debug.Log($"[Player] Unequip: {item.ItemName}");
        if (item == null || item.Category != EItemCategory.Equipment) return;

        foreach (var stat in item.ItemStats)
        {
            ApplyStat(stat.eStat, -stat.value);
        }
    }

    /// <summary>소모품 사용했을 때 호출</summary>
    public void Use(ItemData item)
    {
        Debug.Log($"[Player] Use: {item.ItemName}");
        if (item == null || item.Category != EItemCategory.Consumable) return;

        foreach (var stat in item.ItemStats)
        {
            if (stat.eStat == EStatType.MaxHp)
                HealHP(stat.value);
            else if (stat.eStat == EStatType.MaxMana)
                HealMana(stat.value);
        }
    }

    /// <summary>퀘스트용 건네주기 호출</summary>
    public void GiveQuestItem(ItemData item)
    {
        Debug.Log($"[Player] GiveQuestItem: {item.ItemName}");
        // TODO: 퀘스트 시스템에 통지
    }

    private void ApplyStat(EStatType type, float value)
    {
        switch (type)
        {
            case EStatType.MaxHp:
                Stat.MaxHp += value;
                Stat.CurrentHp = Mathf.Clamp(Stat.CurrentHp, 0, Stat.MaxHp);
                break;
            case EStatType.MaxMana:
                Stat.MaxMana += value;
                Stat.CurrentMana = Mathf.Clamp(Stat.CurrentMana, 0, Stat.MaxMana);
                break;
            case EStatType.Attack:
                Stat.Attack += value;
                break;
            case EStatType.Defense:
                Stat.Defense += value;
                break;
            case EStatType.Luck:
                Stat.Luck += value;
                break;
            case EStatType.Speed:
                Stat.Speed += value;
                break;
        }
    }
}