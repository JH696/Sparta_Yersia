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
    [SerializeField] private ItemData curItem;

    [Header("버튼")]
    [SerializeField] private Button allowButton;
    [SerializeField] private Button cancelButton;

    [Header("타겟 리스트 / 최대 타겟 수")]
    [SerializeField] private List<BaseCharacter> targets;
    [SerializeField] private float targetCount;

    [Header("타겟 포인터")]
    [SerializeField] private TargetPointer tPointer;

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
        ShowButtons();
        curAction = EActionType.BaseAttack;
        targetCount = 1;
    }

    public void SkillTargeting(SkillData skill)
    {
        ShowButtons();
        curAction = EActionType.UseSkill;
        curSkill = skill;
        this.targetCount = skill.Range;
    }

    public void ItemTargeting(ItemData item)
    {
        ShowButtons(); 
        curAction = EActionType.UseItem;
        curItem = item;
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
        if (targets.Count < targetCount && !targets.Contains(target))
        {
            Debug.Log($"타겟 추가됨 {target.name}");
            targets.Add(target);
            tPointer.AddPoint(target.gameObject.transform);
        }
        else if (targets.Count >= targetCount && !targets.Contains(target))
        {
            Debug.Log($"타겟 변경됨 {targets[0].name} -> {target.name}");
            targets.Remove(targets[0]);
            targets.Add(target);
            tPointer.AddPoint(target.gameObject.transform);
        }
        else
        {
            Debug.Log($"타겟 제거됨 {target.name}");
            targets.Remove(targets[0]);
            tPointer.RemovePoint(target.gameObject.transform);
        }
    }

    private void OnCancelButton()
    {
        ResetTraget();
        HideButtons();
        BattleUI.Instance.GoBack();
    }

    private void ResetTraget()
    {
        curAction = EActionType.None;
        targetCount = 0;
        curSkill = null;
        curItem = null;
        targets.Clear();
    }

    private void ShowButtons()
    {
        allowButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);
    }

    private void HideButtons()
    {
        allowButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
    }

    private void OnAllowButton()
    {
        if (targets.Count == 0)
        {
            Debug.Log($"대상을 지정해주세요");
            return;
        }

        foreach (var target in targets)
        {
            if (curAction == EActionType.BaseAttack)
            {
                target.TakeDamage(curCharacter.GainDamage(0));
            }
            else if (curAction == EActionType.UseSkill)
            {
                target.TakeDamage(curCharacter.GainDamage(curSkill.Damage));
              //curCharacter.ReduceMana(curSkill.ManaCost);
            }
            else if (curAction == EActionType.UseItem)
            {
                foreach (var item in curItem.ItemStats)
                {
                    switch (item.eStat)
                    {
                        case EStatType.MaxHp:
                            Debug.Log($"{target.name}의 체력이 {item.value}만큼 회복됨");
                            target.HealHP(item.value);
                            break;
                        case EStatType.MaxMana:
                            Debug.Log($"{target.name}의 마나가 {item.value}만큼 회복됨");
                            target.HealMana(item.value);
                            break;
                    }
                }

            }
        }

        ResetTraget();
        HideButtons();
        BattleUI.Instance.ShowActionButtons();
    }
}
