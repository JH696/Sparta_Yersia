using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    [SerializeField] private SkillSlot previousSlot;

    [SerializeField] private SkillData data;

    [SerializeField] private Image icon;

    [SerializeField] private Image lockIcon;

    public SkillData Data => data;

    public void Start()
    {
        if (data == null)
        {
            icon.enabled = false; // 아이콘 비활성화
            return;
        }

        GameManager.player.skills.OnChanged += RefreshSlot;

        RefreshSlot();
    }

    public void RefreshSlot()
    {
        SkillInventory sInventory = GameManager.player.skills;
        icon.sprite = data.Icon;

        if (previousSlot == null)
        {
            lockIcon.enabled = false;

            if (sInventory.HasSkill(data))
            {
                icon.color = Color.white; // 스킬이 있는 경우 아이콘을 보이게 함
            }
            else
            {
                icon.color = new Color(0.25f, 0.25f, 0.25f);
            }

            return;
        }

        if (!IsLocked())
        {
            lockIcon.enabled = false;

            if (sInventory.HasSkill(data))
            {
                icon.color = Color.white; // 스킬이 있는 경우 아이콘을 보이게 함
            }
            else
            {
                icon.color = new Color(0.25f, 0.25f, 0.25f);
            }
        }
        else
        {
            lockIcon.enabled = true;
            icon.color = new Color(0.25f, 0.25f, 0.25f);
        }
    }

    public bool IsLocked()
    {
        if (previousSlot == null) return false;

        PlayerStatus player = GameManager.player;

        if (!player.skills.HasSkill(previousSlot.Data) || player.Rank < data.Rank)
        {
            return true;
        }

        return false;
    }
}
