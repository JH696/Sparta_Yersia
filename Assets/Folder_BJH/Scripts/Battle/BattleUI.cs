using UnityEngine;

public class BattleUI : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] private GameObject btnParent;
    [SerializeField] private B_AButtons actionBtns;

    [Header("월드 스페이스 캔버스")]
    [SerializeField] private Canvas canvas;

    public void SetButton(Transform transform)
    {
        actionBtns.SetActionButton();

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (canvasRect, screenPos, Camera.main, out localPoint);

        btnParent.GetComponent<RectTransform>().localPosition = localPoint;
    }
}
