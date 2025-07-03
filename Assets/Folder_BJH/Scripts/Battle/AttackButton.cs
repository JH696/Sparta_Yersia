using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour
{

    [SerializeField] private SkillData skillData;

    public event System.Action OnClick;

    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnAttackBtn);
    }

    public void Setbutton(SkillData skillData)
    {
        this.skillData = skillData; 
    }

    public SkillData GetSkillData()
    {
        return this.skillData;
    }

    public void OnAttackBtn()
    {
        OnClick?.Invoke();
    }
}
