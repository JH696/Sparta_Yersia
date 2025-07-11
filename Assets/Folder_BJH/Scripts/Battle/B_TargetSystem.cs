using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class B_TargetSystem : MonoBehaviour
{
    [Header("캐릭터")]
    [SerializeField] private B_Characters chars;

    [Header("사용 스킬")]
    [SerializeField] private SkillStatus useSkill = null;

    [Header("사용 아이템")]
    [SerializeField] private ItemData useItem;

    [Header("타겟 리스트")]
    [SerializeField] private List<B_CharacterSlot> targets;

    [Header("최대 타겟 수")]
    [SerializeField] private float maxCount;

    [Header("타겟 추가 중 여부")]
    [SerializeField] private bool isTargeting;

    [Header("승인 / 취소 버튼")]
    [SerializeField] private Button allowBtn;
    [SerializeField] private Button cancelBtn;

    [Header("이전 위치")]
    [SerializeField] private GameObject beforeObj;

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
                    B_CharacterSlot target = hit.collider.GetComponent<B_CharacterSlot>();

                    if (target.Character != null)
                    {
                        AddTarget(target);
                    }
                    else
                    {
                        Debug.Log("캐릭터가 존재하지 않습니다.");
                    }
                }
                else
                {
                    Debug.Log("클릭한 위치에 콜라이더 없음");
                }
            }
        }
    }

    private void AddTarget(B_CharacterSlot target)
    {
        // 이미 타겟에 포함된 경우 → 제거
        if (targets.Contains(target))
        {
            Debug.Log($"타겟 제거됨: {target.name}");
            targets.Remove(target);
            target.ResetPointer();
            return;
        }

        // 타겟 수가 제한 미만이면 → 추가
        if (targets.Count < maxCount)
        {
            Debug.Log($"타겟 추가됨: {target.name}");
            targets.Add(target);
            target.SetPointer();
            return;
        }

        // 타겟 수가 제한 이상이면 → 가장 오래된 타겟 교체
        Debug.Log($"타겟 변경됨: {targets[0].name} -> {target.name}");
        targets[0].ResetPointer();
        targets.RemoveAt(0);
        targets.Add(target);
        target.SetPointer();
    }

    public void SetBeforeUI(GameObject gameObject)
    {
        gameObject.SetActive(false);
        beforeObj = gameObject;
    }
    
    public void SkillTargeting(SkillStatus skill)
    {
        isTargeting = true;
        useSkill = skill;
        maxCount = skill.Data.Range;
        cancelBtn.gameObject.SetActive(true);
        allowBtn.gameObject.SetActive(true);
    }

    public void ItemTargeting(ItemData item)
    {
        isTargeting = true;
        useItem = item; 
        maxCount = 1;
        cancelBtn.gameObject.SetActive(true);
        allowBtn.gameObject.SetActive(true);
    }

    public void Targeting()
    {
        isTargeting = true;
        maxCount = 1;
        cancelBtn.gameObject.SetActive(true);
        allowBtn.gameObject.SetActive(true);
    }

    private void OnAllowButton()
    {
        if (targets.Count == 0)
        {
            Debug.Log("지정된 대상이 없습니다.");
            return;
        }

        DamageCalculator cal = new DamageCalculator();

        if (useSkill != null)
        {
            foreach (B_CharacterSlot target in targets)
            {
                target.Character.TakeDamage(cal.DamageCalculate(chars.SpotLight.Character, target.Character, useSkill.Data));
                useSkill.Use();
                target.ChangeStatus();
            }
        }
        else if (useItem != null)
        {
            GameManager.Instance.Player.GetComponent<PlayerInventory>().RemoveItem(useItem);

            foreach (B_CharacterSlot target in targets)
            {
                foreach (var item in useItem.ItemStats)
                {
                    switch (item.eStat)
                    {
                        case EStatType.MaxHp:
                            target.Character.HealHP(item.value);
                            break;
                        case EStatType.MaxMana:
                            target.Character.HealMana(item.value);
                            break;

                    }
                }
            }
        }
        else
        {
            foreach (B_CharacterSlot target in targets)
            {
                target.Character.TakeDamage(cal.DamageCalculate(chars.SpotLight.Character, target.Character, null));
                target.ChangeStatus();
            }
        }

        chars.SpotLight.ChangeStatus();
        chars.ResetSpotLight();
        ResetTargeting();
    }

    private void ResetTargeting()
    {
        isTargeting = false;
        useSkill = null;
        useItem = null;
        beforeObj = null;
        maxCount = 0;
        foreach (B_CharacterSlot target in targets)
        {
            target.ResetPointer();
        }
        targets.Clear();
        cancelBtn.gameObject.SetActive(false);
        allowBtn.gameObject.SetActive(false);
    }

    private void OnCancelButton()
    {
        beforeObj.SetActive(true);
        ResetTargeting();
    }
}
