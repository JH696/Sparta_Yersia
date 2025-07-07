using UnityEngine;
using UnityEngine.UI;

public class ActionButtons : MonoBehaviour
{
    [SerializeField] private CharacterSlot slot;
    [SerializeField] private Aimer aimer;

    [Header("버튼")]
    [SerializeField] private AttackButton attackBtn;
    [SerializeField] private Button skillBtn;
    [SerializeField] private Button itemBtn;
    [SerializeField] private Button restBtn;
    [SerializeField] private Button runBtn;

    public void Start()
    {
        attackBtn.Setbutton(slot.GetCurSkill().BaseAttack);

        attackBtn.OnClick += BaseAttack;
    }

    public void SetActioner(CharacterSlot slot)
    {
        this.slot = slot;
    }

    public void SetPosition()
    {
        RectTransform canvasRect = Test_BattleManager.Instance.battleUI.canvas.GetComponent<RectTransform>();
        Vector2 screenPos = Camera.main.WorldToScreenPoint(slot.gameObject.transform.position);

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            Camera.main,
            out localPoint
        );

        this.GetComponent<RectTransform>().anchoredPosition = localPoint;
    }

    private void BaseAttack()
    {
        aimer.StartAiming(slot, attackBtn.GetSkillData());
    }
}
