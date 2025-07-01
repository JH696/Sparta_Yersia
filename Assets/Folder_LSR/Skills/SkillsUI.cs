using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillTreeUI : MonoBehaviour
{
    [SerializeField] private SkillLibrary library;
    [SerializeField] private RectTransform nodesContainer;
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private Button[] categoryButtons;
    [SerializeField] private TextMeshProUGUI skillPointText;
    //[SerializeField] private DetailPanel detailPanel;  // 자체 클래스로 빼야할거 같은데 일단 주석처리

    private ESkillType currentCategory = ESkillType.Fire;
    private int availablePoints; // 플레이어가 가진 스킬 포인트

    private void Start()
    {
        //foreach (var btn in typeButtons)
        //{
        //    btn.onClick.AddListener(() => OnTypeClicked(~~~));
        //}
        RefreshTree();
        UpdatePointsUI();
    }

    private void OnCategoryClicked(ESkillType type)
    {
        currentCategory = type;
        RefreshTree();
    }

    private void RefreshTree()
    {
        // 기존 노드들 지우기
        foreach (Transform t in nodesContainer) Destroy(t.gameObject);

        // 해당 카테고리 스킬들 가져오기
        var skills = library.GetByCategory(currentCategory);

        // Radial layout: Tier별 반경을 다르게, 수량에 맞춰 각도 분산
        foreach (var tier in Enum.GetValues(typeof(ESkillTier)).Cast<ESkillTier>())
        {
            var tierSkills = skills.Where(s => s.Tier == tier).ToArray();
            float radius = GetRadiusForTier(tier);
            for (int i = 0; i < tierSkills.Length; i++)
            {
                float angle = 90f - 360f * i / tierSkills.Length; // 6시 방향부터 시계방향으로 채우려면 조정
                Vector2 pos = new Vector2(
                  Mathf.Cos(angle * Mathf.Deg2Rad),
                  Mathf.Sin(angle * Mathf.Deg2Rad)
                ) * radius;
                var nodeGO = Instantiate(nodePrefab, nodesContainer);
                nodeGO.GetComponent<RectTransform>().anchoredPosition = pos;
                //var node = nodeGO.GetComponent<SkillNode>();
                //node.Initialize(tierSkills[i], OnNodeClicked, IsUnlocked(tierSkills[i]));
            }
        }
    }

    private float GetRadiusForTier(ESkillTier tier)
    {
        switch (tier)
        {
            case ESkillTier.Beginner: return 100f;
            case ESkillTier.Intermediate: return 200f;
            case ESkillTier.Advanced: return 300f;
            default: return 0f;
        }
    }

    //private bool IsUnlocked(SkillData skillData)
    //{
    //    // TODO: 플레이어 데이터에서 해당 스킬 해금 여부 체크
    //    //return PlayerSkillController.Instance.HasSkill(skillData);
    //}

    private void OnNodeClicked(SkillData skillData)
    {
        //detailPanel.Show(skillData, availablePoints >= skillData.UnlockCost, OnUnlockClicked);
    }

    private void OnUnlockClicked(SkillData skillData)
    {
        if (availablePoints >= skillData.UnlockCost)
        {
            //PlayerSkillController.Instance.LearnSkill(skillData);
            availablePoints -= skillData.UnlockCost;
            UpdatePointsUI();
            RefreshTree();
            //detailPanel.Hide();
        }
    }

    private void UpdatePointsUI()
    {
        skillPointText.text = $"스킬 포인트: {availablePoints}";
    }
}
