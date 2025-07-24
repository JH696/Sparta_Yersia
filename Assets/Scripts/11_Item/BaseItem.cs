using UnityEngine;

[System.Serializable]
public struct ItemValue
{
    public EStatType Stat;
    public int Value;
}

// 스킬 데이터 부모 클래스
[System.Serializable]
public abstract class BaseItem : ScriptableObject
{
    [Header("ID / 이름")]
    public string ID;
    public string Name;

    [Header("최대 중첩 수")]
    public int MaxStack;

    [Header("아이콘")]
    public Sprite Icon;

    public abstract E_CategoryType GetCategory();
}