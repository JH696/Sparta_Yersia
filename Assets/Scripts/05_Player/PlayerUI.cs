using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("플레이어 참조")]
    [SerializeField] private Player player;

    [Header("게이지 이미지")]
    [SerializeField] private Image HpGauge;
    [SerializeField] private Image ManaGauge;
    [SerializeField] private Image ExpGauge;

    [Header("프로필 이미지")]
    [SerializeField] private Image ProfileImg;

    private void Start()
    {
        player = GameManager.Instance.Player.GetComponent<Player>();

        // 프로필 초기화 (플레이어가 할당되어 있을 경우)
        if (player != null && player.PlayerData != null && ProfileImg != null)
        {
            ProfileImg.sprite = player.PlayerData.Icon;
        }
    }

    private void Update()
    {
        if (player == null) return;

        // HP 게이지 반영
        if (HpGauge != null)
        {
            float ratio = player.MaxHp > 0f ? player.CurrentHp / player.MaxHp : 0f;
            HpGauge.fillAmount = Mathf.Clamp01(ratio);
        }

        // Mana 게이지 반영
        if (ManaGauge != null)
        {
            float ratio = player.MaxMana > 0f ? player.CurrentMana / player.MaxMana : 0f;
            ManaGauge.fillAmount = Mathf.Clamp01(ratio);
        }

        // 경험치 게이지 반영
        if (ExpGauge != null && player is ILevelable levelable)
        {
            float ratio = levelable.ExpToNextLevel > 0
                ? (float)levelable.CurrentExp / levelable.ExpToNextLevel
                : 0f;
            ExpGauge.fillAmount = Mathf.Clamp01(ratio);
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
}