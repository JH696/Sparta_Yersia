using UnityEngine;

public class BattleUI : MonoBehaviour
{
    [SerializeField] private ActionButtons actionButton;
    [SerializeField] private Canvas canvas;
 

    void Start()
    {
        actionButton.SetCharacter(GameManager.Instance.TurnCharacter.turnChar);
        DisplayActionButton();
    }

    public void DisplayActionButton()
    {
        actionButton.gameObject.SetActive(true);    

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 screenPos = Camera.main.WorldToScreenPoint(GameManager.Instance.TurnCharacter.gameObject.transform.position);

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos,Camera.main, out localPoint);

        actionButton.GetComponent<RectTransform>().anchoredPosition = localPoint;
    }
}
