using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EActionType
{
    None,
    BaseAttack,
    UseSkill,
    UseItem
}

public class TargetHandler : MonoBehaviour
{
    [Header("현재 행동")]
    [SerializeField] private EActionType curAction = EActionType.None;
    [SerializeField] private BaseCharacter curCharacter;
    [SerializeField] private SkillData curSkill;
    [SerializeField] private ItemData curitem;

    [Header("버튼")]
    [SerializeField] private Button allowButton;
    [SerializeField] private Button cancelButton;

    [Header("타겟 리스트 / 최대 타겟 수 ")]
    [SerializeField] private List<BaseCharacter> targets;
    [SerializeField] private float targetCount;

    public void Start()
    {
        allowButton.onClick.AddListener(OnAllowButton);
        cancelButton.onClick.AddListener(OnCancelButton);
    }

    public void Update()
    {
        Targeting();
    }

    public void SetCharacter(BaseCharacter character)
    {
        curCharacter = character;
    }

    public void SingleTargeting()
    {
        curAction = EActionType.BaseAttack;
        targetCount = 1;
    }

    public void SkillTargeting(SkillData skill)
    {
        curAction = EActionType.UseSkill;
        this.targetCount = skill.Range;
    }

    public void ItemTargeting(ItemData item)
    {
        curAction = EActionType.UseItem;
        targetCount = 1;
    }

    private void Targeting()
    {
        if (curAction == EActionType.None) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                BaseCharacter target = hit.collider.GetComponent<BaseCharacter>();

                AddTargets(target);
            }
        }
    }

    private void AddTargets(BaseCharacter target)
    {
        if (targets.Count < targetCount)
        {
            targets.Add(target);
            Debug.Log($"타겟 추가됨 {target.name}");
        }
        else
        {
            targets.Remove(targets[0]);
            targets.Add(target);
            Debug.Log($"타겟 변경됨 {targets[0].name} -> {target.name}");
        }
    }

    private void OnCancelButton()
    {
        curAction = EActionType.None;
        targetCount = 0;
        curCharacter = null;
        curSkill = null;
        curitem = null;
        targets.Clear();
    }

    public void OnAllowButton()
    {
        foreach (var target in targets)
        {
            if (curAction == EActionType.BaseAttack)
            {
                target.TakeDamage(curCharacter.GainDamage(0));
            }
            else if (curAction == EActionType.UseSkill)
            {
                target.TakeDamage(curCharacter.GainDamage(curSkill.Damage));
            }
            else if (curAction == EActionType.UseItem)
            {
                return;
            }
        }

        OnCancelButton();
    }
}
