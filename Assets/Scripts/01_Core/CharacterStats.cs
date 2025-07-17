using UnityEngine;

[System.Serializable]
public class CharacterStats // 예외처리 추가 프로퍼티 추가
{
    [SerializeField] private int level;
    [SerializeField] private int exp;
    [SerializeField] private int maxExp;

    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;

    [SerializeField] private float maxMana;
    [SerializeField] private float currentMana;

    [SerializeField] private float attack;
    [SerializeField] private float defense;
    [SerializeField] private float luck;
    [SerializeField] private float speed;


    public event System.Action LevelUP; // 예: 플레이어 스킬 포인트 획득, 펫 스프라이트 변화
    public event System.Action StatusChanged; // UI 연결

    public int Level
    {
        get => level;
        private set => level = Mathf.Max(0, value);
    }

    public int Exp
    {
        get => exp;
        private set => exp = Mathf.Max(0, value);
    }

    public int MaxExp
    {
        get => maxExp;
        set
        {
            maxExp = Mathf.Max(1, value); // 최소 1
            StatusChanged?.Invoke();
        }
    }

    public float MaxHp
    {
        get => maxHp;
        private set => maxHp = Mathf.Max(1f, value);
    }

    public float CurrentHp
    {
        get => currentHp;
        set
        {
            currentHp = Mathf.Clamp(value, 0, MaxHp);
            StatusChanged?.Invoke();
        }
    }

    public float MaxMana
    {
        get => maxMana;
        private set => maxMana = Mathf.Max(0f, value);
    }

    public float CurrentMana
    {
        get => currentMana;
        set
        {
            currentMana = Mathf.Clamp(value, 0, MaxMana);
            StatusChanged?.Invoke();
        }
    }

    public float Attack
    {
        get => attack;
        private set => attack = Mathf.Max(0f, value);
    }

    public float Defense
    {
        get => defense;
        private set => defense = Mathf.Max(0f, value);
    }

    public float Luck
    {
        get => luck;
        private set => luck = Mathf.Max(0f, value);
    }

    public float Speed
    {
        get => speed;
        private set => speed = Mathf.Max(0f, value);
    }

    public CharacterStats(StatData data)
    {
        if (data == null) return;

        MaxHp = data.maxHp;
        CurrentHp = MaxHp;

        MaxMana = data.maxMana;
        CurrentMana = MaxMana;

        Attack = data.attack;
        Defense = data.defense;
        Luck = data.luck;
        Speed = data.speed;

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

        MaxHp *= multiplier;
        CurrentHp = MaxHp;

        MaxMana *= multiplier;
        CurrentMana = MaxMana;

        Attack *= multiplier;
        Defense *= multiplier;
        Luck *= multiplier;
        Speed *= multiplier;

        LevelUP?.Invoke();
        StatusChanged?.Invoke();
    }

    // 캐릭터 능력치 증가
    public void IncreaseStat(EStatType statType, float amount)
    {
        switch (statType)
        {
            case EStatType.MaxHp:
                MaxHp += Mathf.Max(amount, 0); break;

            case EStatType.MaxMana:
                MaxMana += Mathf.Max(amount, 0); break;

            case EStatType.Attack:
                Attack += Mathf.Max(amount, 0); break;

            case EStatType.Defense:
                Defense += Mathf.Max(amount, 0); break;

            case EStatType.Luck:
                Luck += Mathf.Max(amount, 0); break;

            case EStatType.Speed:
                Speed += Mathf.Max(amount, 0); break;

            default:
                return;
        }
        StatusChanged?.Invoke();
    }

    // 캐릭터 능력치 감소
    public void DecreaseStat(EStatType statType, float amount)
    {
        switch (statType)
        {
            case EStatType.MaxHp:
                MaxHp -= Mathf.Max(amount, MaxHp); break;

            case EStatType.MaxMana:
                MaxMana -= Mathf.Max(amount, MaxMana); break;

            case EStatType.Attack:
                Attack -= Mathf.Max(amount, Attack); break;

            case EStatType.Defense:
                Defense -= Mathf.Max(amount, Defense); break;

            case EStatType.Luck:
                Luck -= Mathf.Max(amount, Luck); break;

            case EStatType.Speed:
                Speed -= Mathf.Max(amount, Speed); break;

            default:
                return;
        }
    }

    // 현재 체력 설정
    public void SetCurrentHp(float amount)
    {
        float hp = CurrentHp + amount;
        CurrentHp = Mathf.Clamp(hp, 0, MaxHp);
    }

    // 현재 마나 설정
    public void SetCurrentMana(float amount)
    {
        float mana = CurrentMana + amount;
        CurrentMana = Mathf.Clamp(mana, 0, MaxMana);
    }
}