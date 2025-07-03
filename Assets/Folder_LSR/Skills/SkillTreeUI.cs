using UnityEngine;
using UnityEngine.UI;

public class SkillTreeUI : MonoBehaviour
{
    [Header("속성 타입 버튼")]
    [SerializeField] private Button fireButton;
    [SerializeField] private Button iceButton;
    [SerializeField] private Button natureButton;
    [SerializeField] private Button physicalButton;

    [Header("중앙 타입 아이콘")]
    [SerializeField] private Image centerTypeIcon;

    [Header("속성별 아이콘 스프라이트")]
    [SerializeField] private Sprite fireIcon;
    [SerializeField] private Sprite iceIcon;
    [SerializeField] private Sprite natureIcon;
    [SerializeField] private Sprite physicalIcon;

    [Header("노드·라인 컨테이너")]
    [SerializeField] private RectTransform nodeContainer;
    [SerializeField] private RectTransform lineContainer;

    [Header("풀 매니저")]
    [SerializeField] private SkillTreePool pool;

    [Header("상세 패널")]
    [SerializeField] private SkillDetailUI detailUI;

    private SkillData[] allSkills;

    private void Awake()
    {
        // 버튼 이벤트 연결
        fireButton.onClick.AddListener(() => OnTypeBtnClick(ESkillType.Fire));
        iceButton.onClick.AddListener(() => OnTypeBtnClick(ESkillType.Ice));
        natureButton.onClick.AddListener(() => OnTypeBtnClick(ESkillType.Nature));
        physicalButton.onClick.AddListener(() => OnTypeBtnClick(ESkillType.Physical));

        allSkills = Resources.LoadAll<SkillData>("SkillDatas");
    }

    private void OnTypeBtnClick(ESkillType type)
    {
        switch (type)
        {
            case ESkillType.Fire:
                centerTypeIcon.sprite = fireIcon;
                break;
            case ESkillType.Ice:
                centerTypeIcon.sprite = iceIcon;
                break;
            case ESkillType.Nature:
                centerTypeIcon.sprite = natureIcon;
                break;
            case ESkillType.Physical:
                centerTypeIcon.sprite = physicalIcon;
                break;
            default:
                Debug.LogWarning("[SkillTreeUI] 알 수 없는 속성 타입: " + type);
                break;
        }

        //TODO: 노드 생성 / 제거로직 추가필요
    }

    //판넬 전체 표시
    public void Show()
    {
        gameObject.SetActive(true);
    }

    //판넬 전체 숨기기
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
