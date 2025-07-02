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
        // 초기화
    }

    private void UpdateNodeUI()
    {
        // 락 해제 가능 상태
        // 락 상태
    }

    private IEnumerator AnimateEffect()
    {
        // 효과 애니메이션 구현
        yield return new WaitForSeconds(0.5f);
    }

    private void OnNodeClick()
    {
        skillTreeUI.SelectSkill(skillData);
    }
}
