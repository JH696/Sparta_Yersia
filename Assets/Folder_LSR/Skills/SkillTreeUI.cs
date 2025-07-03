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
    [SerializeField] private TextMeshProUGUI skillNameTxt;
    [SerializeField] private TextMeshProUGUI skillDescTxt;
    [SerializeField] private Image skillIconImg;
    [SerializeField] private TextMeshProUGUI skillCostTxt;
    [SerializeField] private Button unlockBtn;
    [SerializeField] private Button levelUpBtn;
    [SerializeField] private TextMeshProUGUI skillPointTxt;

    [Header("현재 타입 필터")]
    [SerializeField] private ESkillType currentType = ESkillType.Fire;

    private PlayerSkillController playerSkillController;
    private List<SkillNodeUI> spawnedNodes = new List<SkillNodeUI>();
    private SkillData selectedSkill;

    private void Start()
    {
        playerSkillController = FindObjectOfType<PlayerSkillController>();
        unlockBtn.onClick.AddListener(AttemptUnlock);
        levelUpBtn.onClick.AddListener(AttemptLevelUp);

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
            nodeUI.Setup(this, selectedSkill, playerSkillController);
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
        skillNameTxt.text = data.DisplayName;
        skillDescTxt.text = data.Description;
        skillIconImg.sprite = data.Icon;
        skillCostTxt.text = $"해금 비용: {data.BaseUnlockCost} p";

        bool isUnlocked = playerSkillController.HasSkillUnlocked(data.SkillID);
        unlockBtn.gameObject.SetActive(!isUnlocked);
        levelUpBtn.gameObject.SetActive(!isUnlocked);

        RefreshSkillPoint();
    }

    private void HideDetailPanel()
    {
        selectedSkill = null;
        skillNameTxt.text = string.Empty;
        skillDescTxt.text = string.Empty;
        skillIconImg.sprite = null;
        skillCostTxt.text = string.Empty;
        unlockBtn.gameObject.SetActive(false);
        levelUpBtn.gameObject.SetActive(false);
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
        skillPointTxt.text = $"남은 스킬 포인트: {playerSkillController.AvailableSkillPoints:N0}";
    }
}
