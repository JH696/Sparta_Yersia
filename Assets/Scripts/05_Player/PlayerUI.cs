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
        // 프로필 초기화 (플레이어가 할당되어 있을 경우)
        if (player != null && player.Status != null && ProfileImg != null)
        {
            ProfileImg.sprite = player.PlayerData.Icon;
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