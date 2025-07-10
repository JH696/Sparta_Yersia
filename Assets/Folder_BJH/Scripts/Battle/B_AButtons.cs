using UnityEngine;
using UnityEngine.UI;

public class B_AButtons : MonoBehaviour
{
    [Header("캐릭터")]
    [SerializeField] private B_Characters chars;

    [Header("행동 버튼")]
    [SerializeField] private Button attackBtn;
    [SerializeField] private Button skillBtn;
    [SerializeField] private Button itemBtn;
    [SerializeField] private Button RestBtn;
    [SerializeField] private Button RunBtn;

    [Header("반응형 버튼")]
    [SerializeField] private B_DButtons dBtns;

    [Header("타겟 시스템")]
    [SerializeField] private B_TargetSystem targetSystem;

    public void SetActionButton()
    { 
        this.gameObject.SetActive(true);   

        attackBtn.onClick.AddListener(OnAttackButton);
        skillBtn.onClick.AddListener(OnSkillButton);
        itemBtn.onClick.AddListener(OnItemButton);
        RestBtn.onClick.AddListener(OnRestButton);
        RunBtn.onClick.AddListener(OnRunBtn);
    }

    public void OnAttackButton()
    {
        Debug.Log("기본 공격");

        targetSystem.SetBeforeUI(this.gameObject);
        targetSystem.Targeting();
        this.gameObject.SetActive(false);
    }

    public void OnSkillButton()
    {
        Debug.Log("스킬 액션");

        this.gameObject.SetActive(false);

        //dBtns.SetSkillButton();
    }

    public void OnItemButton()
    {
        Debug.Log("아이템 액션");

        this.gameObject.SetActive(false);
    }

    public void OnRestButton()
    {
        Debug.Log("휴식");

        BaseCharacter target = chars.SpotLight.Character;
        target.HealMana(target.MaxMana * 0.1f);

        this.gameObject.SetActive(false);
        chars.ResetSpotLight();
    }

    public void OnRunBtn()
    {
        Debug.Log("도주");

        this.gameObject.SetActive(false);
    }
}
