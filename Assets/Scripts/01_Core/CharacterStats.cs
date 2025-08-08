using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    [Header("레벨 관련 능력치")]
    [SerializeField] private int level;
    [SerializeField] private int curExp;
    [SerializeField] private int maxExp;

    [Header("기본 능력치")]
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float maxMana;
    [SerializeField] private float currentMana;
    [SerializeField] private float attack;
    [SerializeField] private float defense;
    [SerializeField] private float luck;
    [SerializeField] private float speed;

    [Header("추가 능력치")]
    [SerializeField] private float bonusHealth = 0;
    [SerializeField] private float bonusMana = 0;
    [SerializeField] private float bonusAttack = 0;
    [SerializeField] private float bonusDefense = 0;
    [SerializeField] private float bonusLuck = 0;
    [SerializeField] private float bonusSpeed = 0;

    public event System.Action LevelUP; // 예: 플레이어 스킬 포인트 획득, 펫 스프라이트 변화
    public event System.Action StatusChanged; // UI 연결

    // 레벨 관련
    public int Level
    {
        get => level;
        private set => level = Mathf.Max(0, value);
    }

    public int Exp
    {
        get => curExp;
        private set => curExp = Mathf.Max(0, value);
    }

    public int MaxExp
    {
        get => maxExp;
        set
        {
            maxExp = Mathf.Max(1, value);
            StatusChanged?.Invoke();
        }
    }

    // 체력 / 마나
    public float MaxHp => Mathf.Max(1f, maxHp + bonusHealth);

    public float CurrentHp
    {
        get => currentHp;
        set
        {
            currentHp = Mathf.Clamp(value, 0, MaxHp);
            StatusChanged?.Invoke();
        }
    }

    public float MaxMana => Mathf.Max(0f, maxMana + bonusMana);

    public float CurrentMana
    {
        get => currentMana;
        set
        {
            currentMana = Mathf.Clamp(value, 0, MaxMana);
            StatusChanged?.Invoke();
        }
    }

    // 공격력 등
    public float Attack => Mathf.Max(0f, attack + bonusAttack);
    public float Defense => Mathf.Max(0f, defense + bonusDefense);
    public float Luck => Mathf.Max(0f, luck + bonusLuck);
    public float Speed => Mathf.Max(0f, speed + bonusSpeed);

    public CharacterStats(StatData data)
    {
        if (data == null) return;

        level = 1;
        curExp = 0;
        maxExp = 100;

        maxHp = data.maxHp;
        CurrentHp = MaxHp;
        maxMana = data.maxMana;
        CurrentMana = MaxMana;

        attack = data.attack;
        defense = data.defense;
        luck = data.luck;
        speed = data.speed;

        StatusChanged?.Invoke();
    }

    public void AddExp(int amount)
    {
        if (amount <= 0) return;

        Exp += amount;

        while (Exp >= MaxExp)
        {
            Exp -= MaxExp;
            LevelUp(1.1f); // 기본 레벨업 배수
        }

        StatusChanged?.Invoke();
    }

    public void LevelUp(float multiplier)
    {
        if (multiplier <= 0f) multiplier = 1.0f;

        Level++;

        maxHp *= multiplier;
        CurrentHp = MaxHp;

        maxMana *= multiplier;
        CurrentMana = MaxMana;

        attack *= multiplier;
        defense *= multiplier;

        MaxExp = Mathf.RoundToInt(MaxExp * multiplier);

        LevelUP?.Invoke();
        StatusChanged?.Invoke();
    }

    // 캐릭터 추가 능력치 증가
    public void IncreaseBonusStat(EStatType statType, float amount)
    {
        switch (statType)
        {
            case EStatType.MaxHp:
                bonusHealth += Mathf.Max(amount, 0); break;

            case EStatType.MaxMana:
                bonusMana += Mathf.Max(amount, 0); break;

            case EStatType.Attack:
                bonusAttack += Mathf.Max(amount, 0); break;

            case EStatType.Defense:
                bonusDefense += Mathf.Max(amount, 0); break;

            case EStatType.Luck:
                bonusLuck += Mathf.Max(amount, 0); break;

            case EStatType.Speed:
                bonusSpeed += Mathf.Max(amount, 0); break;

            default:
                return;
        }
        StatusChanged?.Invoke();
    }

    // 캐릭터 추가 능력치 감소
    public void DecreaseBonusStat(EStatType statType, float amount)
    {
        switch (statType)
        {
            case EStatType.MaxHp:
                bonusHealth -= Mathf.Max(amount, MaxHp); break;

            case EStatType.MaxMana:
                bonusMana -= Mathf.Max(amount, MaxMana); break;

            case EStatType.Attack:
                bonusAttack -= Mathf.Max(amount, Attack); break;

            case EStatType.Defense:
                bonusDefense -= Mathf.Max(amount, Defense); break;

            case EStatType.Luck:
                bonusLuck -= Mathf.Max(amount, Luck); break;

            case EStatType.Speed:
                bonusSpeed -= Mathf.Max(amount, Speed); break;

            default:
                return;
        }
        StatusChanged?.Invoke();
    }

    // 추가 능력치 초기화
    public void ResetBonusStat()
    {
        bonusHealth = 0;
        bonusMana = 0;
        bonusAttack = 0;
        bonusDefense = 0;
        bonusLuck = 0;
        bonusSpeed = 0;
        StatusChanged?.Invoke();
    }

    // 현재 체력 설정
    public void SetCurrentHp(float amount)
    {
        CurrentHp = Mathf.Clamp(amount, 0, MaxHp);
    }

    // 현재 마나 설정
    public void SetCurrentMana(float amount)
    {
        CurrentMana = Mathf.Clamp(amount, 0, MaxMana);
    }

    // 마나 소모
    public bool ReduceMana(float amount)
    {
        if (CurrentMana < amount) return false;

        CurrentMana -= amount;
        return true;
    }

    // 레벨 설정
    public void SetLevel(int level)
    {
        Level = Mathf.Max(1, level);
        StatusChanged?.Invoke();
    }
}