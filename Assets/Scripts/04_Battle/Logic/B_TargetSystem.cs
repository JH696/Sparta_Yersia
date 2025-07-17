//using System.Collections.Generic;
//using System.Linq;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;
//using static UnityEngine.GraphicsBuffer;

//public class B_TargetSystem : MonoBehaviour
//{
//    [Header("캐릭터")]
//    [SerializeField] private B_Characters chars;

//    [Header("사용 스킬")]
//    [SerializeField] private SkillStatus useSkill = null;

//    [Header("사용 아이템")]
//    [SerializeField] private ItemData useItem;

//    [Header("타겟 리스트")]
//    [SerializeField] private List<B_CharacterSlot> aTargets;
//    [SerializeField] private List<B_MonsterSlot> mTargets;

//    [Header("최대 타겟 수")]
//    [SerializeField] private float maxCount;

//    [Header("타겟 추가 중 여부")]
//    [SerializeField] private bool isTargeting;

//    [Header("승인 / 취소 버튼")]
//    [SerializeField] private Button allowBtn;
//    [SerializeField] private Button cancelBtn;

//    [Header("이전 위치")]
//    [SerializeField] private GameObject beforeObj;

//    private void Start()
//    {
//        allowBtn.onClick.AddListener(OnAllowButton);
//        cancelBtn.onClick.AddListener(OnCancelButton);  
//    }


//    private void Update()
//    {
//        if (isTargeting)
//        {
//            if (Input.GetMouseButtonDown(0))
//            {
//                // UI 위 클릭 무시
//                if (EventSystem.current.IsPointerOverGameObject()) return;

//                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

//                if (hit.collider != null)
//                {
//                    B_CharacterSlot aTarget = hit.collider.GetComponent<B_CharacterSlot>();
//                    B_MonsterSlot mTarget = hit.collider.GetComponent<B_MonsterSlot>();

//                    if (aTarget != null)
//                    {
//                        AddAllyTarget(aTarget);
//                    }
//                    else if (mTarget != null)
//                    {
//                        AddEnemyTarget(mTarget);
//                    }
//                    else
//                    {
//                        Debug.Log("타겟이 없습니다");
//                    }
//                }
//            }
//        }
//    }

//    private void AddAllyTarget(B_CharacterSlot target)
//    {
//        // 이미 타겟에 포함된 경우 → 제거
//        if (aTargets.Contains(target))
//        {
//            aTargets.Remove(target);
//            target.ResetPointer();
//            return;
//        }

//        // 타겟 수가 제한 미만이면 → 추가
//        if (aTargets.Count + mTargets.Count < maxCount)
//        {
//            aTargets.Add(target);
//            target.SetPointer();
//            return;
//        }

//        if (aTargets.Count != 0)
//        {
//            // 타겟 수가 제한 이상이면 → 가장 오래된 타겟 교체
//            aTargets[0].ResetPointer();
//            aTargets.RemoveAt(0);
//            aTargets.Add(target);
//            target.SetPointer();
//        }
//    }

//    private void AddEnemyTarget(B_MonsterSlot target)
//    {
//        // 이미 타겟에 포함된 경우 → 제거
//        if (mTargets.Contains(target))
//        {
//            mTargets.Remove(target);
//            target.ResetPointer();
//            return;
//        }

//        // 타겟 수가 제한 미만이면 → 추가
//        if (mTargets.Count + aTargets.Count < maxCount)
//        {
//            mTargets.Add(target);
//            target.SetPointer();
//            return;
//        }

//        if (mTargets.Count != 0)
//        {
//            // 타겟 수가 제한 이상이면 → 가장 오래된 타겟 교체
//            mTargets[0].ResetPointer();
//            mTargets.RemoveAt(0);
//            mTargets.Add(target);
//            target.SetPointer();
//        }
//    }
    
//    public void SkillTargeting(SkillStatus skill, GameObject gameObject)
//    {
//        if (skill.Data.ManaCost > chars.SpotLight.Character.CurrentMana) return;

//        gameObject.SetActive(false);
//        beforeObj = gameObject;

//        isTargeting = true;
//        useSkill = skill;
//        maxCount = skill.Data.Range;
//        cancelBtn.gameObject.SetActive(true);
//        allowBtn.gameObject.SetActive(true);
//    }

//    public void ItemTargeting(ItemData item, GameObject gameObject)
//    {
//        gameObject.SetActive(false);
//        beforeObj = gameObject;

//        isTargeting = true;
//        useItem = item; 
//        maxCount = 1;
//        cancelBtn.gameObject.SetActive(true);
//        allowBtn.gameObject.SetActive(true);
//    }

//    public void Targeting(GameObject gameObject)
//    {
//        gameObject.SetActive(false);
//        beforeObj = gameObject;

//        isTargeting = true;
//        maxCount = 1;
//        cancelBtn.gameObject.SetActive(true);
//        allowBtn.gameObject.SetActive(true);
//    }

//    private void OnAllowButton()
//    {
//        if (aTargets.Count == 0 && mTargets.Count == 0)
//        {
//            Debug.Log("지정된 대상이 없습니다.");
//            return;
//        }

//        B_CharacterSlot slot = chars.SpotLight;

//        List<CharacterStatus> targets = new List<CharacterStatus>();

//        targets.AddRange(aTargets
//            .Where(slot => slot.Character != null)
//            .Select(slot => slot.Character));

//        targets.AddRange(mTargets
//            .Where(slot => slot.Monster != null)
//            .Select(slot => slot.Monster));

//        DamageCalculator cal = new DamageCalculator();

//        if (useSkill != null)
//        {
//            slot.Character.HealMana(-useSkill.Data.ManaCost);
//            useSkill.Use();

//            foreach (CharacterStatus target in targets)
//            {
//                target.TakeDamage(cal.DamageCalculate(slot.Character, target, useSkill.Data));
//            }
//        }
//        else if (useItem != null)
//        {
//            GameManager.Instance.Player.GetComponent<Player>().Inventory.RemoveItem(useItem);

//            foreach (CharacterStatus target in targets)
//            {
//                foreach (var item in useItem.ItemStats)
//                {
//                    switch (item.eStat)
//                    {
//                        case EStatType.MaxHp:
//                            target.HealHP(item.value);
//                            break;
//                        case EStatType.MaxMana:
//                            target.HealMana(item.value);
//                            break;

//                    }
//                }
//            }

//        }
//        else
//        {
//            foreach (CharacterStatus target in targets)
//            {
//                target.TakeDamage(cal.DamageCalculate(slot.Character, target, null));
//            }
//        }

//        chars.ResetSpotLight();
//        ResetTargeting();
//    }

//    private void ResetTargeting()
//    {
//        isTargeting = false;
//        useSkill = null;
//        useItem = null;
//        beforeObj = null;
//        maxCount = 0;

//        foreach (B_CharacterSlot target in aTargets)
//        {
//            target.ResetPointer();
//        }

//        foreach (B_MonsterSlot target in mTargets)
//        {
//            target.ResetPointer();
//        }

//        aTargets.Clear();
//        mTargets.Clear();
//        cancelBtn.gameObject.SetActive(false);
//        allowBtn.gameObject.SetActive(false);
//    }

//    private void OnCancelButton()
//    {
//        beforeObj.SetActive(true);
//        ResetTargeting();
//    }
//}
