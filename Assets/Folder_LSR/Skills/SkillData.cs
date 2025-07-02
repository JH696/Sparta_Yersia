using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "S_f01", menuName = "Data/SkillData")]
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

    [Header("선행 스킬 ID (확장성 고려)")]
    [SerializeField] private List<string> prerequisites = new List<string>();

    #region Public Properties
    public string SkillID => skillID;
    public string DisplayName => displayName;
    public Sprite Icon => icon;
    public string Description => description;
    public ESkillType SkillType => skillType;
    public ETier TierRequirement => tierRequirement;
    public int MaxLevel => maxLevel;
    public int BaseUnlockCost => baseUnlockCost;
    public int ManaBonusOnUnlock => manaBonusOnUnlock;
    public IReadOnlyList<string> Prerequisites => prerequisites;
    #endregion
}

