using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class B_DynamicButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image icon;

    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    public void SetColor(Color color)
    {
        icon.color = color;
    }

    public void SetText(string message)
    {
        text.text = message;
    }
}
