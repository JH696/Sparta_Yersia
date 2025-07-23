using System.Collections.Generic;
using UnityEngine;
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

    [Header("행동 중인 캐릭터")]
    [SerializeField] private CharacterStatus curStatus;

    [Header("액션 핸들러")]
    [SerializeField] private B_ActionHandler actionHandler;

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
        curStatus = slot.Character;

        Canvas canvas = GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        RectTransform myRect = GetComponent<RectTransform>();

        // 월드 좌표 → 화면 좌표
        Vector2 screenPos = Camera.main.WorldToScreenPoint(slot.gameObject.transform.position);

        // 화면 좌표 → 캔버스 로컬 좌표
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main, out localPoint))
        {
            myRect.localPosition = localPoint;
        }

        aButtonParent.SetActive(true);
    }

    private void OnTurnEnd()
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

        allowBtn.gameObject.SetActive(false);
        cancelBtn.gameObject.SetActive(false);

        if (curStatus.IsDead)
        {
            actionHandler.EndTargeting(true);
            return;
        }

        actionHandler.EndTargeting(false);
    }

   public void OnAttackButton()
   {
        Debug.Log("기본 공격");
        actionType = E_ActionType.Attack;
        actionHandler.StartTargeting(1);
        ShowAllowButton();
   }

   public void OnSkillButton()
   {
        Debug.Log("스킬 액션");

        foreach (var btn in buttons)
        {
            btn.OnSkillSelected -= UseSkill;
        }

        List<SkillStatus> skills = curStatus.skills.LearnSkills;

        if (skills.Count <= 0) return;

        actionType = E_ActionType.Skill;

        dButtonParent.SetActive(true);
        aButtonParent.SetActive(false);

        for (int i = 0; i < 5; i++) // 장착 스킬 카운터로 변경
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

        if (filter.Count <= 0) return;
        
        for (int i = 0; i < filter.Count; i++)
        {
            buttons[i].SetItem(filter[i]);
            buttons[i].OnItemSelected += UseItem;
        }
    }

    private void UseSkill(SkillStatus status)
    {
        if (curStatus.stat.CurrentMana < status.Data.Cost) return;

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

        allowBtn.gameObject.SetActive(true);
        cancelBtn.gameObject.SetActive(true);
    }

    public void OnRestButton()
    {
        actionType = E_ActionType.Rest;
        ShowAllowButton();
    }

    public void OnRunBtn()
    {
        actionType = E_ActionType.Run;
        ShowAllowButton();
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

        DamageCalculator cal = new DamageCalculator();

        switch (actionType)
        {
            case E_ActionType.Attack:
                foreach (CharacterStatus target in actionHandler.Targets)
                {
                    if (actionHandler.Targets.Count <= 0) return;

                    target.TakeDamage(cal.DamageCalculate(curStatus.stat, target.stat, null));
                }
                break;
 
            case E_ActionType.Skill:
                selectedSkill.Cast(curStatus);
                foreach (CharacterStatus target in actionHandler.Targets)
                {
                    if (actionHandler.Targets.Count <= 0) return;

                    target.TakeDamage(cal.DamageCalculate(curStatus.stat, target.stat, selectedSkill));
                }
                break;

            case E_ActionType.Item:
                foreach (CharacterStatus target in actionHandler.Targets)
                {
                    if (actionHandler.Targets.Count <= 0) return;

                    if (selectedItem.Data is ConsumeItemData itemData)
                    {
                        itemData.Consume(target);
                    }
                    selectedItem.LoseItem(actionHandler.Targets.Count);
                }
                break;

            case E_ActionType.Rest:
                curStatus.RecoverMana(curStatus.stat.MaxMana * 0.1f);
                break;

            case E_ActionType.Run:
                float roll = Random.Range(0f, 100f);
                if (roll <= curStatus.stat.Luck)
                {
                    BattleManager.Instance.Lose();
                    allowBtn.gameObject.SetActive(false);
                    cancelBtn.gameObject.SetActive(false);
                    Debug.Log("도망 성공");
                    return;
                }
                else
                {
                    Debug.Log("도망 실패");
                }
                break;

        }

        OnTurnEnd();
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

        allowBtn.gameObject.SetActive(false);
        cancelBtn.gameObject.SetActive(false);
    }

    private void OnReturnButton()
    {
        foreach (B_DynamicButton button in buttons)
        {
            button.ResetButton();
        }

        dButtonParent.SetActive(false);
        aButtonParent.SetActive(true);
    }
}
