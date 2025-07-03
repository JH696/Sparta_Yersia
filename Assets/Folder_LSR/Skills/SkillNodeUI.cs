using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillNodeUI : MonoBehaviour
{
    [Header("UI 참조")]
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

    public void UpdateNodeUI()
    {
        bool isUnlocked = playerSkillController.HasSkillUnlocked(skillData.SkillID);

        if (isUnlocked)
        {
            lockOverlay.color = new Color(0, 0, 0, 0);
            effectOverlay.gameObject.SetActive(false);
            StartCoroutine(AnimateUnlock());
            fillGauge.fillAmount = 1f;
        }
        else if (playerSkillController.PlayerTier >= skillData.TierRequirement)
        {
            lockOverlay.color = new Color(0, 0, 0, 0.7f);
            effectOverlay.gameObject.SetActive(true);
            StartCoroutine(AnimateEffect());
        }
        else
        {
            lockOverlay.color = new Color(0, 0, 0, 0.7f);
            effectOverlay.gameObject.SetActive(false);
            fillGauge.fillAmount = 0f;
        }
    }

    public void UpdateFillGauge(float ratio)
    {
        if (fillGauge != null)
        {
            fillGauge.fillAmount = Mathf.Lerp(fillGauge.fillAmount, ratio, Time.deltaTime * 3f);
        }
    }

    private IEnumerator AnimateEffect()
    {
        while (effectOverlay.gameObject.activeSelf)
        {
            Color color = effectOverlay.color;
            color = Color.Lerp(Color.clear, new Color(1, 1, 1, 0.3f), Mathf.PingPong(Time.time, 1f));
            effectOverlay.color = color;
            yield return null;
        }
    }

    private IEnumerator AnimateUnlock()
    {
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime;
            fillGauge.fillAmount = Mathf.Lerp(0, 1, timer);
            yield return null;
        }
        fillGauge.fillAmount = 1;
    }

    private void OnNodeClick()
    {
        skillTreeUI.SelectSkill(skillData);
    }
}
