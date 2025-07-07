using UnityEngine;
using UnityEngine.UI;

public class ActionGauge : MonoBehaviour
{
    [SerializeField] private GameObject charSlot;
    [SerializeField] private Image actionGauge;

    private void Start()
    {
        SetPosition();
        charSlot.GetComponent<CharacterSlot>().LinkGauge(this); 
    }

    private void SetPosition()
    {
        RectTransform canvasRect = Test_BattleManager.Instance.battleUI.canvas.GetComponent<RectTransform>();
        Vector2 screenPos = Camera.main.WorldToScreenPoint(charSlot.transform.position);

        // 2. 스크린 → 캔버스 로컬 좌표
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            Camera.main,
            out localPoint
        );

        // 3. y 위치를 위로 1.5f만큼 올림
        localPoint.y += 1.5f;

        // 4. 게이지 위치 설정
        this.GetComponent<RectTransform>().anchoredPosition = localPoint;
    }

    public void RefreshGauge(float amount)
    {
        actionGauge.fillAmount = Mathf.Clamp01(amount / 100f);
    }

    public void HideGauge()
    {
        this.gameObject.SetActive(false);
    }
}
