using UnityEngine;
using UnityEngine.UI;

public class ActionButtons : MonoBehaviour
{
    [Header("행동 중인 캐릭터")]
    [SerializeField] private BaseCharacter curCharacter;

    [Header("행동 버튼")]
    [SerializeField] private Button attackButton;
    [SerializeField] private Button skillButton;
    [SerializeField] private Button itemButton;
    [SerializeField] private Button restButton;
    [SerializeField] private Button runButton;

    [Header("스킬 / 아이템 버튼")]
    [SerializeField] private SkillButtons skillButtons;
    [SerializeField] private ItemButtons itemButtons;

   void Start()
    {
        attackButton.onClick.AddListener(OnAttackButton);
        skillButton.onClick.AddListener(OnSkillButton);
        itemButton.onClick.AddListener(OnItemButton);
        restButton.onClick.AddListener(OnRestButton);
        runButton.onClick.AddListener(OnRunButton); 
    }

    public void SetCharacter(BaseCharacter character)
    {
        Debug.Log($"{character}");
        curCharacter = character;
    }

    private void OnAttackButton()
    {
        Debug.Log("일반 공격 중");
        BattleUI.Instance.SingleTargeting();
    }

    private void OnSkillButton()
    {
        Debug.Log("스킬 선택");
        skillButtons.SetSkillButton();
        this.gameObject.SetActive(false);
    }

    private void OnItemButton()
    {
        Debug.Log("아이템 선택");
        itemButtons.SetItemButton();
        this.gameObject.SetActive(false);
    }

    private void OnRestButton()
    {
        Debug.Log("마나 회복");
        curCharacter.HealMana(curCharacter.MaxMana * 0.1f);
    }

    private void OnRunButton()
    {

    }
}
