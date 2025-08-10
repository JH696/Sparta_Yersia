using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;

    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private RectTransform backgroundRect;

    private void Awake()
    {
        Instance = this;
        HideTooltip();
    }

    private void Update()
    {
        // 마우스 위치 따라다니기
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent.GetComponent<RectTransform>(),
            Input.mousePosition,
            null,
            out localPoint);

        transform.localPosition = localPoint + new Vector2(50, -50);
    }

    public void ShowTooltip(string message)
    {
        gameObject.SetActive(true);
        tooltipText.text = message;

        // 텍스트에 맞춰 배경 크기 조정
        Vector2 size = tooltipText.GetPreferredValues(message);
        backgroundRect.sizeDelta = size + new Vector2(16, 8);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}