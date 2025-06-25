using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    public GameObject DialogueBox;
    public TextMeshProUGUI Text;

    public void ToggleDialogueBox()
    {
        if (DialogueBox == null) return;

        DialogueBox.SetActive(!DialogueBox.activeSelf);
    }

    public void RefreshText(string text)
    {
        if (Text == null) return;

        Text.text = text;
    }
}
