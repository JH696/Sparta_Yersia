using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeUI : MonoBehaviour
{
    [Header("스킬 노드 프리팹")]
    [SerializeField] private GameObject skillNodePrefab;

    [Header("노드 생성 부모 오브젝트")]
    [SerializeField] private Transform nodeContainer;

    [Header("스킬 상세 정보 UI")]
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI skillDescText;
    [SerializeField] private Image skillIconImage;
    [SerializeField] private TextMeshProUGUI skillCostText;
    [SerializeField] private Button unlockButton;
    [SerializeField] private Button levelUpButton;
    [SerializeField] private TextMeshProUGUI skillPointText;

    [Header("현재 타입 필터")]
    [SerializeField] private ESkillType currentType = ESkillType.Fire;

    private PlayerSkillController playerSkillController;
    private List<SkillNodeUI> spawnedNodes = new List<SkillNodeUI>();
    private SkillData selectedSkill;

    private void Start()
    {
        playerSkillController = FindObjectOfType<PlayerSkillController>();
        unlockButton.onClick.AddListener(AttemptUnlock);
        levelUpButton.onClick.AddListener(AttemptLevelUp);

        RefreshSkillTree();
        RefreshSkillPoint();
        HideDetailPanel();
    }

    public void ChangeType(int typeIndex)
    {
        currentType = (ESkillType)typeIndex;
        RefreshSkillTree();
        HideDetailPanel();
    }

    private void RefreshSkillTree()
    {
        foreach (var node in spawnedNodes)
        {
            Destroy(node.gameObject);
        }
        spawnedNodes.Clear();

        var skills = SkillLibrary.Instance.GetSkillsByType(currentType);

        foreach (var skill in skills)
        {
            var nodeObj = Instantiate(skillNodePrefab, nodeContainer);
            var nodeUI = nodeObj.GetComponent<SkillNodeUI>();
            //nodeUI.Setup(this, data, playerSkillController);
            spawnedNodes.Add(nodeUI);
        }
    }

    public void SelectSkill(SkillData data)
    {
        selectedSkill = data;
        ShowDetailPanel(data);
    }

    private void ShowDetailPanel(SkillData data)
    {
        skillNameText.text = data.DisplayName;
        skillDescText.text = data.Description;
        skillIconImage.sprite = data.Icon;
        skillCostText.text = $"해금 비용: {data.BaseUnlockCost} p";

        bool isUnlocked = playerSkillController.HasSkillUnlocked(data.SkillID);
        unlockButton.gameObject.SetActive(!isUnlocked);
        levelUpButton.gameObject.SetActive(!isUnlocked);

        RefreshSkillPoint();
    }

    private void HideDetailPanel()
    {
        selectedSkill = null;
        skillNameText.text = string.Empty;
        skillDescText.text = string.Empty;
        skillIconImage.sprite = null;
        skillCostText.text = string.Empty;
        unlockButton.gameObject.SetActive(false);
        levelUpButton.gameObject.SetActive(false);
    }

    // 스킬 레벨업 시도
    private void AttemptUnlock()
    {
        if (selectedSkill == null) return;

        if (!playerSkillController.CanUnlockSkill(selectedSkill))
        {
            Debug.Log("해금조건 불충족");
            return;
        }

        playerSkillController.UnlockSkill(selectedSkill);
        RefreshSkillTree();
        ShowDetailPanel(selectedSkill);
    }

    private void AttemptLevelUp()
    {
        if (selectedSkill == null) return;
        if (!playerSkillController.CanLevelUpSkill(selectedSkill))
        {
            Debug.Log("레벨업 조건 불충족");
            return;
        }

        playerSkillController.LevelUpSkill(selectedSkill);
        ShowDetailPanel(selectedSkill);
    }

    private void RefreshSkillPoint()
    {
        skillPointText.text = $"남은 스킬 포인트: {playerSkillController.AvailableSkillPoints:N0}";
    }
}
