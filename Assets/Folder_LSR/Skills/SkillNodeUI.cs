using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillNodeUI : MonoBehaviour
{
    [Header("UI 컴포넌트")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Image lockOverlay;
    [SerializeField] private Image fillGauge;
    [SerializeField] private Image effectOverlay;
    [SerializeField] private Button nodeButton;

    private SkillData skillData;
    private PlayerSkillController playerSkillController;
    private SkillTreeUI skillTreeUI;

    private void Awake()
    {
        nodeButton.onClick.AddListener(OnNodeClick);
    }

    public void Setup(SkillTreeUI treeUI, SkillData data, PlayerSkillController controller)
    {
        skillTreeUI = treeUI;
        skillData = data;
        playerSkillController = controller;

        iconImage.sprite = data.Icon;
        UpdateNodeUI();
    }

    private void UpdateNodeUI()
    {
        bool isUnlocked = playerSkillController.HasSkillUnlocked(skillData.SkillID);

        if (isUnlocked)
        {
            lockOverlay.color = new Color(0, 0, 0, 0);
            effectOverlay.gameObject.SetActive(false);
            fillGauge.fillAmount = 1f;
        }
        else if (playerSkillController.PlayerTier >= skillData.TierRequirement)
        {
            // 락 해제 가능 상태
            lockOverlay.color = new Color(0, 0, 0, 0.7f);
            effectOverlay.gameObject.SetActive(true);
            StartCoroutine(AnimateEffect());
            fillGauge.fillAmount = 0f;
        }
        else
        {
            // 락 상태
            lockOverlay.color = new Color(0, 0, 0, 0.7f);
            effectOverlay.gameObject.SetActive(false);
            fillGauge.fillAmount = 0f;
        }
    }

    private IEnumerator AnimateEffect()
    {
        while (effectOverlay != null && effectOverlay.gameObject.activeSelf)
        {
            Color c = effectOverlay.color;
            c.a = Mathf.PingPong(Time.time * 0.5f, 0.3f) + 0.2f;
            effectOverlay.color = c;
            yield return null;
        }
    }

    private void OnNodeClick()
    {
        skillTreeUI.SelectSkill(skillData);
    }
}
