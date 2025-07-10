using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class B_TargetSystem : MonoBehaviour
{
    [Header("공격자")]
    [SerializeField] private BaseCharacter character;

    [Header("사용 스킬")]
    [SerializeField] private SkillData useSkill;

    [Header("사용 아이템")]
    [SerializeField] private ItemData useItem;

    [Header("타겟 리스트")]
    [SerializeField] private List<BaseCharacter> targets;

    [Header("최대 타겟 수")]
    [SerializeField] private float maxCount;

    [Header("타겟 추가 중 여부")]
    [SerializeField] private bool isTargeting;

    [Header("승인 / 취소 버튼")]
    [SerializeField] private Button allowBtn;
    [SerializeField] private Button cancelBtn;

    [Header("이전 위치")]
    [SerializeField] private GameObject beforeUI;

    private void Start()
    {
        allowBtn.onClick.AddListener(OnAllowButton);
        cancelBtn.onClick.AddListener(OnCancelButton);  
    }


    private void Update()
    {
        if (isTargeting)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // UI 위 클릭 무시
                if (EventSystem.current.IsPointerOverGameObject()) return;

                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                if (hit.collider != null)
                {
                    BaseCharacter target = hit.collider.GetComponent<B_CharacterSlot>().GetCharacter();

                    AddTarget(target);
                }
                else
                {
                    Debug.Log("클릭한 위치에 콜라이더 없음");
                }
            }
        }
    }

    private void AddTarget(BaseCharacter target)
    {
        // 이미 타겟에 포함된 경우 → 제거
        if (targets.Contains(target))
        {
            Debug.Log($"타겟 제거됨: {target.name}");
            targets.Remove(target);
            return;
        }

        // 타겟 수가 제한 미만이면 → 추가
        if (targets.Count < maxCount)
        {
            Debug.Log($"타겟 추가됨: {target.name}");
            targets.Add(target);
            return;
        }

        // 타겟 수가 제한 이상이면 → 가장 오래된 타겟 교체
        Debug.Log($"타겟 변경됨: {targets[0].name} -> {target.name}");
        targets.RemoveAt(0);
        targets.Add(target);
    }

    public void SetBeforeUI(GameObject gameObject)
    {
        gameObject.SetActive(false);
        beforeUI = gameObject;
    }
    
    public void SkillTargeting(BaseCharacter character, SkillData skill)
    {
        isTargeting = true;
        useSkill = skill;
        //maxCount = skill.Range;
        this.character = character;   
    }

    public void ItemTargeting(BaseCharacter character, ItemData item)
    {
        isTargeting = true;
        useItem = item; 
        maxCount = 1;
        this.character = character;
    }

    public void Targeting(BaseCharacter character)
    {
        isTargeting = true;
        maxCount = 1;
        this.character = character;
    }

    private void OnAllowButton()
    {
        if (useItem == null || isTargeting)
        {
            DamageCalculator cal = new DamageCalculator();

            foreach (BaseCharacter target in targets)
            {
                target.TakeDamage(cal.DamageCalculate(character, target, useSkill));
            }
        }
        else if (useItem != null || isTargeting)
        {
            foreach (BaseCharacter target in targets)
            {
                foreach (var item in useItem.ItemStats)
                {
                    switch (item.eStat)
                    {
                        case EStatType.MaxHp:
                            target.HealHP(item.value);
                            break;
                        case EStatType.MaxMana:
                            target.HealMana(item.value);
                            break;

                    }
                }
            }
        }

        ResetTargeting();
    }

    private void ResetTargeting()
    {
        isTargeting = false;
        character = null;
        useSkill = null;
        useItem = null;
        beforeUI = null;
        maxCount = 0;
        targets.Clear();
        cancelBtn.enabled = false;
        allowBtn.enabled = false;
    }

    private void OnCancelButton()
    {
        ResetTargeting();
        beforeUI.SetActive(true);
    }
}
