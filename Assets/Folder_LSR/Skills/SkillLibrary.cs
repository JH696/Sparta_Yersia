using System.Collections.Generic;
using UnityEngine;

public class SkillLibrary : MonoBehaviour
{
    public static SkillLibrary Instance { get; private set; }

    [SerializeField] private List<SkillData> allSkills = new List<SkillData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadAllSkills();
    }

    private void LoadAllSkills()
    {
        allSkills.Clear();
        SkillData[] loaded = Resources.LoadAll<SkillData>("SkillDatas");
        allSkills.AddRange(loaded);

        Debug.Log($"[SkillLibrary] Loaded {allSkills.Count} skills from Resources/SkillDatas");
    }

    public List<SkillData> GetSkillsByType(ESkillType type)
    {
        return allSkills.FindAll(s => s.SkillType == type);
    }

    public SkillData GetSkillByID(string id)
    {
        return allSkills.Find(s => s.SkillID == id);
    }
}
