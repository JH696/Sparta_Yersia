using UnityEngine;
using UnityEngine.UI;

public class B_ActionGauge : MonoBehaviour
{
    [Header("게이지 이미지")]
    [SerializeField] private Image img;

    [Header("월드 스페이스 캔버스")]
    [SerializeField] private Canvas canvas;

    public void SetGauge(B_CharacterSlot slot)
    {
        slot.LinkActionGauge(this);

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 screenPos = Camera.main.WorldToScreenPoint(slot.gameObject.transform.position);

        // 이 UI의 위치를 대상 오브젝트(슬롯) 트랜스폼 조금 위로 이동
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (canvasRect, screenPos, Camera.main, out localPoint);

        localPoint.y += 1.5f;
        
        this.GetComponent<RectTransform>().localPosition = localPoint;
    }

    public void RefreshGauge(float amount)
    {
        img.fillAmount = amount / 100f;
    }
}
