using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    [SerializeField] private SkillInventory sInventory; 

    [SerializeField] private SkillData data;

    [SerializeField] private Button learnButton;

    [SerializeField] private Image icon;

    public void Start()
    {
        SetSlot(GameManager.player.skills);
    }

    public void SetSlot(SkillInventory sInventory)
    {
        icon.sprite = data.Icon;
        this.sInventory = sInventory;
        sInventory.OnChanged += RefreshSlot;
        RefreshSlot();
    }


    public void RefreshSlot()
    {
        learnButton.onClick.RemoveAllListeners();

        if (GameManager.player.skills.HasSkill(data))
        {
            icon.color = Color.white; // 스킬이 있는 경우 아이콘을 보이게 함
            learnButton.onClick.AddListener(() => sInventory.LevelUpSkill(data)); // 스킬 레벨업 버튼 설정
        }
        else
        {
            icon.color = Color.gray; // 스킬이 없는 경우 아이콘을 회색으로 표시
            learnButton.onClick.AddListener(LearnSKill); // 스킬 학습 버튼 설정
        }
    }

    public void LearnSKill()
    {
        //GameManager.player.skillPoint--;

        sInventory.AddSkill(data);
    }

}
