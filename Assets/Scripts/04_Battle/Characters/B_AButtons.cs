using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public enum E_ActionType
{
    None,
    Attack,
    Skill,
    Item,
    Rest,
    Run
}

public class B_BattleButtons : MonoBehaviour
{
    [Header("선택한 행동")]
    [SerializeField] private E_ActionType actionType = E_ActionType.None;

    [Header("행동 중인 슬롯")]
    [SerializeField] private B_Slot curSlot;

    [Header("액션 핸들러")]
    [SerializeField] private B_ActionHandler actionHandler;

    [Header("슬롯 매니저")]
    [SerializeField] private B_SlotManager slotManager;

    [Header("행동 버튼 부모")]
    [SerializeField] private GameObject aButtonParent;

    [Header("반응형 버튼 부모")]
    [SerializeField] private GameObject dButtonParent;

    [Header("행동 버튼")]
    [SerializeField] private Button attackBtn;
    [SerializeField] private Button skillBtn;
    [SerializeField] private Button itemBtn;
    [SerializeField] private Button restBtn;
    [SerializeField] private Button runBtn;

    [Header("스킬 / 아이템 버튼")]
    [SerializeField] private List<B_DynamicButton> buttons;
    [SerializeField] private Button returnBtn;

    [Header("승인 / 취소 버튼")]
    [SerializeField] private Button allowBtn;
    [SerializeField] private Button cancelBtn;

    [Header("가이드 텍스트")]
    [SerializeField] private TextMeshProUGUI guideText;

    [Header("선택한 스킬, 아이템")]
    [SerializeField] private SkillStatus selectedSkill;
    [SerializeField] private ItemStatus selectedItem;


    // 읽기용 버튼 리스트
    public List<Button> Buttons
    {
        get
        {
            List<Button> btns = new List<Button>
            {
                attackBtn,
                skillBtn,
                itemBtn,
                restBtn,
                runBtn
            };
            return btns;
        }
    }

    public void Start()
    {
        attackBtn.onClick.AddListener(OnAttackButton);
        skillBtn.onClick.AddListener(OnSkillButton);
        itemBtn.onClick.AddListener(OnItemButton);
        restBtn.onClick.AddListener(OnRestButton);
        runBtn.onClick.AddListener(OnRunBtn);

        returnBtn.onClick.AddListener(OnReturnButton);

        allowBtn.onClick.AddListener(OnAllowButton);
        cancelBtn.onClick.AddListener(OnCancelButton);
    }

    public void OnTurnStart(B_Slot slot)
    {
        curSlot = slot;
        Canvas canvas = GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        RectTransform myRect = GetComponent<RectTransform>();

        // 월드 좌표 → 화면 좌표
        Vector2 screenPos = BattleManager.Instance.BattleCamera.WorldToScreenPoint(slot.gameObject.transform.position);

        // 사용할 카메라: Render Mode가 Overlay면 null, 아니면 canvas의 worldCamera 사용
        Camera renderCam = (canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : canvas.worldCamera;

        // 화면 좌표 → 캔버스 로컬 좌표
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, renderCam, out Vector2 localPoint))
        {
            myRect.localPosition = localPoint;
        }

        aButtonParent.SetActive(true);
        aButtonParent.GetComponent<Animator>().SetTrigger("Scatter");

        UpdateText();
    }

    public void OnTurnEnd()
    {
        actionType = E_ActionType.None;
        selectedSkill = null;
        selectedItem = null;

        foreach (var btn in buttons)
        {
            btn.OnSkillSelected -= UseSkill;
            btn.OnItemSelected -= UseItem;
            btn.ResetButton();
        }

        if (curSlot.Character.IsDead)
        {
            actionHandler.EndTargeting(true);
            return;
        }

        actionHandler.EndTargeting(false);
        curSlot = null;

        UpdateText();
    }

    public void OnMonsterturn(B_Slot slot)
    {
        curSlot = slot;

        CharacterStatus curStatus = slot.Character;

        List<BattleEffecter> effecters = slotManager.GetNonEmptySlots()
            .Where(slot => slot.GetSlotType() == E_B_SlotType.Ally)
            .Select(slot => slot.GetComponentInChildren<BattleEffecter>())
            .Where(effecter => effecter != null) // ← 이거 추가
            .ToList();

        if (effecters.Count == 0)
        {
            slotManager.ClearCurrentSlot();
            return;
        }

        List<SkillStatus> skills = curStatus.skills.AllSkills;
        List<SkillStatus> castableSkills = skills
            .Where(skill => skill.CanCast(curStatus))
            .ToList();

        List<BattleEffecter> randomTargets = new List<BattleEffecter>();

        if (castableSkills.Count <= 0 || Random.value <= 0.75f)
        {
            BattleEffecter target = effecters[Random.Range(0, effecters.Count)];

            target.SetBaseEffect(curStatus.stat, this);
            Debug.Log($"몬스터 {curStatus}이(가) {target}에게 일반 공격을 실행 했습니다.");
        }
        else
        {
            SkillStatus randomSkill = castableSkills[Random.Range(0, castableSkills.Count)];

            int range = Mathf.Min(randomSkill.Data.Range, effecters.Count);

            List<int> indices = Enumerable.Range(0, effecters.Count).ToList();
            for (int i = 0; i < range; i++)
            {
                int rand = Random.Range(i, indices.Count);
                (indices[i], indices[rand]) = (indices[rand], indices[i]);
                randomTargets.Add(effecters[indices[i]]);
            }

            randomSkill.Cast(curStatus);

            foreach (BattleEffecter e in randomTargets)
            {
                e.SetSkillEffecter(curStatus.stat, randomSkill, this);
                Debug.Log($"몬스터 {curStatus}이(가) {e}에게 {randomSkill.Data.name}을 사용했습니다.");
            }
        }

        slot.PlayAttackAnim();
    }

   public void OnAttackButton()
   {
        Debug.Log("기본 공격");
        actionType = E_ActionType.Attack;
        actionHandler.StartTargeting(1);
        ShowAllowButton();
        UpdateText();
    }

   public void OnSkillButton()
   {
        Debug.Log("스킬 액션");

        foreach (var btn in buttons)
        {
            btn.OnSkillSelected -= UseSkill;
        }

        List<SkillStatus> skills = curSlot.Character.skills.EquipSkills;

        actionType = E_ActionType.Skill;

        dButtonParent.SetActive(true);
        aButtonParent.SetActive(false);
        UpdateText();

        if (skills.Count <= 0) return;

        for (int i = 0; i < skills.Count; i++) // 장착 스킬 카운터로 변경
        {
            buttons[i].SetSkill(skills[i]);
            buttons[i].OnSkillSelected += UseSkill;
        }
    }

    public void OnItemButton()
    {
        Debug.Log("아이템 액션");

        foreach (var btn in buttons)
        {
            btn.OnItemSelected -= UseItem;
        }

        List<ItemStatus> items = GameManager.player.inventory.Items;
        List<ItemStatus> filter = items.FindAll(item => item.Data.GetCategory() == E_CategoryType.Consume);

        actionType = E_ActionType.Item;
        dButtonParent.SetActive(true);
        aButtonParent.SetActive(false);
        UpdateText();

        if (filter.Count <= 0) return;
        
        for (int i = 0; i < filter.Count; i++)
        {
            buttons[i].SetItem(filter[i]);
            buttons[i].OnItemSelected += UseItem;
        }
    }

    private void UseSkill(SkillStatus status)
    {
        if (curSlot.Character.stat.CurrentMana < status.Data.Cost) return;

        selectedSkill = status;
        actionType = E_ActionType.Skill;
        actionHandler.StartTargeting(status.Data.Range);
        ShowAllowButton();
    }

    public void UseItem(ItemStatus status) // 작업 필요
    {
        if (status.Stack <= 0) return;

        selectedItem = status;
        actionType = E_ActionType.Item;
        actionHandler.StartTargeting(Mathf.Clamp(status.Stack, 1, 3));
        ShowAllowButton();
    }

    private void ShowAllowButton()
    {
        aButtonParent.SetActive(false);
        dButtonParent.SetActive(false);

        allowBtn.interactable = true;
        cancelBtn.interactable = true;
    }

    public void OnRestButton()
    {
        actionType = E_ActionType.Rest;
        ShowAllowButton();
        UpdateText();
    }

    public void OnRunBtn()
    {
        actionType = E_ActionType.Run;
        ShowAllowButton();
        UpdateText();
    }

    private void OnAllowButton()
    {
        if (actionType == E_ActionType.Attack || actionType == E_ActionType.Skill || actionType == E_ActionType.Item)
        {
            if (actionHandler.Targets.Count <= 0)
            {
                return;
            }
        }

        CharacterStats stats = curSlot.Character.stat;

        switch (actionType)
        {
            case E_ActionType.Attack:
                foreach (BattleEffecter effecter in actionHandler.Targets)
                {
                    if (actionHandler.Targets.Count <= 0) return;

                    effecter.SetBaseEffect(curSlot.Character.stat, this);
                }
                break;
 
            case E_ActionType.Skill:
                selectedSkill.Cast(curSlot.Character);
                foreach (BattleEffecter effecter in actionHandler.Targets)
                {
                    if (actionHandler.Targets.Count <= 0) return;

                    effecter.SetSkillEffecter(curSlot.Character.stat, selectedSkill, this);
                }
                break;

            case E_ActionType.Item:
                foreach (BattleEffecter effecter in actionHandler.Targets)
                {
                    if (actionHandler.Targets.Count <= 0) return;

                    if (selectedItem.Data is ConsumeItemData itemData)
                    {
                        itemData.Consume(effecter.Slot.Character);
                    }
                    selectedItem.LoseItem(actionHandler.Targets.Count);
                }

                OnTurnEnd();
                break;

            case E_ActionType.Rest:
                curSlot.Character.RecoverMana(stats.MaxMana * 0.1f);
                OnTurnEnd();
                break;

            case E_ActionType.Run:
                float roll = Random.Range(0f, 100f);
                if (roll <= stats.Luck)
                {
                    BattleManager.Instance.Lose();
                    allowBtn.interactable = false;
                    cancelBtn.interactable = false;  
                    Debug.Log("도망 성공");
                    return;
                }
                else
                {
                    Debug.Log("도망 실패");
                }
                OnTurnEnd();
                break;

        }

        actionHandler.ClearAllTargetsPointer();
        actionType = E_ActionType.None;
        UpdateText();

        allowBtn.interactable = false;
        cancelBtn.interactable = false;
    }

    private void OnCancelButton()
    {
        if (actionType == E_ActionType.Skill || actionType == E_ActionType.Item)
        {
            dButtonParent.SetActive(true);
        }
        else
        {
            aButtonParent.SetActive(true);  
        }

        actionHandler.ClearAllTargetsPointer();
        actionType = E_ActionType.None;
        UpdateText();

        allowBtn.interactable = false;
        cancelBtn.interactable = false;
    }

    private void OnReturnButton()
    {
        foreach (B_DynamicButton button in buttons)
        {
            button.ResetButton();
        }

        dButtonParent.SetActive(false);
        aButtonParent.SetActive(true);

        actionType = E_ActionType.None;
        UpdateText();
    }

    private void UpdateText()
    {
        switch (actionType)
        {
            case E_ActionType.Attack:
                guideText.text = "- 일반 공격 -" + "\n선택한 대상 하나에게 물리 피해를 입힙니다.";
                break;
            case E_ActionType.Skill:
                guideText.text = "- 마법 사용 -" + "\n행동자가 사용 가능한 마법을 나열합니다.";
                break;
            case E_ActionType.Item:
                guideText.text = "- 아이템 사용 -" + "\n행동자가 사용 가능한 소비 아이템을 나열합니다.";
                break;
            case E_ActionType.Rest:
                guideText.text = "- 휴식하기 -" + "\n행동자의 마나를 일정 비율 회복합니다.";
                break;
            case E_ActionType.Run:
                guideText.text = "- 도망가기 -" + "\n행동자의 행운과 비례하는 확률로 도주를 시도합니다.";
                break;
            default:
                guideText.text = string.Empty;
                break;
        }
    }
}
