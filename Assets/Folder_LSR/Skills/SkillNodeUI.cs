using UnityEngine;
using UnityEngine.UI;

public class SkillNodeUI : MonoBehaviour
{
    [SerializeField] private Image iconImg;
    [SerializeField] private Button btn;
    [SerializeField] private GameObject lockOverlay;

    private SkillData data;
    private SkillDetailUI detailUI;
    private PlayerSkillController skillController;

    private void Awake()
    {
        skillController = FindObjectOfType<PlayerSkillController>();
    }

    // 스킬 데이터, 상세UI 전달 받아 세팅
    public void SetData(SkillData skillData, SkillDetailUI detailUI)
    {
        this.data = skillData;
        this.detailUI = detailUI;

        iconImg.sprite = data.Icon;

        // 잠금 상태
        RefreshLockState();

        // 클릭시 상세패널 표시
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => detailUI.Show(data, this));
    }

    public void SetUnlocked(bool unlocked)
    {
        iconImg.color = unlocked ? Color.white : new Color(1f, 1f, 1f, 0.7f);
    }

    public void RefreshLockState()
    {
        bool unlocked = skillController.IsUnlocked(data);
        btn.interactable = unlocked;
        iconImg.color = unlocked ? Color.white : new Color(1f, 1f, 1f, 0.7f);
        lockOverlay.SetActive(!unlocked);
    }
}
