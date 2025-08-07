using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillMasteryUI : MonoBehaviour
{
    [Header("확인 중인 스킬 슬롯")]
    [SerializeField] private SkillSlot curSlot;

    [Header("스킬 인벤토리")]
    [SerializeField] private SkillInventory sInventory;

    [Header("스킬 포인트")]
    [SerializeField] private TextMeshProUGUI skillPoints;

    [Header("스킬 정보창")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI power;
    [SerializeField] private TextMeshProUGUI range;
    [SerializeField] private TextMeshProUGUI cooldown;
    [SerializeField] private TextMeshProUGUI cost;

    [Header("버튼")]
    [SerializeField] private Button learnBtn;
    [SerializeField] private Button forgetBtn;
    [SerializeField] private TextMeshProUGUI learnBtnText;
    [SerializeField] private TextMeshProUGUI forgetBtnText;

    [Header("스킬 숙련도 UI")]
    [SerializeField] private GameObject fireMastery;
    [SerializeField] private GameObject iceMastery;
    [SerializeField] private GameObject natureMastery;
    [SerializeField] private GameObject physicalMastery;

    [Header("스크롤 뷰")]
    [SerializeField] private ScrollRect scrollRect;

    private Vector2 initialPosition;

    public void Start()
    {
        sInventory = GameManager.player.skills;
        initialPosition = scrollRect.normalizedPosition;
    }

    public void ShowSkillMasteryUI(E_ElementalType type)
    {
        this.gameObject.SetActive(true);

        skillPoints.text = "남은 스킬 포인트:" + $"\n {GameManager.player.SkillPoints}";

        switch (type)
        {
            case E_ElementalType.Fire: fireMastery.SetActive(true); break;
            case E_ElementalType.Ice: iceMastery.SetActive(true); break;
            case E_ElementalType.Nature: natureMastery.SetActive(true); break;
            case E_ElementalType.Physical: physicalMastery.SetActive(true); break;
        }
    }

    public void ResetSkillMasteryUI()
    {
        curSlot = null;
        fireMastery.SetActive(false);
        iceMastery.SetActive(false);
        natureMastery.SetActive(false);
        physicalMastery.SetActive(false);
        infoPanel.SetActive(false);
        ResetSkillInfo();
        scrollRect.normalizedPosition = initialPosition;

        this.gameObject.SetActive(false);
    }

    public void SetInfoUI(SkillSlot slot)
    {
        curSlot = slot;

        DialogueManager.Instance.ChangeCurDialogue($"{slot.Data.ID}");
        DialogueManager.Instance.DialogueUI.PassTyping();

        RefreshUI();
        infoPanel.SetActive(true);
    }

    public void RefreshUI()
    {
        SkillData data = curSlot.Data;
        SkillStatus status = sInventory.GetSkillStatus(data);
        skillPoints.text = "남은 스킬 포인트:" + $"\n {GameManager.player.SkillPoints}";

        ResetSkillInfo();

        skillName.text = data.Name;

        if (curSlot.IsLocked())
        {
            level.text = $"미학습";

            learnBtn.gameObject.SetActive(false);
            forgetBtn.gameObject.SetActive(false);
        }
        else
        {
            float powerValue = status != null ? status.Power : data.Power;
            power.text = $"배율: {powerValue * 100f:F1}%";

            range.text = $"범위: {data.Range}";
            cooldown.text = $"쿨다운: {data.Cooldown}";
            cost.text = $"마나 소모량: {data.Cost}";

            if (sInventory.HasSkill(data))
            {
                level.text = $"Lv.{status.Level}";
            }
            else
            {
                level.text = $"미학습";
            }

            SetButtons(status, data);
        }
    }

    public void ResetSkillInfo()
    {
        skillName.text = string.Empty;
        level.text = string .Empty;
        power.text = string.Empty;
        range.text = string .Empty;
        cooldown.text = string.Empty;
        cost.text = string.Empty;
    }

    public void SetButtons(SkillStatus status, SkillData data)
    {
        learnBtn.onClick.RemoveAllListeners();
        forgetBtn.onClick.RemoveAllListeners();

        learnBtn.gameObject.SetActive(true);

        if (status == null)
        {
            learnBtn.onClick.AddListener(() => OnLearnButton(data));
            learnBtnText.text = "학습하기";

            forgetBtn.gameObject.SetActive(false);
        }
        else
        {
            learnBtn.onClick.AddListener(() => OnUpgradeButton(status));
            learnBtnText.text = "레벨 업";

            if (status.Level >= 2)
            {
                forgetBtn.gameObject.SetActive(true);
                forgetBtn.onClick.AddListener(() => OnForgetButton(status));
                forgetBtnText.text = "레벨 다운";
            }
            else
            {
                forgetBtn.gameObject.SetActive(false);
            }
        }
    }

    private void OnLearnButton(SkillData data)
    {
        PlayerStatus player = GameManager.player;

        if (player.SkillPoints > 0)
        {
            if (sInventory.AddSkill(data)) player.SkillPoints--;
        }

        RefreshUI();
    }
     
    private void OnUpgradeButton(SkillStatus status)
    {
        PlayerStatus player = GameManager.player;

        if (player.SkillPoints > 0)
        {
            if (status.LevelUP()) player.SkillPoints--;
        }

        RefreshUI();
    }

    private void OnForgetButton(SkillStatus status)
    {
        PlayerStatus player = GameManager.player;

        if (status.LevelDown()) player.SkillPoints++;

        RefreshUI();
    }

}

