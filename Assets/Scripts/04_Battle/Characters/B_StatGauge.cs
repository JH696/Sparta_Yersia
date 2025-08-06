using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class B_StatGauge : MonoBehaviour
{
    private B_Slot slot;

    [Header("체력 / 마나 텍스트")]
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI mpText;

    [Header("포인터")]
    [SerializeField] private GameObject pointer;

    [Header("체력 / 마나 게이지")]
    [SerializeField] private Image hpGauge;
    [SerializeField] private Image mpGauge;

    [Header("행동력 게이지")]
    [SerializeField] private Image apGauge;

    public B_Slot Slot => slot;

    public void SetGauges(B_Slot slot)
    {
        this.slot = slot;
        slot.Character.stat.StatusChanged += RefreshGauge;

        RefreshGauge();
        this.gameObject.SetActive(true);

        // RectTransform 가져오기
        Canvas canvas = GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        RectTransform thisRect = GetComponent<RectTransform>();

        // 월드 좌표 → 스크린 좌표
        Vector2 screenPos = BattleManager.Instance.BattleCamera.WorldToScreenPoint(slot.transform.position);

        // 사용할 카메라 가져오기 (Canvas에 설정된 Render Camera)
        Camera renderCam = (canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : canvas.worldCamera;

        // 스크린 좌표 → 캔버스 로컬 좌표
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, renderCam, out Vector2 localPoint))
        {
            thisRect.localPosition = localPoint;
        }
    }

    public void ResetGauge()
    {
        slot.Character.stat.StatusChanged -= RefreshGauge;

        hpGauge.fillAmount = 0;
        hpText.text = "";
        mpGauge.fillAmount = 0;
        mpText.text = "";

        this.gameObject.SetActive(false);
    }

    public void RefreshGauge()
    {
        CharacterStats stats = slot.Character.stat;

        hpGauge.fillAmount = stats.CurrentHp / stats.MaxHp;
        hpText.text = $"{stats.CurrentHp} / {stats.MaxHp}";
        mpGauge.fillAmount = stats.CurrentMana / stats.MaxMana;
        mpText.text = $"{stats.CurrentMana} / {stats.MaxMana}";
    }

    public void RefreshAPGauge(float amount)
    {
        apGauge.fillAmount = amount / 100;
    }

    public void ShowPointer()
    {
        pointer.gameObject.SetActive(true);
    }

    public void HidePointer()
    {
        pointer.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        slot.Character.stat.StatusChanged -= RefreshGauge;
    }
}
