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

    public void SetMaxHp(float value)
    {
        MaxHp = Mathf.Max(1f, value);
        CurrentHp = Mathf.Min(CurrentHp, MaxHp);
    }

    public void SetCurrentHp(float value)
    {
        CurrentHp = Mathf.Clamp(value, 0f, MaxHp);
    }

    public void SetMaxMp(float value)
    {
        MaxMp = Mathf.Max(0f, value);
        CurrentMp = Mathf.Min(CurrentMp, MaxMp);
    }

    public void SetCurrentMp(float value)
    {
        CurrentMp = Mathf.Clamp(value, 0f, MaxMp);
    }
}
