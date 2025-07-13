using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        attackBtn.onClick.RemoveAllListeners();
        skillBtn.onClick.RemoveAllListeners();
        itemBtn.onClick.RemoveAllListeners();
        RestBtn.onClick.RemoveAllListeners();
        RunBtn.onClick.RemoveAllListeners();

        attackBtn.onClick.AddListener(OnAttackButton);
        skillBtn.onClick.AddListener(OnSkillButton);
        itemBtn.onClick.AddListener(OnItemButton);
        RestBtn.onClick.AddListener(OnRestButton);
        RunBtn.onClick.AddListener(OnRunBtn);
    }

    public void OnAttackButton()
    {
        Debug.Log("기본 공격");

        targetSystem.Targeting(this.gameObject);
    }

    public void OnSkillButton()
    {
        Debug.Log("스킬 액션");

        CharacterSkill characterSkill = chars.SpotLight.GetLearnedSkill();

        List<SkillStatus> skills = characterSkill.AllStatuses;

        if (skills.Count <= 0) return;

        characterSkill.TickAllCooldowns();

        this.gameObject.SetActive(false);

        dBtns.SetSkillButton(skills);
    }

    public void OnItemButton()
    {
        Debug.Log("아이템 액션");

        this.gameObject.SetActive(false);

        dBtns.SetItemButton();
    }

    public void OnRestButton()
    {
        BaseCharacter target = chars.SpotLight.Character;
        target.HealMana(target.MaxMana * 0.1f);

        this.gameObject.SetActive(false);
        chars.ResetSpotLight();
    }

    public void OnRunBtn()
    {
        float roll = Random.Range(0f, 100f);
        if (roll <= chars.SpotLight.Character.Luck)
        {
            SceneManager.LoadSceneAsync("Scene_BJH");
        }

        this.gameObject.SetActive(false);
        chars.ResetSpotLight();
    }
}
