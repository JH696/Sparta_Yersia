using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("플레이어 참조")]
    [SerializeField] private Player player;

    [Header("텍스트 UI")]
    [SerializeField] private TMP_Text NameTxt;
    [SerializeField] private TMP_Text LevelRankTxt;

    [Header("게이지 이미지")]
    [SerializeField] private Image HpGauge;
    [SerializeField] private Image ManaGauge;
    [SerializeField] private Image ExpGauge;

    [Header("프로필 이미지")]
    [SerializeField] private Image ProfileImg;

    private void Start()
    {
        if (player == null || player.Status == null) return;

        // 프로필 초기화 (플레이어가 할당되어 있을 경우)
        if (ProfileImg != null)
            ProfileImg.sprite = player.Status.GetProfileIcon();

        // 이름, 레벨 / 랭크 텍스트
        if (NameTxt != null)
            NameTxt.text = player.Status.PlayerName;

        if (LevelRankTxt != null)
        {
            int level = player.Status.stat.Level;
            string rank = player.Status.PlayerData.Rank.ToString();
            LevelRankTxt.text = $"Lv.{level} / {rank}";
        }

        // UI 동기화 이벤트 등록
        player.Status.stat.StatusChanged += RefreshUI;

        RefreshUI();
    }

    public void RefreshUI()
    {
        var stats = player.Status.stat;

        if (HpGauge != null) HpGauge.fillAmount = stats.CurrentHp / stats.MaxHp;
        if (ManaGauge != null) ManaGauge.fillAmount = stats.CurrentMana / stats.MaxMana;
        if (ExpGauge != null) ExpGauge.fillAmount = (float)stats.Exp / stats.MaxExp;

        if (LevelRankTxt != null && player.Status.PlayerData != null)
        {
            string rankText = string.Empty;

            switch (player.Status.PlayerData.Rank)
            {
                case E_Rank.Basic:
                    rankText = "초급 마법사";
                    break;
                case E_Rank.Advanced:
                    rankText = "중급 마법사";
                    break;
                case E_Rank.Expert:
                    rankText = "상급 마법사";
                    break;
                default:
                    rankText = "알 수 없음";
                    break;
            }

            LevelRankTxt.text = $"Lv {stats.Level} / {rankText}";
        }

        if (NameTxt != null)
        {
            NameTxt.text = player.Status.PlayerName;
        }
    }


    public void ShowPlayerUI()
    {
        this.gameObject.SetActive(true);
    }

    public void HidePlayerUI()
    {
        this.gameObject.SetActive(false);
    }

    public void SetProfileIcon(Sprite icon)
    {
        if (ProfileImg != null)
            ProfileImg.sprite = icon;
    }
}

static class ImageExtensions
{
    public static void SetFillAmount(this Image img, float f) { if (img != null) img.fillAmount = f; }
}