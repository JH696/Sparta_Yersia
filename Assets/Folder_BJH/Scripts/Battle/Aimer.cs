using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aimer : MonoBehaviour
{
    [Header("선택된 스킬")]
    [SerializeField] private BaseCharacter curCharacter;
    [SerializeField] private SkillData curSkill;

    [Header("조준 중 여부")]
    [SerializeField] private bool isAiming = false;

    [Header("최대 타겟 수")]
    [SerializeField] private float maxTargets;

    [Header("조준 타겟")]
    [SerializeField] private List<BaseCharacter> aimTarget;

    [Header("버튼")]
    [SerializeField] private GameObject allowButton;
    [SerializeField] private GameObject cancleButton;


    private void Start()
    {
        allowButton.GetComponent<Button>().onClick.AddListener(AttackTarget);
        cancleButton.GetComponent<Button>().onClick.AddListener(ResetAimer);
    }

    private void Update()
    {
        if (!isAiming) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            BaseCharacter target = hit.collider.GetComponent<BaseCharacter>();

            AddTarget(target);
        }
    }

    public void StartAiming(CharacterSlot slot, SkillData skillData)
    {
        Debug.Log("지정 시작");
        aimTarget.Clear();
        curCharacter = slot.GetCurChar();
        curSkill = skillData;
        this.maxTargets = skillData.Range;
        isAiming = true;
    }

    public void ResetAimer()
    {
        curCharacter = null;
        curSkill = null;
        isAiming = false;
        this.maxTargets = 0;
        aimTarget.Clear();
    }

    public void AddTarget(BaseCharacter target)
    {
        if (aimTarget.Count < maxTargets && !aimTarget.Contains(target))
        {
            Debug.Log($"타겟 추가됨: {target.name}");
            aimTarget.Add(target); 
        }
        else if (aimTarget.Count >= maxTargets && !aimTarget.Contains(target))
        {
            Debug.Log($"타겟 제거됨: {aimTarget[0].name}, 타겟 추가됨: {target.name}");
            aimTarget.Remove(aimTarget[0]);
            aimTarget.Add(target);
        }
        else if (aimTarget.Contains(target))
        {
            Debug.Log($"타겟 제거됨: {target.name}");
            aimTarget.Remove(target);
        }
    }

    public void AttackTarget()
    {
        foreach (BaseCharacter target in aimTarget)
        {
            DamageCalculator damageCal = new DamageCalculator();
            target.TakeDamage(damageCal.DamageCalculate(curCharacter, curSkill));
        }

        ResetAimer();
    }
}
