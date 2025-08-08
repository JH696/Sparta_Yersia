using TMPro;
using UnityEngine;

public class InteractCensor : MonoBehaviour
{
    [Header("감지된 타겟")]
    [SerializeField] private IInteractable target;

    [Header("상호작용 UI")]
    [SerializeField] private GameObject interactUI;
    [SerializeField] private TextMeshProUGUI interactText;

    public IInteractable GetTarget()
    {
        return target;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            target = other.GetComponent<IInteractable>();

            if (target == null) return;

            interactText.text = $"{target.InteractText()}";
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            if (target == null) return;

            Vector3 offset = new Vector3(0f, 1.5f, 0f);
            Vector3 worldPosition = transform.position + offset;

            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
            interactUI.transform.position = screenPos;

            interactUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            target = null;

            interactUI.SetActive(false);
            interactText.text = string.Empty;
        }
    }
}
