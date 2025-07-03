using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillLibrary : MonoBehaviour
{
    public static SkillLibrary Instance { get; private set; }

    [Header("스킬 데이터 로드")]
    [SerializeField] private List<SkillData> allSkills = new List<SkillData>();

    public IReadOnlyList<SkillData> AllSkills => allSkills;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// ID로 특정 스킬 조회
    /// </summary>
    public SkillData GetSkillByID(string skillID)
    {
        return allSkills.FirstOrDefault(s => s.SkillID == skillID);
    }

    /// <summary>
    /// 타입별 스킬 리스트 조회
    /// </summary>
    public List<SkillData> GetSkillsByType(ESkillType type)
    {
        return allSkills.Where(s => s.SkillType == type).ToList();
    }
}
