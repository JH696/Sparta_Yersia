using UnityEngine;
using UnityEngine.UI;

public class GenderSelectUI : MonoBehaviour
{
    [Header("이 패널을 끄고 켤 CanvasGroup")]
    [SerializeField] private CanvasGroup panel;

    [Header("이후에 띄워줄 이름 입력 UI")]
    [SerializeField] private GameObject nameInputUI;

    [Header("Player 컴포넌트")]
    [SerializeField] private Player player;

    [Header("남자용 SO")]
    [SerializeField] private PlayerData malePlayerData;
    [Header("여자용 SO")]
    [SerializeField] private PlayerData femalePlayerData;

    [Header("월드 씬 스프라이트 미리보기")]
    [SerializeField] private Image worldPreviewImage;

    [Header("일러스트 스프라이트 미리보기")]
    [SerializeField] private Image dialogSprite;

    [Header("배틀 씬 스프라이트 미리보기")]
    [SerializeField] private Image battlePreviewImage;

    private NameInputUI nameInputComp;
    private PlayerData selectedData;

    private void Awake()
    {
        panel.alpha = 1;
        panel.interactable = true;
        panel.blocksRaycasts = true;

        nameInputUI.SetActive(false);
        nameInputComp = nameInputUI.GetComponent<NameInputUI>();

        selectedData = malePlayerData;
        PreviewGender(malePlayerData);
    }

    // Preview 버튼 핸들러
    public void OnPreviewMale() => PreviewGender(malePlayerData);
    public void OnPreviewFemale() => PreviewGender(femalePlayerData);

    private void PreviewGender(PlayerData data)
    {
        selectedData = data;
        bool isExpert = data.Rank == E_Rank.Expert;

        // 월드용
        worldPreviewImage.sprite =
            isExpert
                ? data.darkWorldSprite
                : data.brownWorldSprite;

        // 대화용
        dialogSprite.sprite =
            isExpert
                ? data.darkDialogSprite
                : data.brownDialogSprite;

        // 배틀용
        var visuals = isExpert
                ? data.darkBattleVisuals
                : data.brownBattleVisuals;
        battlePreviewImage.sprite = visuals.Stand;
    }

    // Confirm 버튼 핸들러
    public void OnConfirm()
    {
        if (selectedData == null) return;

        // 실제 PlayerData 교체
        player.SetPlayerData(selectedData);
        player.ChangeSprite();

        // 패널 닫고 이름 입력 UI 열기
        panel.alpha = 0;
        panel.interactable = false;
        panel.blocksRaycasts = false;
        nameInputUI.SetActive(true);

        // NameInputUI에 미리보기 스프라이트 전달
        nameInputComp.SetPreviewSprite(
            selectedData.Rank == E_Rank.Expert
                ? selectedData.darkWorldSprite
                : selectedData.brownWorldSprite
        );

        var statsUI = FindObjectOfType<StatsUI>();
        var playerUI = FindObjectOfType<PlayerUI>();
        nameInputComp.Init(player, statsUI, playerUI);
    }
}