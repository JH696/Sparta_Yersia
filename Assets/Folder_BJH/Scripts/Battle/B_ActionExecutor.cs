using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class B_ActionExecutor : MonoBehaviour
{
    [Header("행동 중인 슬롯")]
    [SerializeField] private B_CharacterSlot spotLight;

    [Header("행동 버튼")]
    [SerializeField] private Button attackBtn;
    [SerializeField] private Button skillBtn;
    [SerializeField] private Button itemBtn;
    [SerializeField] private Button RestBtn;
    [SerializeField] private Button RunBtn;

    [Header("반응형 버튼")]
    [SerializeField] private B_DButtonEditor editor;

    [Header("월드 스페이스 캔버스")]
    [SerializeField] private Canvas canvas;

    [Header("타겟 시스템")]
    [SerializeField] private B_TargetSystem targetSystem;

    public void SetActionButton(B_CharacterSlot slot)
    {
        spotLight = slot;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 screenPos = Camera.main.ScreenToWorldPoint(slot.transform.position);

        Vector2 LocalPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (canvasRect, screenPos, Camera.main, out LocalPoint );

        this.GetComponent<RectTransform>().anchoredPosition = LocalPoint;


        this.gameObject.SetActive(true);

        attackBtn.onClick.AddListener(OnAttackButton);
        skillBtn.onClick.AddListener(OnSkillButton);
        itemBtn.onClick.AddListener(OnItemButton);
        RestBtn.onClick.AddListener(OnRestButton);
        RunBtn.onClick.AddListener(OnRunBtn);
    }

    public void OnAttackButton()
    {
        targetSystem.Targeting(spotLight.GetCharacter());
    }

    public void OnSkillButton()
    {
        editor.SetSkillButton(spotLight.GetCharacter());
    }

    public void OnItemButton()
    {

    }

    public void OnRestButton()
    {

    }

    public void OnRunBtn()
    {

    }

}
