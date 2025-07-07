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

    [Header("타겟팅")]
    [SerializeField] private TargetHandler targetHandler;

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
        targetHandler.SetCharacter(curCharacter);
        targetHandler.SingleTargeting();
    }

    private void OnSkillButton()
    {
    }
    private void OnItemButton()
    {

    }

    private void OnRestButton()
    {

    }

    private void OnRunButton()
    {

    }
}
