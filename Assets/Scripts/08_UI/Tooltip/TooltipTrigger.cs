using UnityEngine;
using UnityEngine.EventSystems;
public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ITooltipHandler tooltipHandler;

    private void Awake()
    {
        tooltipHandler = GetComponent<ITooltipHandler>();

        if (tooltipHandler == null)
        {
            Debug.Log($"[TooltipTrigger] {gameObject.name}에 ITooltipHandler가 없습니다.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltipHandler != null)
        {
            TooltipUI.Instance.ShowTooltip(tooltipHandler.GetTooltipText());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.HideTooltip();
    }

    private void OnDisable()
    {
        TooltipUI.Instance.HideTooltip();
    }
}