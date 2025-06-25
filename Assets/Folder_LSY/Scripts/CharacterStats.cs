using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    public float MaxHp { get; private set; } = 100f;
    public float CurrentHp { get; private set; } = 100f;

    public float MaxMp { get; private set; } = 50f;
    public float CurrentMp { get; private set; } = 50f;

    public float Attack { get; private set; } = 20f;
    public float Defense { get; private set; } = 10f;
    public float Luck { get; private set; } = 1f;
    public float Speed { get; private set; } = 5f;

    // 스크립터블 오브젝트 데이터를 기반으로 초기화합니다
    public void InitFromData(CharacterStatData data)
    {
        MaxHp = data.maxHp;
        CurrentHp = MaxHp;

        MaxMp = data.maxMp;
        CurrentMp = MaxMp;

        Attack = data.attack;
        Defense = data.defense;
        Luck = data.luck;
        Speed = data.speed;
    }

    public void SetCurrentHp(float value)
    {
        CurrentHp = Mathf.Clamp(value, 0f, MaxHp);
    }

    public void SetCurrentMp(float value)
    {
        CurrentMp = Mathf.Clamp(value, 0f, MaxMp);
    }
}