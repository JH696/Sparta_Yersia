using TMPro;
using UnityEngine;

public class UtillityButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;

    public void SetUtillityButton(string text)
    {
        buttonText.text = text;
    }

    public void OnUtillityButton()
    { 
        DialogueManager.Instance.ChangeCurDialogue("Utillity");
        DialogueManager.Instance.DialogueUI.PassTyping();
    }
}
