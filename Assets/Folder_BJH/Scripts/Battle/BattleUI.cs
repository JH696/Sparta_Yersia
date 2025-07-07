using UnityEngine;

public class BattleUI : MonoBehaviour
{
    public static BattleUI Instance;

    [SerializeField] private ActionButtons actionButtons;
    [SerializeField] private SkillButtons skillButtons;
    [SerializeField] private ItemButtons itemButtons;

    [SerializeField] private Canvas canvas;
    [SerializeField] private TargetHandler targetHandler;

    [SerializeField] private GameObject before = null;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        targetHandler.SetCharacter(GameManager.Instance.TurnCharacter.turnChar); // 이벤트로 변경
        actionButtons.SetCharacter(GameManager.Instance.TurnCharacter.turnChar); // 이벤트로 변경
        SetActionButton();
    }

    public void SingleTargeting()
    {
        before = actionButtons.gameObject;
        actionButtons.gameObject.SetActive(false);
        targetHandler.SingleTargeting();
    }

    public void SkillTargeting(SkillData skill)
    {
        before = skillButtons.gameObject;
        skillButtons.gameObject.SetActive(false);
        targetHandler.SkillTargeting(skill);
    }

    public void ItemTargeting(ItemData item)
    {
        before = itemButtons.gameObject;
        itemButtons.gameObject.SetActive(false);
        targetHandler.ItemTargeting(item);
    }

    public void ShowActionButtons()
    {
        actionButtons.gameObject.SetActive(true);
    }

    public void GoBack()
    {
        if (before == null) return;
            
        before.SetActive(true);
        before = null;
    }

    public void SetActionButton()
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 screenPos = Camera.main.WorldToScreenPoint(GameManager.Instance.TurnCharacter.gameObject.transform.position);

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos,Camera.main, out localPoint);

        this.GetComponent<RectTransform>().anchoredPosition = localPoint;
    }
}
