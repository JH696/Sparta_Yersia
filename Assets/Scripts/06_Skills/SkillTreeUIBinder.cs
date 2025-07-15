using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkillTreeUIBinder : MonoBehaviour
{
    [Header("UI 세팅")]
    [SerializeField] private RectTransform contentParent;
    [SerializeField] private Button skillBtnPrefab;

    [Header("자동할당")]
    [SerializeField] private CharacterSkill characterSkill;
    [SerializeField] private PlayerSkillController playerSkillController;

    private readonly List<Button> spawnBtns = new List<Button>();

    private void Awake()
    {
        if (characterSkill == null)
        {
            characterSkill = FindObjectOfType<CharacterSkill>();
        }

        if (playerSkillController == null)
        {
            playerSkillController = FindObjectOfType<PlayerSkillController>();
        }
    }

    private void Start()
    {
        BindSkillTree();
    }

    // CharacterSkill에 있는 스킬 상태만큼 버튼을 생성
    private void BindSkillTree()
    {
        // 기존 버튼 제거
        foreach (var btn in spawnBtns)
        {
            Destroy(btn.gameObject);
        }
        spawnBtns.Clear();

        // 리스트만큼 새로 생성
        foreach (var skillStatus in characterSkill.AllStatuses)
        {
            // 버튼 프리팹 복제
            var btnInstance = Instantiate(skillBtnPrefab, contentParent);
            spawnBtns.Add(btnInstance);

            var view = btnInstance.GetComponent<PlayerSkillView>();

            // 아이콘 세팅
            view.IconImg.sprite = skillStatus.Data.Icon;
            view.LockedOverlay.gameObject.SetActive(!skillStatus.IsUnlocked);

            // 레벨 텍스트
            view.LevelTxt.text = skillStatus.IsUnlocked ? $"Lv.{skillStatus.Level}" : string.Empty;

            // 장착 상태 아웃라인 효과
            view.EquippedOutline.enabled = playerSkillController.IsEquipped(skillStatus.Data.Id);

            // 상태 변화 콜백 등록
            view.UseBtn.onClick.AddListener(() =>
            {
                if (playerSkillController.IsEquipped(skillStatus.Data.Id))
                {
                    playerSkillController.UnEquipSkill(skillStatus.Data.Id);
                }
                else
                {
                    playerSkillController.EquipSkill(skillStatus.Data.Id);
                }

                UpdateSkillTreeBtn(skillStatus, view);
            });

            var txtComponent = view.UseBtn.GetComponentInChildren<Text>();
            bool unlocked = skillStatus.IsUnlocked;
            view.UseBtn.interactable = unlocked;

            if (unlocked)
            {
                txtComponent.text = playerSkillController.IsEquipped(skillStatus.Data.Id) ? "해제" : "장착";
            }
            else
            {
                txtComponent.text = string.Empty;
            }

            // 레벨업 버튼 설정
            view.LevelUpBtn.onClick.AddListener(() =>
            {
                if (playerSkillController.LevelUpSkill(skillStatus.Data.Id))
                {
                    view.LevelTxt.text = $"Lv.{skillStatus.Level}";
                }
            });
            view.LevelUpBtn.interactable = skillStatus.IsUnlocked;

            // 상태 변경 이벤트
            skillStatus.OnStateChanged += status => UpdateSkillTreeBtn(status, view);
            skillStatus.OnLevelChanged += status => view.LevelTxt.text = $"Lv.{status.Level}";
        }
    }

    // 스킬 상태 변화에 따라 버튼 UI 갱신
    private void UpdateSkillTreeBtn(SkillStatus status, PlayerSkillView view)
    {
        bool isUnlocked = status.IsUnlocked;
        bool isEquipped = isUnlocked && playerSkillController.IsEquipped(status.Data.Id);

        view.LockedOverlay.gameObject.SetActive(!isUnlocked);
        view.EquippedOutline.enabled = isEquipped;

        // UseButton 버튼 텍스트, 활성화
        view.UseBtn.GetComponentInChildren<Text>().text = isUnlocked
                ? (isEquipped ? "해제" : "장착")
                : string.Empty;
        view.UseBtn.interactable = isUnlocked;

        view.LevelUpBtn.interactable = isUnlocked;
    }
}
