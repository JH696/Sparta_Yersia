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

    public void SetBaseStats(StatData data)
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
            Level++;
            LevelUp(1.1f); // 기본 레벨업 배수
        }

        StatusChanged?.Invoke();
    }

    public void LevelUp(float multiplier)
    {
        if (multiplier <= 0f) multiplier = 1.0f;

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

    public void ApplyStat(EStatType type, float value)
    {
        switch (type)
        {
            case EStatType.MaxHp:
                MaxHp += value;
                CurrentHp = Mathf.Clamp(CurrentHp, 0, MaxHp);
                break;
            case EStatType.MaxMana:
                MaxMana += value;
                CurrentMana = Mathf.Clamp(CurrentMana, 0, MaxMana);
                break;
            case EStatType.Attack:
                Attack += value;
                break;
            case EStatType.Defense:
                Defense += value;
                break;
            case EStatType.Luck:
                Luck += value;
                break;
            case EStatType.Speed:
                Speed += value;
                break;
        }
        StatusChanged?.Invoke();
    }

    public void MultiplyStats(float multiplier)
    {
        if (multiplier <= 0f) multiplier = 1f;

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

    public void SetCurrentHp(float hp)
    {
        CurrentHp = Mathf.Clamp(hp, 0, MaxHp);
    }

    public void SetCurrentMana(float mana)
    {
        CurrentHp = Mathf.Clamp(mana, 0, MaxMana);
    }
}