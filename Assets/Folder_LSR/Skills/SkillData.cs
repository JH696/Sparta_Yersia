using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "S_FireBall", menuName = "Data/SkillData")]
public class SkillData : ScriptableObject
{
    [Header("스킬 기본 정보")]
    [SerializeField] private string skillID;
    [SerializeField] private string displayName;
    [SerializeField] private Sprite icon;
    [TextArea]
    [SerializeField] private string description;

    [Header("스킬 설정")]
    [SerializeField] private ESkillType skillType;
    [SerializeField] private ETier tierRequirement;
    [SerializeField] private int maxLevel = 10;
    [SerializeField] private int baseUnlockCost = 1000;
    [SerializeField] private int manaBonusOnUnlock = 5;

    [Header("전투용 스탯")]
    [SerializeField] private float cooldown;
    [SerializeField] private int damage;
    [SerializeField] private int manaCost;
    [SerializeField] private int targetCount;

    [Header("선행 스킬 (확장용)")]
    [SerializeField] private List<string> prerequisites = new List<string>();

    #region Properties
    public string SkillID => skillID;
    public string DisplayName => displayName;
    public Sprite Icon => icon;
    public string Description => description;
    public ESkillType SkillType => skillType;
    public ETier TierRequirement => tierRequirement;
    public int MaxLevel => maxLevel;
    public int BaseUnlockCost => baseUnlockCost;
    public int ManaBonusOnUnlock => manaBonusOnUnlock;
    public float Cooldown => cooldown;
    public int Damage => damage;
    public int ManaCost => manaCost;
    public int TargetCount => targetCount;
    public IReadOnlyList<string> Prerequisites => prerequisites;
    #endregion

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(displayName))
            displayName = name;

        skillID = name;
    }
#endif
}

