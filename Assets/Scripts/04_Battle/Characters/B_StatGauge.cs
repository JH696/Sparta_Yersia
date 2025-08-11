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

    public void SetGauges(B_Slot slot, E_SizeType size)
    {
        // 이벤트 중복 방지
        if (this.slot != null)
            this.slot.Character.stat.StatusChanged -= RefreshGauge;

        this.slot = slot;
        slot.Character.stat.StatusChanged += RefreshGauge;

        RefreshGauge();
        gameObject.SetActive(true);

        // 필수 컴포넌트
        Canvas canvas = GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        RectTransform thisRect = GetComponent<RectTransform>();

        // 월드→스크린: 반드시 '해당 오브젝트를 그리는 월드 카메라' 사용
        Camera worldCam = BattleManager.Instance.BattleCamera;
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(worldCam, slot.transform.position);

        // 픽셀 기준 Y 오프셋
        float y = size switch
        {
            E_SizeType.Small => 150f,
            E_SizeType.Medium => 150f,
            _ => 200f
        };
        screenPos.y += y;

        // 스크린→캔버스 로컬: Screen Space - Overlay면 cam=null, Camera/World면 canvas.worldCamera
        Camera uiCam = (canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : canvas.worldCamera;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, uiCam, out var localPoint))
        {
            // 카메라 스케일(Scale With Screen Size)일 땐 anchoredPosition을 써야 스케일러가 자동 반영됩니다.
            thisRect.anchoredPosition = localPoint;
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
        hpText.text = $"{(int)stats.CurrentHp} / {stats.MaxHp}";
        mpGauge.fillAmount = stats.CurrentMana / stats.MaxMana;
        mpText.text = $"{(int)stats.CurrentMana} / {stats.MaxMana}";
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
