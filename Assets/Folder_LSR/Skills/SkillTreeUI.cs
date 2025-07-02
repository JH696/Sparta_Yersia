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

    private ESkillType currentType;
    private int availablePoints; // 플레이어가 가진 스킬 포인트

    private void Start()
    {
        //foreach (var btn in typeButtons)
        //{
        //    btn.onClick.AddListener(() => OnTypeClicked(~~~));
        //}
        RefreshTree();
    }

    private void ShowType(ESkillType type)
    {
        currentType = type;
        RefreshTree();
    }

    private void RefreshTree()
    {
        // 기존 노드들 지우기
        foreach (Transform t in nodesContainer) Destroy(t.gameObject);
    }

    private void SelectSkillNode(SkillData data)
    {
        //detailPanel.Show(data, availablePoints >= data.UnlockCost, OnUnlockClicked);
    }

    private void AttemptUnlock()
    {

    }

    private void AttemptLevelUp()
    {

    }
}
